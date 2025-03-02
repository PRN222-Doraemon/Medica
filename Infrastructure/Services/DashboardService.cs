using Core.Dtos;
using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications.Courses;
using Core.Specifications.Feedbacks;
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

        static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.AddMonths(-1).Month;
        readonly DateTime previousMonthStartDate = new DateTime(DateTime.Now.Year, previousMonth, 1);
        readonly DateTime currentMonthStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        // ==============================
        // === Constructors
        // ==============================

        public DashboardService(IAccountService accountService, ICourseService courseService, IFeedbackService feedbackService)
        {
            _accountService = accountService;
            _courseService = courseService;
            _feedbackService = feedbackService;
        }

        // ==============================
        // === Methods
        // ==============================

        public async Task<RadialBarChartDto> GetRegisteredUserChartData()
        {
            var totalRegisteredUser = await _accountService.GetAllRegisteredUserAsync();
            var countTotal = totalRegisteredUser.Count();
            var countByCurrentMonth = totalRegisteredUser.Count(u => currentMonthStartDate <= u.CreatedAt && u.CreatedAt <= DateTime.Now);
            var countByPreviousMonth = totalRegisteredUser.Count(f => previousMonthStartDate <= f.CreatedAt && f.CreatedAt <= currentMonthStartDate);

            return GetRadialBarChartDto(countTotal, countByCurrentMonth, countByPreviousMonth);
        }

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
    }
}
