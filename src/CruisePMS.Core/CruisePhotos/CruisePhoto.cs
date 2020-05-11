using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CruisePMS.CruiseMasterAmenities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.CruisePhotos
{
    [Table("AppCruisePhoto")]
    [Audited]
    public class CruisePhoto: Entity<long>, IMayHaveTenant, IFullAudited
    {

		public int? TenantId { get; set; }


		[Required]
		public virtual string PhotoSource { get; set; }

		public virtual byte[] Photo1 { get; set; }

		public virtual byte[] Photo2 { get; set; }

		public virtual byte[] Photo3 { get; set; }

		public virtual byte[] Photo4 { get; set; }

		public virtual byte[] Photo5 { get; set; }

		public virtual int PhotoSourceId { get; set; }


		public virtual int PhotoNameId { get; set; }

		[ForeignKey("PhotoNameId")]
		public MasterAmenities PhotoNameFk { get; set; }


		public long? CreatorUserId { get; set; }
		public DateTime CreationTime { get; set; }
		public long? LastModifierUserId { get; set; }
		public DateTime? LastModificationTime { get; set; }
		public long? DeleterUserId { get; set; }
		public DateTime? DeletionTime { get; set; }
		public bool IsDeleted { get; set; }
	}
}
