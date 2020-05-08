using Abp.Auditing;
using Abp.Domain.Entities;
using CruisePMS.CruiseMasterAmenities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.AgePolicies
{

    [Table("AppAgePolicy")]
    [Audited]
    public class AgePolicy : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        public virtual int AgeFrom { get; set; }

        public virtual int AgeTo { get; set; }


        public virtual int GuestType { get; set; }

        [ForeignKey("GuestType")]
        public MasterAmenities GuestTyFk { get; set; }

    }
}
