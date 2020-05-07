using Abp.Auditing;
using Abp.Domain.Entities;
using CruisePMS.CruiseMasterAmenities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.CruiseServiceGroups
{
	[Table("AppCruiseServiceGroup")]
	[Audited]
	public class CruiseServiceGroup : Entity, IMayHaveTenant
	{
		public int? TenantId { get; set; }


		public virtual bool IsMainService { get; set; }

		public virtual bool OnlyOneCanBeChoosen { get; set; }

		public virtual DateTime? StartDate { get; set; }

		public virtual DateTime? StopDate { get; set; }


		public virtual int? ServiceGroupName { get; set; }

		[ForeignKey("ServiceGroupName")]
		public MasterAmenities ServiceGroupNaFk { get; set; }

	}
}
