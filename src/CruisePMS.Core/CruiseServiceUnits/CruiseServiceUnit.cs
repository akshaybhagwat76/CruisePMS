using Abp.Auditing;
using Abp.Domain.Entities;
using CruisePMS.CruiseMasterAmenities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.CruiseServiceUnits
{
    [Table("AppCruiseServiceUnit")]
    [Audited]
    public class CruiseServiceUnit : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public virtual int? ServiceUnit { get; set; }

        [ForeignKey("ServiceUnit")]
        public MasterAmenities ServiceUnFk { get; set; }

    }
}
