using Core.Interfaces.Services;
using Core.Specifications.Feedbacks;
using Infrastructure.Services;
using MedicaWeb_MVC.ViewModels.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicaWeb_MVC.Controllers
{
    public class DashboardController : Controller
    {
        // ==============================
        // === Fields & Props
        // ==============================

        private readonly IDashboardService _dashboardService;

        // ==============================
        // === Constructors
        // ==============================

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // ==============================
        // === Methods
        // ==============================

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalFeedbackRadialChartData()
        {

            return Json(await _dashboardService.GetTotalFeedbackRadialChartData());
        }

        public async Task<IActionResult> GetRegisteredUserRadialChartData()
        {

            return Json(await _dashboardService.GetRegisteredUserChartData());
        }

        public async Task<IActionResult> GetTotalCoursesRadialChartData()
        {

            return Json(await _dashboardService.GetTotalCoursesChartData());
        }
    }
}
