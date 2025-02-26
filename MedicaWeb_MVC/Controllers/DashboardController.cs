using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicaWeb_MVC.Controllers
{
    [Authorize]
    public class DashboardController(ICourseService courseService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
