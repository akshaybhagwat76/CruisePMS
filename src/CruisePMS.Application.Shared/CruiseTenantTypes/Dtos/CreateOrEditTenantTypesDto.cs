using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseTenantTypes.Dtos
{
    public class CreateOrEditTenantTypesDto : EntityDto<int?>
    {
        public string TenantTypeName { get; set; }

    }
}
