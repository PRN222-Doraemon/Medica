using Core.Dtos;
using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications.Courses;
using Core.Specifications.Feedbacks;
using Core.Specifications.Orders;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        // ==============================
        // === Fields & Props
        // ==============================

        private readonly IAccountService _accountService;
        private readonly ICourseService _courseService;
        private readonly IFeedbackService _feedbackService;
        private readonly IUnitOfWork _unitOfWork;

        static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.AddMonths(-1).Month;
        readonly DateTime previousMonthStartDate = new DateTime(DateTime.Now.Year, previousMonth, 1);
        readonly DateTime currentMonthStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        // ==============================
        // === Constructors
        // ==============================

        public DashboardService(IUnitOfWork unitOfWork, IAccountService accountService, ICourseService courseService, IFeedbackService feedbackService)
        {
            _accountService = accountService;
            _courseService = courseService;
            _feedbackService = feedbackService;
            _unitOfWork = unitOfWork;
        }

        // ==============================
        // === Methods
        // ==============================

        // Return the radial bar chart data comparing from current month and previous month of Registered Users
        public async Task<RadialBarChartDto> GetRegisteredUserChartData()
        {
            var totalRegisteredUser = await _accountService.GetAllRegisteredUserAsync();
            var countTotal = totalRegisteredUser.Count();
            var countByCurrentMonth = totalRegisteredUser.Count(u => currentMonthStartDate <= u.CreatedAt && u.CreatedAt <= DateTime.Now);
            var countByPreviousMonth = totalRegisteredUser.Count(f => previousMonthStartDate <= f.CreatedAt && f.CreatedAt <= currentMonthStartDate);

            return GetRadialBarChartDto(countTotal, countByCurrentMonth, countByPreviousMonth);
        }

        // Return the radial bar chart data comparing from current month and previous month of Courses
        public async Task<RadialBarChartDto> GetTotalCoursesChartData()
        {
            var spec = new CourseSpecification(new CourseParams()
            {
                Status = CourseStatus.Active
            }, false);

            var totalCourses = await _courseService.GetCoursesAsync(spec);
            var countTotal = totalCourses.Count();
            var countByCurrentMonth = totalCourses.Count(u => currentMonthStartDate <= u.CreatedAt && u.CreatedAt <= DateTime.Now);
            var countByPreviousMonth = totalCourses.Count(f => previousMonthStartDate <= f.CreatedAt && f.CreatedAt <= currentMonthStartDate);

            return GetRadialBarChartDto(countTotal, countByCurrentMonth, countByPreviousMonth);
        }

        // Return the radial bar chart data comparing from current month and previous month of Feedback
        public async Task<RadialBarChartDto> GetTotalFeedbackRadialChartData()
        {
            var spec = new FeedbackSpecification(new FeedbackParams()
            {
                Status = FeedbackStatus.Enabled
            });

            var totalFeedback = await _feedbackService.GetAllFeedbacks(spec);

            var countTotal = totalFeedback.Count();
            var countByCurrentMonth = totalFeedback.Count(f => currentMonthStartDate <= f.CreatedAt && f.CreatedAt <= DateTime.Now);
            var countByPreviousMonth = totalFeedback.Count(f => previousMonthStartDate <= f.CreatedAt && f.CreatedAt <= currentMonthStartDate);

            return GetRadialBarChartDto(countTotal, countByCurrentMonth, countByPreviousMonth);
        }

        // Return the radial bar chart data comparing from current month and previous month
        private RadialBarChartDto GetRadialBarChartDto(int totalCount, int currentMonthCount, int previousMonthCount)
        {
            int ratioDifference = 100;
            if (previousMonthCount != 0)
            {
                ratioDifference = Convert.ToInt32((double)(currentMonthCount - previousMonthCount) / previousMonthCount * 100);
            }

            return new RadialBarChartDto()
            {
                Series = new int[] { ratioDifference },
                CountInCurrentMonth = currentMonthCount,
                TotalCount = totalCount,
                HasRatioIncreased = currentMonthCount > previousMonthCount
            };
        }

        // Return the number of paid and failed orders
        public async Task<(int paidOrders, int failedOrders)> GetOrderStatusPieChartData()
        {
            var specPaid = new OrderSpecification(new OrderParams() { Status = OrderStatus.Paid });
            var specFailed = new OrderSpecification(new OrderParams() { Status = OrderStatus.Cancelled });

            var ordersPaid = await _unitOfWork.Repository<Order>().ListAsync(specPaid);
            var ordersFailed = await _unitOfWork.Repository<Order>().ListAsync(specFailed);

            return (ordersPaid.Count(), ordersFailed.Count());
        }

        // Return the order growth data
        public async Task<IEnumerable<(DateTime date, int orderCount)>> GetOrderGrowthData(DateTime? startDate = null, DateTime? endDate = null)
        {
            // Default range is 1 month
            startDate ??= DateTime.Now.AddMonths(-24);
            endDate ??= DateTime.Now;

            var spec = new OrderSpecification(new OrderParams()
            {
                StartDate = startDate,
                EndDate = endDate
            });

            var orders = await _unitOfWork.Repository<Order>().ListAsync(spec);
            var result = orders
                            .GroupBy(o => new { o.OrderTime.Year, o.OrderTime.Month })
                            .OrderBy(o => o.Key.Year)                // e.g. 2024, 2025, ...
                            .ThenBy(o => o.Key.Month)                // e.g. 2024-01, 2024-02, ...
                            .Select(g =>                          // Create a tuple for each group
                            (
                                date: new DateTime(g.Key.Year, g.Key.Month, 1), // e.g. 2024-01-01, 2024-02-01, ...
                                orderCount: g.Count()                           // e.g. 10, 20, ...
                            ));
            return result;
        }
    }
}
