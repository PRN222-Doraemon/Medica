using MedicaWeb_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MedicaWeb_MVC.Controllers
{
    public class HomeController : Controller
    {
        // =========================
        // === Fields & Props
        // =========================

        private readonly ILogger<HomeController> _logger;

        // =========================
        // === Constructors
        // =========================

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // =========================
        // === Methods
        // =========================

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
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
