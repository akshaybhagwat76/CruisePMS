using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseTenantTypes.Dtos
{
    public class GetAllTenantTypesInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CodeFilter { get; set; }

        public string DisplayNameFilter { get; set; }
        public string TenantTypeName { get; set; }

    }
}
