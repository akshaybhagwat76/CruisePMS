using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.CruiseCabinAmenitiesGroups
{
    [Table("AppCruiseCabinAmenitiesGroups")]
    [Audited]
    public class CruiseCabinAmenitiesGroup : Entity, IMayHaveTenant, IFullAudited
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string CabinAmenityGroup { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
