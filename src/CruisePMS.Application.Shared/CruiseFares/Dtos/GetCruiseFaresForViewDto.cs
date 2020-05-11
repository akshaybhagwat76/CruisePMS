using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseFares.Dtos
{
    public class GetCruiseFaresForViewDto
    {
        public CruiseFaresDto CruiseFares { get; set; }

        public string CruisesTenantId { get; set; }

        public string CruiseMasterAmenitiesDisplayName { get; set; }


            
    }
}
