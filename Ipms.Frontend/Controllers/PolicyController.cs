using Ipms.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Ipms.Frontend.Controllers
{
    public class PolicyController : Controller
    {
        private readonly HttpClient _http;
        private readonly string? _apiBaseUrl;

        public PolicyController(IHttpClientFactory httpFactory, IConfiguration configuration)
        {
            _http = httpFactory.CreateClient();
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
        }

        private void AttachToken()
        {
            var token = HttpContext.Session.GetString("JWToken");
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        [HttpGet]
        public async Task<IActionResult> AllPolicies(string? search, string? status, int page = 1, int pageSize = 10)
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Auth");

            AttachToken();

            var response = await _http.GetAsync(_apiBaseUrl + "Policy/all");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to load policies.";
                return View(new PolicyListPagedViewModel());
            }

            var json = await response.Content.ReadAsStringAsync();
            var policies = JsonSerializer.Deserialize<List<PolicyListViewModel>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                policies = policies.Where(p =>
                    (p.PolicyNumber ?? "").ToLower().Contains(s) ||
                    (p.CustomerName ?? "").ToLower().Contains(s) ||
                    (p.InsuranceType ?? "").ToLower().Contains(s) ||
                    (p.SchemeName ?? "").ToLower().Contains(s) ||
                    (p.PolicyStatus ?? "").ToLower().Contains(s)
                ).ToList();
            }

            if (!string.IsNullOrEmpty(status))
                policies = policies.Where(p =>
                    (p.PolicyStatus ?? "").Equals(status, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            var totalRecords = policies.Count;
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            var paged = policies
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new PolicyListPagedViewModel
            {
                Policies = paged,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                PageSize = pageSize,
                Search = search,
                StatusFilter = status
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Review(int id)
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Auth");

            AttachToken();

            var response = await _http.GetAsync(_apiBaseUrl + $"Policy/underwriter/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Policy not found.";
                return RedirectToAction("AllPolicies");
            }

            var json = await response.Content.ReadAsStringAsync();
            var policy = JsonSerializer.Deserialize<PolicyUnderwriterViewModel>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(policy);
        }

        [HttpPost]
        public async Task<IActionResult> Review(int id, PolicyReviewModel model)
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Auth");

            AttachToken();

            var dto = new
            {
                premiumAmount = model.PremiumAmount,
                status = model.Status,
                remarks = model.Remarks
            };

            var json = System.Text.Json.JsonSerializer.Serialize(dto);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _http.PutAsync(_apiBaseUrl + $"Policy/review/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = $"Policy {model.Status} successfully!";
                return RedirectToAction("AllPolicies");
            }

            var error = await response.Content.ReadAsStringAsync();
            TempData["Error"] = $"Failed: {error}";
            return RedirectToAction("Review", new { id });
        }
    }
}