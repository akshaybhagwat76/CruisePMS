using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.Cruises.Dtos
{
    public class GetAllCruisesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CruiseShipsCruiseShipNameFilter { get; set; }

        public string CruiseThemesCruiseThemeNameFilter { get; set; }

        public string CruiseServicesServiceNameFilter { get; set; }

        public string CruiseItinerariesItineraryNameFilter { get; set; }

        public bool OneWay { get; set; }

        public int cruisePortFilter { get; set; }

        public int cruiseDurationFilter { get; set; }
        public bool virtualFilter { get; set; }
        public bool aPIFilter { get; set; }
        public bool B2BFilter { get; set; }
        public bool cruiseIsEnabledFilter { get; set; }
        public bool transferIncludedFilter { get; set; }


    }
}
