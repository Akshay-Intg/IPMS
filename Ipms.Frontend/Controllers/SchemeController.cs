using Ipms.Frontend.Helpers;
using Ipms.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Ipms.Frontend.Controllers
{
    public class SchemeController : Controller
    {
        private readonly HttpClient _http;
        private readonly string? _apiBaseUrl;

        public SchemeController(IHttpClientFactory httpFactory, IConfiguration configuration)
        {
            _http = httpFactory.CreateClient();
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
        }
        private bool IsAdminLoggedIn(out string token)
        {
            token = HttpContext.Session.GetString("JWToken") ?? string.Empty;
            if (string.IsNullOrEmpty(token)) return false;
            var role = JwtHelper.GetUserRole(token);
            return role == "Admin";
        }
        [HttpGet]
        public async Task<IActionResult> SchemeList()
        {
            if (!IsAdminLoggedIn(out var token))
                return RedirectToAction("Login", "Auth");

            var request = new HttpRequestMessage(HttpMethod.Get, _apiBaseUrl + "Scheme");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var schemes = JsonSerializer.Deserialize<List<InsuranceSchemeViewModel>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(schemes);
            }

            TempData["Error"] = "Failed to load schemes.";
            return View(new List<InsuranceSchemeViewModel>());
        }

        [HttpGet]
        public IActionResult CreateInsuranceScheme()
        {
            if (!IsAdminLoggedIn(out _))
                return RedirectToAction("Login", "Auth");

            return View(new InsuranceSchemeViewModel { IsActive = true });
        }

        [HttpPost]
        public async Task<IActionResult> CreateInsuranceScheme(InsuranceSchemeViewModel model)
        {
            if (!IsAdminLoggedIn(out var token))
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid) return View(model);

            var request = new HttpRequestMessage(HttpMethod.Post, _apiBaseUrl + "Scheme");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(model);
            var response = await _http.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<InsuranceSchemeViewModel>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Scheme created successfully!";
                return RedirectToAction("SchemeList");
            }

            TempData["Error"] = "Failed to create scheme. Name may already exist.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditScheme(int id)
        {
            if (!IsAdminLoggedIn(out var token))
                return RedirectToAction("Login", "Auth");

            var request = new HttpRequestMessage(HttpMethod.Get, _apiBaseUrl + $"Scheme/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var scheme = JsonSerializer.Deserialize<InsuranceSchemeViewModel>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(scheme);
            }

            TempData["Error"] = "Scheme not found.";
            return RedirectToAction("SchemeList");
        }

        [HttpPost]
        public async Task<IActionResult> EditScheme(int id, InsuranceSchemeViewModel model)
        {
            if (!IsAdminLoggedIn(out var token))
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid) return View(model);

            var request = new HttpRequestMessage(HttpMethod.Put, _apiBaseUrl + $"Scheme/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(model);
            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Scheme updated successfully!";
                return RedirectToAction("SchemeList");
            }

            TempData["Error"] = "Failed to update scheme.";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id, bool isActive)
        {
            if (!IsAdminLoggedIn(out var token))
                return RedirectToAction("Login", "Auth");

            var getRequest = new HttpRequestMessage(HttpMethod.Get, _apiBaseUrl + $"Scheme/{id}");
            getRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var getResponse = await _http.SendAsync(getRequest);

            if (!getResponse.IsSuccessStatusCode)
            {
                TempData["Error"] = "Scheme not found.";
                return RedirectToAction("SchemeList");
            }

            var json = await getResponse.Content.ReadAsStringAsync();
            var scheme = JsonSerializer.Deserialize<InsuranceSchemeViewModel>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            scheme!.IsActive = isActive;

            var putRequest = new HttpRequestMessage(HttpMethod.Put, _apiBaseUrl + $"Scheme/{id}");
            putRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            putRequest.Content = JsonContent.Create(scheme);
            var putResponse = await _http.SendAsync(putRequest);

            TempData[putResponse.IsSuccessStatusCode ? "Success" : "Error"] =
                putResponse.IsSuccessStatusCode
                    ? $"Scheme {(isActive ? "activated" : "deactivated")} successfully."
                    : "Failed to update scheme status.";

            return RedirectToAction("SchemeList");
        }
    }
}