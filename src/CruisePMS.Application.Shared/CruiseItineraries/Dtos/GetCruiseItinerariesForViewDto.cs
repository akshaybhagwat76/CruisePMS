using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseItineraries.Dtos
{
    public class GetCruiseItinerariesForViewDto
    {
        public CruiseItinerariesDto CruiseItineraries { get; set; }

        public string OnBoardServiceName { get; set; }
    }
}
