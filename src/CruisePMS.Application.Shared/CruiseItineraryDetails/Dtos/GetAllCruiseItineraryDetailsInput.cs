using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseItineraryDetails.Dtos
{
    public class GetAllCruiseItineraryDetailsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int ItineraryId { get; set; }
        public string CruiseItinerariesItineraryNameFilter { get; set; }


    }
}
