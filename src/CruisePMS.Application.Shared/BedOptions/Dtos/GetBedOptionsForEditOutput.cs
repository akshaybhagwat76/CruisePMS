using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.BedOptions.Dtos
{
    public class GetBedOptionsForEditOutput
    {
        public CreateOrEditBedOptionsDto BedOptions { get; set; }

        public string CruiseMasterAmenitiesDisplayName { get; set; }
    }
}
