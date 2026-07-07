using Ipms.Frontend.DTOs.Deserialize;
using Ipms.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Ipms.Frontend.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _http;
        private readonly string? _apiBaseUrl;
        private readonly IConfiguration configuration;
        public AdminController(IHttpClientFactory httpFactory, IConfiguration configuration)
        {
            this.configuration = configuration;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
            _http = httpFactory.CreateClient();
        }
        private void AttachToken()
        {
            var token = HttpContext.Session.GetString("JWToken");
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(string? search,int page=1,int pageSize=10)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");
            var request = new HttpRequestMessage(HttpMethod.Get, _apiBaseUrl + "Admin/all");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var profiles = JsonSerializer.Deserialize<List<CustomerListDeserialize>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();

                    profiles = profiles.Where(c =>
                        ((c.firstName ?? "") + " " + (c.lastName ?? "")).ToLower().Contains(search) ||

                        (c.email ?? "").ToLower().Contains(search) ||

                        (c.phoneNo ?? "").ToLower().Contains(search) ||

                        (c.city ?? "").ToLower().Contains(search) ||

                        (c.state ?? "").ToLower().Contains(search) ||

                        c.customerID.ToString().Contains(search)
                    ).ToList();
                }
                var totalRecords = profiles.Count;
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages))); 

                var paged = profiles
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var viewModel = new CustomerListViewModel
                {
                    Customers = paged,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalRecords = totalRecords,
                    PageSize = pageSize,
                    Search = search
                };


                return View(viewModel);
            }

            TempData["Error"] = "Unable to load profiles";
            return View(new CustomerListViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> EditCustomer(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            ViewBag.ZipcodeApiKey = configuration["ZipcodeBase:ApiKey"];
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var request = new HttpRequestMessage(HttpMethod.Get, _apiBaseUrl + $"Customer/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var profile = JsonSerializer.Deserialize<AdminEditViewModel>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(profile);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["Error"] = "Customer not found.";
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                TempData["Error"] = "Something went wrong.";
            }
            return RedirectToAction("GetAll","Admin");
        }

        [HttpPost]
        public async Task<IActionResult> EditCustomer(int id, AdminEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            ViewBag.ZipcodeApiKey = configuration["ZipcodeBase:ApiKey"];
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var request = new HttpRequestMessage(HttpMethod.Put, _apiBaseUrl + $"Admin/update/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(model);
            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("GetAll");

            TempData["Error"] = "Update failed.";
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Brokers()
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Auth");

            var token = HttpContext.Session.GetString("JWToken");
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.GetAsync(_apiBaseUrl + "Admin/brokers");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var brokers = JsonSerializer.Deserialize<List<BrokerViewModel>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return View(brokers ?? new List<BrokerViewModel>());
            }

            // ← Always pass empty list, never null
            return View(new List<BrokerViewModel>());
        }
        public IActionResult BrokerDashboard()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CreateBroker()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBroker(CreateBrokerModel model)
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Auth");

            var token = HttpContext.Session.GetString("JWToken");
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // Send only broker-specific fields
            var dto = new
            {
                model.FirstName,
                model.LastName,
                model.Email,
                model.Password,
                model.PhoneNo,
                model.LicenseNumber
            };

            var response = await _http.PostAsJsonAsync(_apiBaseUrl + "Admin/create-broker", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Broker created successfully!";
                return RedirectToAction("Brokers");
            }

            var error = await response.Content.ReadAsStringAsync();
            TempData["Error"] = "Failed to create broker. " + error;
            return RedirectToAction("Brokers");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleBrokerStatus(int brokerId)
        {
            var token = HttpContext.Session.GetString("JWToken");
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.PutAsync(
                _apiBaseUrl + $"Admin/toggle-broker/{brokerId}", null);

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Broker status updated.";
            else
                TempData["Error"] = "Failed to update broker status.";

            return RedirectToAction("Brokers");
        }

        [HttpPost]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.PutAsync(
                _apiBaseUrl + $"Admin/soft-delete/{id}", null);

            if (response.IsSuccessStatusCode)
                return Ok(new { success = true, message = "Customer deleted successfully." });

            return BadRequest(new { success = false, message = "Failed to delete customer." });
        }

        [HttpGet]
        public async Task<IActionResult> AuditLogs(string stringOrder, int pageNumber)
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Auth");

            AttachToken();

            var response = await _http.GetAsync(_apiBaseUrl + "AuditLog/all");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to load audit logs.";
                return View(new AuditPaginatedList<AuditLogViewModel>(new(), 0, 1, 10));
            }

            var json = await response.Content.ReadAsStringAsync();
            var logs = JsonSerializer.Deserialize<List<AuditLogViewModel>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

            if (pageNumber < 1)
                pageNumber = 1;
            string sortOrder = string.IsNullOrEmpty(stringOrder)
    ? "id_desc"
    : stringOrder;

            ViewData["CurrentSort"] = sortOrder;

            var isAsc = sortOrder == "id_asc";

            logs = isAsc
                ? logs.OrderBy(x => x.AuditId).ToList()
                : logs.OrderByDescending(x => x.AuditId).ToList();

            int pageSize = 10;

            return View(await AuditPaginatedList<AuditLogViewModel>
                .CreateAsync(logs, pageNumber, pageSize));
        }
    }
}
