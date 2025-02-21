using MedicaWeb_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MedicaWeb_MVC.Controllers
{
    [AllowAnonymous]
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
    }
}
