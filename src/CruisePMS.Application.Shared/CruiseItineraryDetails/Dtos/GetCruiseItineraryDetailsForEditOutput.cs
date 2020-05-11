using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseItineraryDetails.Dtos
{
    public class GetCruiseItineraryDetailsForEditOutput
    {
        public CreateOrEditCruiseItineraryDetailsDto CruiseItineraryDetails { get; set; }

        public string CruiseItinerariesItineraryName { get; set; }


    }
}
