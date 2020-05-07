using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.CruiseTenantTypes.Dtos
{
    public class TenantTypesDto: EntityDto
    {
        public virtual string TenantTypeName { get; set; }

    }
}
