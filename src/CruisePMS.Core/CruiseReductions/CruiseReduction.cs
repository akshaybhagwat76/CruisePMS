using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CruisePMS.AgePolicies;
using CruisePMS.CruiseServices;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace CruisePMS.CruiseReductions
{
    [Table("AppCruiseReduction")]
    [Audited]
    public class CruiseReduction : Entity, IMayHaveTenant, IFullAudited
    {
        public int? TenantId { get; set; }
        public virtual int PassangerNoName { get; set; }
        public virtual int ReductionName { get; set; }
        public virtual decimal ReductionAmount { get; set; }
        public virtual string ReductionType { get; set; }
        public virtual int ActivateOn { get; set; }
        public virtual int ServicesId { get; set; }
        public virtual long AgePoliciesId { get; set; }

        [ForeignKey("ServicesId")]
        public CruiseService ServicesFk { get; set; }
        [ForeignKey("AgePoliciesId")]
        public AgePolicy AgePoliciesFk { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
