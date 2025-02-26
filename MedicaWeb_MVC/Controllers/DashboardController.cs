using Core.Interfaces.Services;
using Core.Specifications.Feedbacks;
using MedicaWeb_MVC.ViewModels.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicaWeb_MVC.Controllers
{
    [Authorize]
    public class DashboardController(
        ICourseService courseService,
        IFeedbackService feedbackService) : Controller
    {
        // ==============================
        // === Fields & Props
        // ==============================

        static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.AddMonths(-1).Month;
        readonly DateTime previousMonthStartDate = new DateTime(DateTime.Now.Year, previousMonth, 1);
        readonly DateTime currentMonthStartDate = new DateTime(DateTime.Now.Year, previousMonth, 1);

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalFeedbackRadialChartData()
        {
            var feedbackSpec = new FeedbackSpecification(new FeedbackParams() { Status = Core.Entities.FeedbackStatus.Enabled });
            var totalFeedbacks = await feedbackService.GetAllFeedbacks(feedbackSpec);
            var countByCurrentMonth = totalFeedbacks.Count(u =>
                                        u.CreatedAt >= currentMonthStartDate
                                        && u.CreatedAt <= DateTime.Now);
            var countByPreviousMonth = totalFeedbacks.Count(u =>
                                        u.CreatedAt >= previousMonthStartDate
                                        && u.CreatedAt <= currentMonthStartDate);

            int increaseDecreaseRadio = 100;
            if (countByPreviousMonth != 0)
            {
                increaseDecreaseRadio = Convert.ToInt32((countByCurrentMonth - countByPreviousMonth) / countByPreviousMonth * 100);
            }

            RadicalBarChartVM radicalBarChartVM = new()
            {
                TotalCount = totalFeedbacks.Count(),
                HasRatioIncreased = countByCurrentMonth > countByPreviousMonth,
                IncreaseDecreaseAmount = increaseDecreaseRadio,
                CountInCurrentMonth = countByCurrentMonth,
                Series = new int[] { increaseDecreaseRadio }
            };

            return Json(radicalBarChartVM);
        }
    }
}
