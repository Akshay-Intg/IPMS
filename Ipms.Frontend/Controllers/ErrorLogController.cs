using Ipms.Frontend.DTOs.Deserialize;
using Ipms.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Ipms.Frontend.Controllers
{
    public class ErrorLogController : Controller
    {
        private readonly HttpClient _http;
        private readonly string? _apiBaseUrl;
        private readonly IConfiguration configuration;
        public ErrorLogController(IHttpClientFactory httpFactory, IConfiguration configuration)
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
        public async Task<IActionResult> ErrorLogs(string stringOrder, string searchString, int pageNumber=1)
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Auth");

            AttachToken();

            var response = await _http.GetAsync(_apiBaseUrl + "ErrorLog/all");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to load error logs.";
                return View(new AuditPaginatedList<ErrorLogDeserialize>(
            new List<ErrorLogDeserialize>(), 0, 1, 10));
            }
            try
            {
                var json = await response.Content.ReadAsStringAsync();

                var logs = JsonSerializer.Deserialize<List<ErrorLogDeserialize>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new List<ErrorLogDeserialize>(); 

                ViewData["IdSortParam"] = stringOrder == "id_asc" ? "id_asc" : "id_desc";

                logs = stringOrder == "id_asc"
                    ? logs.OrderBy(e => e.LoggedDate).ToList()
                    : logs.OrderByDescending(e => e.LoggedDate).ToList();
                if (!string.IsNullOrEmpty(searchString))
                {
                    logs = logs.Where(n =>
                        (n.ErrorMessage?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (n.StackTrace?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (n.RequestPath?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (n.UserName?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? false)
                    ).ToList();
                }

                if (pageNumber < 1)
                    pageNumber = 1;

                int pageSize = 10;

                return View(await AuditPaginatedList<ErrorLogDeserialize>
                    .CreateAsync(logs, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return View(new AuditPaginatedList<ErrorLogDeserialize>(
                    new List<ErrorLogDeserialize>(), 0, 1, 10));
            }
        }
    }
}
