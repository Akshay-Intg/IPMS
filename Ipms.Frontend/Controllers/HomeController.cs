using BLL.DTOs;
using Ipms.Frontend.DTOs.Deserialize;
using Ipms.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ipms.Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Welcome()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return View();
            }
            return RedirectToAction("Dashboard", "Customer");
        }
       
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Plans()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
