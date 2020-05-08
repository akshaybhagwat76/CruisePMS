using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.BookingStatuses.Dtos
{
    public class GetAllCruiseBookingStatusInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string StatusNameFilter { get; set; }



    }

}
