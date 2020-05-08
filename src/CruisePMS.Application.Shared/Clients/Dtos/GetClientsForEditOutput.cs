using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.Clients.Dtos
{
    public class GetClientsForEditOutput
    {
        public CreateOrEditClientsDto Clients { get; set; }

        public string CruiseMasterAmenitiesDisplayName { get; set; }

        public string CruiseMasterAmenitiesDisplayName2 { get; set; }

        public string CruiseMasterAmenitiesDisplayName3 { get; set; }

    }
}
