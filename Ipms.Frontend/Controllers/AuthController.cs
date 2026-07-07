using Ipms.Frontend.DTOs.Deserialize;
using Ipms.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ipms.Frontend.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _http ;
        private readonly string? _apiBaseUrl;
        private readonly IConfiguration _configuration;
        public AuthController(IHttpClientFactory httpFactory, IConfiguration configuration)
        {
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
            _http = httpFactory.CreateClient();
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.ZipcodeApiKey = _configuration["ZipcodeBase:ApiKey"];
            return View();
        }

        // ── Register POST — now calls initiate-register ──
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Please fill all required fields." });

            try
            {
                var response = await _http.PostAsJsonAsync(_apiBaseUrl + "Auth/initiate-register", model);
                var msg = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonElement>(msg,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (response.IsSuccessStatusCode)
                {
                    // Store email in session for verify step
                    var email = data.GetProperty("email").GetString();
                    HttpContext.Session.SetString("PendingRegEmail", email ?? "");
                    return Ok(new { message = "OTP sent.", email });
                }

                return BadRequest(new
                {
                    message = data.TryGetProperty("message", out var m) ? m.GetString() : "Registration failed."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ── Verify Registration OTP ──
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> VerifyRegistration([FromBody] VerifyRegistrationModel model)
        {
            try
            {
                var response = await _http.PostAsJsonAsync(_apiBaseUrl + "Auth/verify-registration", model);
                var msg = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonElement>(msg,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.Remove("PendingRegEmail");
                    return Ok(new { message = data.GetProperty("message").GetString() });
                }

                return BadRequest(new { message = data.GetProperty("message").GetString() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ── Resend Registration OTP ──
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ResendRegistrationOtp([FromBody] ResendOtpModel model)
        {
            try
            {
                var response = await _http.PostAsJsonAsync(_apiBaseUrl + "Auth/initiate-register",
                    new { email = model.Email, resend = true });
                var msg = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonElement>(msg,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (response.IsSuccessStatusCode)
                    return Ok(new { message = "New OTP sent to " + model.Email });

                return BadRequest(new { message = data.GetProperty("message").GetString() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {            

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input." });
            }                

            try
            {
                var response = await _http.PostAsJsonAsync(_apiBaseUrl + "Auth/login", model);
                var msg = await response.Content.ReadAsStringAsync();
                var responseMessage = JsonSerializer.Deserialize<CustomerDeserializeDTO>(msg,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("UserId", responseMessage!.customerId.ToString());
                    HttpContext.Session.SetString("JWToken", responseMessage.token);
                    HttpContext.Session.SetString("UserRole", responseMessage.Role ?? "Customer");
                    TempData["Success"] = $"Welcome back!";
                    string redirectUrl = responseMessage.Role switch
                    {
                        "Admin" => "/Admin/Dashboard",
                        "Broker" => "/Broker/Dashboard",
                         "Customer" => "/Customer/Dashboard",
                        "Underwriter" => "Customer/Dashboard"
                    };

                    return Ok(new { message = "Login successful.", redirectUrl = "/Customer/Dashboard" });
                }

                return Unauthorized(new { message = responseMessage?.message ?? "Invalid Email or Password." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Server Error: Could not connect to API." });
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var role = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userId))
                return View();

            return RedirectToAction("Dashboard", "Customer");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            ViewBag.ApiBase = _configuration["ApiSettings:BaseUrl"];
            return View();
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            try
            {
                var response = await _http.PostAsJsonAsync(_apiBaseUrl + "Auth/forgot-password", model);
                var msg = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonElement>(msg);

                if (response.IsSuccessStatusCode)
                    return Ok(new { message = data.GetProperty("message").GetString() });

                return BadRequest(new { message = data.GetProperty("message").GetString() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpModel model)
        {
            try
            {
                var response = await _http.PostAsJsonAsync(_apiBaseUrl + "Auth/verify-otp", model);
                var msg = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonElement>(msg);

                if (response.IsSuccessStatusCode)
                    return Ok(new { message = data.GetProperty("message").GetString() });

                return BadRequest(new { message = data.GetProperty("message").GetString() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}