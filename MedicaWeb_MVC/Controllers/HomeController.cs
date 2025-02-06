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
            // Define Narbar
            //var navbarVM = new NavbarVM()
            //{
            //    MenuItems = new List<NavbarItemVM>
            //    {
            //         new NavbarItemVM { Name = "Courses", Endpoint = Url.Action("Index", "Courses") ?? string.Empty },
            //        new NavbarItemVM { Name = "News", Endpoint = Url.Action("Index", "News") ?? string.Empty},
            //        new NavbarItemVM { Name = "My Learnings", Endpoint = Url.Action("Index", "Mylearning") ?? string.Empty }
            //    }
            //};

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
