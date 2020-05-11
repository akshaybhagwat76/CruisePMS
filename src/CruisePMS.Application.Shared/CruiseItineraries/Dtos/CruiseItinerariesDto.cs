using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseItineraries.Dtos
{
    public class CruiseItinerariesDto : EntityDto<long>
    {
        public string ItineraryName { get; set; }

        public string ItineraryCode { get; set; }





    }
}
