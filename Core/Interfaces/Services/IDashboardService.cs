using Core.Dtos;

namespace Core.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<RadialBarChartDto> GetTotalFeedbackRadialChartData();
        Task<RadialBarChartDto> GetTotalCoursesChartData();
        Task<RadialBarChartDto> GetRegisteredUserChartData();
        Task<(int paidOrders, int failedOrders)> GetOrderStatusPieChartData();
        Task<IEnumerable<(DateTime date, int orderCount)>> GetOrderGrowthData(DateTime? startDate = null, DateTime? endDate = null);
    }
}
