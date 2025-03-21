﻿using Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<RadialBarChartDto> GetTotalFeedbackRadialChartData();
        Task<RadialBarChartDto> GetTotalCoursesChartData();
        Task<RadialBarChartDto> GetRegisteredUserChartData();
    }
}
