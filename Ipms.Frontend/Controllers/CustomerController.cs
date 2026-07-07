using AutoMapper;
using BLL.DTOs;
using BLL.Services;
using DAL.Models;
using DAL.Repositories;
using Ipms.Frontend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;

namespace Ipms.Frontend.Controllers
{
    public class CustomerController : Controller
    {
        private readonly HttpClient _http;
        private readonly string? _apiBaseUrl;
        private readonly IConfiguration _configuration;

        public IMapper _mapper { get; }

        public CustomerController(IHttpClientFactory httpFactory, IConfiguration configuration,IMapper mapper)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration["ApiSettings:BaseUrl"];
            _http = httpFactory.CreateClient();
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Auth");
            

            var token = HttpContext.Session.GetString("JWToken");
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var profileResponse = await _http.GetAsync(_apiBaseUrl + "Customer/profile");

            var policyResponse = await _http.GetAsync(_apiBaseUrl + "Policy/my-policies");

            var vm = new Customerdashboard();

            if (profileResponse.IsSuccessStatusCode)
            {
                var json = await profileResponse.Content.ReadAsStringAsync();
                var profile = JsonSerializer.Deserialize<CustomerViewModel>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                vm.FullName = profile.firstName;
            }

            if (policyResponse.IsSuccessStatusCode)
            {
                var json = await policyResponse.Content.ReadAsStringAsync();
                var policies = JsonSerializer.Deserialize<List<PolicyViewModel>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (policies != null && policies.Any())
                {
                    vm.Policies = policies;
                    vm.TotalPolicies = policies.Count;
                    vm.TotalPremiumPaid = policies.Where(p => p.PolicyStatus == "Active").Sum(p => p.PremiumAmount);

                    var active = policies.FirstOrDefault(p => p.PolicyStatus == "Active");
                    if (active != null)
                    {
                        vm.PolicyNumber = active.PolicyNumber;
                        vm.InsuranceType = active.InsuranceType;
                        vm.PremiumAmount = active.PremiumAmount;
                        vm.SumAssured = active.SumAssured;
                        vm.PolicyTerm = active.PolicyTerm;
                        vm.PolicyDate = active.PolicyDate;
                        vm.MaturityDate = active.MaturityDate;
                        vm.PolicyStatus = active.PolicyStatus;
                        vm.NomineeName = active.NomineeName;
                        vm.PaymentStatus= active.PaymentStatus;
                    }
                }
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> PolicyDetails(int id)
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Auth");

            var token = HttpContext.Session.GetString("JWToken");
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.GetAsync(_apiBaseUrl + $"Policy/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Policy not found.";
                return RedirectToAction("Dashboard");
            }

            var json = await response.Content.ReadAsStringAsync();
            var policy = JsonSerializer.Deserialize<PolicyViewModel>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(policy);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Session Expired. Please Login Again.";
                return RedirectToAction("Login", "Auth");
            }

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.GetAsync(_apiBaseUrl + "Customer/profile");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var profile = JsonSerializer.Deserialize<CustomerViewModel>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return View(profile);
            }

            TempData["Error"] = "Unable to load profile.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Logged out successfully.";
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            ViewBag.ZipcodeApiKey = _configuration["ZipcodeBase:ApiKey"];
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.GetAsync(_apiBaseUrl + $"Customer/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var profile = JsonSerializer.Deserialize<EditViewModel>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return View(profile);
            }

            TempData["Error"] = "Unable to load profile.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            ViewBag.ZipcodeApiKey = _configuration["ZipcodeBase:ApiKey"];
            var token = HttpContext.Session.GetString("JWToken");

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.PutAsJsonAsync(_apiBaseUrl + $"Customer/update/{id}", model);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Profile");

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Auth");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var userId = HttpContext.Session.GetString("UserId");

            var dto = new { model.CurrentPassword, model.NewPassword };

            var request = new HttpRequestMessage(HttpMethod.Put, _apiBaseUrl + $"Customer/change-password/{userId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(dto);
            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Password changed successfully!";
                return RedirectToAction("Profile");
            }

            var error = await response.Content.ReadAsStringAsync();
            TempData["Error"] = error.Contains("incorrect")
                ? "Current password is incorrect."
                : "Failed to change password.";
            return View(model);
        }

        [HttpGet]
        public IActionResult InsuranceForm(int schemeId, string type, string planName = "")
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Auth");
            ViewBag.ZipcodeApiKey = _configuration["ZipcodeBase:ApiKey"];
            ViewBag.SchemeId = schemeId;   
            ViewBag.InsuranceType = type ?? "Health";
            ViewBag.PlanName = planName;

            return View(new PolicyFormModel
            {
                SchemeId = schemeId,       
                InsuranceType = type,
                PlanName = planName
            });
        }

        [HttpPost]
        public async Task<IActionResult> InsuranceForm([FromBody] PolicyFormModel model)
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return Json(new { success = false, message = "Session expired. Please login again." });

            if (!model.DocumentsVerified)
                return Json(new { success = false, message = "Documents not verified. Please complete Step 3." });

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();
                return Json(new { success = false, message = errors ?? "Please fill all required fields." });
            }

            var premium = Math.Round((model.SumAssured ?? 0) * 0.05m, 2);

            var modelJson = System.Text.Json.JsonSerializer.Serialize(model);
            HttpContext.Session.SetString("PendingPolicy", modelJson);

            return Json(new
            {
                success = true,
                message = "Redirecting to payment...",
                policyId = 0,        
                premium = premium
            });
        }

        public async Task<IActionResult> CPlans()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var request = new HttpRequestMessage(HttpMethod.Get, _apiBaseUrl + "Scheme/allinsurancetype");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var types = JsonSerializer.Deserialize<List<InsuranceTypeViewModel>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(types);
            }

            TempData["Error"] = "Failed to load insurance types.";
            return View(new List<InsuranceTypeViewModel>());
        }
        public IActionResult HealthInsurance()
        {
            return View();
        }
        public IActionResult LifeInsurance()
        {
            return View();
        }
        public IActionResult TermInsurance()
        {
            return View();
        }
        public IActionResult GroupInsurance()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Payment(int policyId, decimal amount, string type, string plan)
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Auth");

            ViewBag.PolicyId = policyId;
            ViewBag.Amount = amount;
            ViewBag.Type = type;
            ViewBag.Plan = plan;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPayment([FromBody] PaymentConfirmRequest request)
        {
            var token = HttpContext.Session.GetString("JWToken");
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var pendingJson = HttpContext.Session.GetString("PendingPolicy");
            if (string.IsNullOrEmpty(pendingJson))
                return Json(new { success = false, error = "Session expired. Please refill the form." });

            var model = System.Text.Json.JsonSerializer.Deserialize<PolicyFormModel>(pendingJson,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var applyJson = System.Text.Json.JsonSerializer.Serialize(model);
            var applyContent = new StringContent(applyJson, System.Text.Encoding.UTF8, "application/json");
            var applyResponse = await _http.PostAsync(_apiBaseUrl + "Policy/apply", applyContent);

            if (!applyResponse.IsSuccessStatusCode)
            {
                var err = await applyResponse.Content.ReadAsStringAsync();
                return Json(new { success = false, error = "Policy creation failed: " + err });
            }

            var applyBody = await applyResponse.Content.ReadAsStringAsync();
            var applyResult = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(applyBody,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var actualPolicyId = applyResult.TryGetProperty("policyId", out var pid) ? pid.GetInt32() : 0;

            var payRequest = new PaymentConfirmRequest
            {
                PolicyId = actualPolicyId,
                Amount = request.Amount,
                PaymentMode = request.PaymentMode,
                TransactionId = request.TransactionId
            };

            var payJson = System.Text.Json.JsonSerializer.Serialize(payRequest);
            var payContent = new StringContent(payJson, System.Text.Encoding.UTF8, "application/json");
            var payResponse = await _http.PostAsync(_apiBaseUrl + "Customer/ConfirmPayment", payContent);

            if (payResponse.IsSuccessStatusCode)
            {
                // ✅ Clear pending policy from session
                HttpContext.Session.Remove("PendingPolicy");

                return Json(new
                {
                    success = true,
                    transactionId = payRequest.TransactionId
                });
            }

            return Json(new { success = false, error = "Payment failed." });
        }
        public async Task<IActionResult> Schemes(int typeId)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var request = new HttpRequestMessage(HttpMethod.Get,
                _apiBaseUrl + $"Scheme/bytype/{typeId}");
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            

            ViewBag.TypeId = typeId;
            ViewBag.TypeName = typeId switch
            {
                1 => "Health",
                2 => "Life",
                3 => "Term",
                4 => "Group",
                _ => "Insurance"
            };
            if (!response.IsSuccessStatusCode)
                return View(new List<InsuranceSchemeViewModel>());
            var schemes = JsonSerializer.Deserialize<List<InsuranceSchemeViewModel>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(schemes ?? new List<InsuranceSchemeViewModel>());
        }

    }

}