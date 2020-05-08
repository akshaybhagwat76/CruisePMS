using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.Contracts.Dtos
{
    public class GetAllCruiseContractInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int TravelAgentsTenantId { get; set; }
        public int CruiseOpratorTenantId { get; set; }

    }
}
