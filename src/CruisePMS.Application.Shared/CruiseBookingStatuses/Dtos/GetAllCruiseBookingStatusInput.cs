using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseBookingStatuses.Dtos
{
    public class GetAllCruiseBookingStatusInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string StatusNameFilter { get; set; }



    }
}
