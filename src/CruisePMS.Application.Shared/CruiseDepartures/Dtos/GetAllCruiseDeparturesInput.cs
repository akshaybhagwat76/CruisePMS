using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseDepartures.Dtos
{
    public class GetAllCruiseDeparturesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public int DayNumber { get; set; }
        public string CruisesTenantIdFilter { get; set; }

        public int CruiseId { get; set; }
    }
}
