using Ipms.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Ipms.Frontend.Controllers
{
    public class BrokerController : Controller
    {
        private readonly HttpClient _http;
        private readonly string? _apiBaseUrl;
        private readonly IConfiguration _configuration;

        public BrokerController(IHttpClientFactory httpFactory, IConfiguration configuration)
        {
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
            _http = httpFactory.CreateClient();
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Auth");

            // Check role
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Broker")
                return RedirectToAction("Dashboard", "Customer");

            var token = HttpContext.Session.GetString("JWToken");
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // Get broker's sold policies
            var response = await _http.GetAsync(_apiBaseUrl + "Policy/my-policies");

            var vm = new BrokerDashboardViewModel
            {
                BrokerName = HttpContext.Session.GetString("CustomerName"),
                TotalSold = 0,
                Policies = new List<PolicyViewModel>()
            };

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var policies = JsonSerializer.Deserialize<List<PolicyViewModel>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                vm.Policies = policies ?? new List<PolicyViewModel>();
                vm.TotalSold = vm.Policies.Count;
                vm.TotalPremium = vm.Policies.Sum(p => p.PremiumAmount);
            }

            return View(vm);
        }
        
    }
}
