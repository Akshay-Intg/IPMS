using Ipms.Frontend.DTOs.Deserialize;
using Ipms.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Ipms.Frontend.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly HttpClient _http;
        private readonly string? _apiBaseUrl;
        private readonly IConfiguration _configuration;
        public ClaimsController(IHttpClientFactory httpFactory, IConfiguration configuration)
        {
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
            _http = httpFactory.CreateClient();
            _configuration = configuration;
        }
        private void AttachToken()
        {
            var token = HttpContext.Session.GetString("JWToken");
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
        [HttpGet]
        public IActionResult Apply(int id)
        {
            var viewModel = new CreateClaimModel
            {
                PolicyId = id   
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Apply(CreateClaimModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var request = new HttpRequestMessage(HttpMethod.Post, _apiBaseUrl + "Claims/apply");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(model);
            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Claim Applied Successfully!!";
                return RedirectToAction("Dashboard", "Customer");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                TempData["Error"] = "Claim Already Applied on the Policy!";
                return View(model);
            }
            else if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                TempData["Error"] = "Claim amount cannot exceed sum assured";
                return View(model);
            }
            TempData["Error"] = "Apply failed.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetClaimsById(string? search, string? status, int page = 1, int pageSize = 10)
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Auth");

            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Session Expired. Please Login Again.";
                return RedirectToAction("Login", "Auth");
            }

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.GetAsync(_apiBaseUrl + "Claims/claimslist");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var claims = JsonSerializer.Deserialize<List<ClaimsDeserializeDTO>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

                if (!string.IsNullOrEmpty(search))
                {
                    var s = search.ToLower();
                    claims = claims.Where(c =>
                        c.ClaimId.ToString().Contains(s) ||
                        c.PolicyId.ToString().Contains(s) ||
                        (c.PolicyNumber ?? "").ToLower().Contains(s) ||
                        (c.Reason ?? "").ToLower().Contains(s) ||
                        (c.ClaimStatus ?? "").ToLower().Contains(s)
                    ).ToList();
                }

                if (!string.IsNullOrEmpty(status))
                {
                    claims = claims.Where(c =>
                        (c.ClaimStatus ?? "").Equals(status, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                var totalRecords = claims.Count;
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

                var paged = claims
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var viewModel = new ClaimsListViewModel
                {
                    Claims = paged,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalRecords = totalRecords,
                    PageSize = pageSize,
                    Search = search,
                    StatusFilter = status
                };

                return View(viewModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["NotFound"] = "Claims not available!";
                return View(new ClaimsListViewModel());
            }

            TempData["Error"] = "Unable to Load Claims!";
            return View(new ClaimsListViewModel());
        }




        [HttpGet("claimList")]
        public async Task<IActionResult> GetClaimsList(string? search, string? status, int page = 1, int pageSize = 10)
        {
            AttachToken();

            var response = await _http.GetAsync(_apiBaseUrl + "Claims/allclaims");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Unable to Load Claims!";
                return View(new ClaimsListViewModel());
            }

            var json = await response.Content.ReadAsStringAsync();
            var claims = JsonSerializer.Deserialize<List<ClaimsDeserializeDTO>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                claims = claims.Where(c =>
                    c.ClaimId.ToString().Contains(s) ||
                    c.PolicyId.ToString().Contains(s) ||
                    (c.PolicyNumber ?? "").ToLower().Contains(s) ||
                    (c.Reason ?? "").ToLower().Contains(s) ||
                    (c.ClaimStatus ?? "").ToLower().Contains(s)
                ).ToList();
            }

            if (!string.IsNullOrEmpty(status))
                claims = claims.Where(c =>
                    (c.ClaimStatus ?? "").Equals(status, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            var totalRecords = claims.Count;
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            var paged = claims
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new ClaimsListViewModel
            {
                Claims = paged,
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
        public async Task<IActionResult> GetClaimsByClaimId(int claimId)
        {
            AttachToken();
            var response = await _http.GetAsync(_apiBaseUrl + $"Claims/claimByClaimId/{claimId}");


            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var claimslist = JsonSerializer.Deserialize<ClaimsDeserializeDTO>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return View(claimslist);
            }
            TempData["Error"] = "Unable to Load Claim!";
            return View(new ClaimsDeserializeDTO());
        }
        [HttpPost]
        public async Task<IActionResult> UpdateClaimStatus(ClaimsDeserializeDTO model)
        {
            AttachToken();

            var response = await _http.PatchAsync(
                _apiBaseUrl + $"Claims/{model.ClaimId}",
                JsonContent.Create(model.ClaimStatus)
            );

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Claim status updated successfully!";
                return RedirectToAction("GetClaimsList");
            }

            TempData["Error"] = "Unable to update claim status!";
            return RedirectToAction("GetClaimsByClaimId", new { claimId = model.ClaimId });
        }
    }
}
