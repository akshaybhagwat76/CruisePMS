using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;

namespace CruisePMS.CruiseItineraryDetails.Dtos
{
    public class CreateOrEditCruiseItineraryDetailsDto : EntityDto<int?>
    {

        public int Day { get; set; }

        public string PortID { get; set; }

        public bool Breakfast { get; set; }

        public bool Lunch { get; set; }

        public bool AfternoonSnack { get; set; }

        public bool Dinner { get; set; }

        public bool CaptainDinner { get; set; }

        public bool LiveMusic { get; set; }

        public string Description { get; set; }

        public string Photo { get; set; }

        public int? CruiseItinerariesId { get; set; }

        public bool OnAnchor { get; set; }
        public string Note { get; set; }
    }
}
