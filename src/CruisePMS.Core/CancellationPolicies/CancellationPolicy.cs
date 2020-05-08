using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CruisePMS.CruiseMasterAmenities;
using CruisePMS.CruiseServices;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace CruisePMS.CancellationPolicies
{
    [Table("AppCancellationPolicy")]
    [Audited]
    public class CancellationPolicy : Entity, IMayHaveTenant, IFullAudited
    {
		public int? TenantId { get; set; }

		public virtual string CancellationPercentage { get; set; }

		public virtual int DaysFrom { get; set; }

		public virtual int DaysTo { get; set; }

		public virtual int SeasonYear { get; set; }


		public virtual int? CruiseServicesId { get; set; }

		[ForeignKey("CruiseServicesId")]
		public CruiseService CruiseServicesFk { get; set; }

		public virtual int? CancellationPreText { get; set; }

		[ForeignKey("CancellationPreText")]
		public MasterAmenities CancellationPreTeFk { get; set; }

		public virtual int? CancellationPostText { get; set; }

		[ForeignKey("CancellationPostText")]
		public MasterAmenities CancellationPostTeFk { get; set; }

		public long? CreatorUserId { get; set; }
		public DateTime CreationTime { get; set; }
		public long? LastModifierUserId { get; set; }
		public DateTime? LastModificationTime { get; set; }
		public long? DeleterUserId { get; set; }
		public DateTime? DeletionTime { get; set; }
		public bool IsDeleted { get; set; }
	}
}
