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

        public async Task<IActionResult> GetOrderStatusPieChartData()
        {
            var (paidOrders, failedOrders) = await _dashboardService.GetOrderStatusPieChartData();

            var viewModel = new PieChartVM()
            {
                Labels = ["Paid", "Failed"],
                Series = [paidOrders, failedOrders],
            };

            return Json(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderGrowthLineData(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate
        )
        {
            var orderData = await _dashboardService.GetOrderGrowthData(startDate, endDate);
            var viewModel = new LineChartVM()
            {
                Dates = orderData.Select(x => x.date.ToString("MMM yyyy")).ToList(), // get the date only
                Series = new List<SeriesVM>()
                {
                    new SeriesVM()
                    {
                        Name = "Orders",
                        Data = orderData.Select(x => x.orderCount).ToList(), // Get the count only
                    }
                }
            };

            return Json(viewModel);
        }
    }
}
