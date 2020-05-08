using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseDepartures.Dtos
{
    public class GetCruiseDeparturesForEditOutput
    {
        public CreateOrEditCruiseDeparturesDto CruiseDepartures { get; set; }

        public string CruisesTenantId { get; set; }


    }
}
