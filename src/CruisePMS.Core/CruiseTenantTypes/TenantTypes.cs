using Abp.Auditing;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.CruiseTenantTypes
{
    [Table("AppTenantTypes")]
    [Audited]
    public class TenantTypes : AuditedEntity

    {
        [Required]
        public virtual string TenantTypeName { get; set; }
    }
}
