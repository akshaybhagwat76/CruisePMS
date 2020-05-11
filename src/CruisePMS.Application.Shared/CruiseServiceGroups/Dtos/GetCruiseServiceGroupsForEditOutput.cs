using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseServiceGroups.Dtos
{
    public class GetCruiseServiceGroupsForEditOutput
    {
        public CreateOrEditCruiseServiceGroupsDto CruiseServiceGroups { get; set; }

        public string CruiseMasterAmenitiesDisplayName { get; set; }


    }
}
