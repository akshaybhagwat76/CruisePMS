﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CruisePMS.MultiTenancy.HostDashboard.Dto;

namespace CruisePMS.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}