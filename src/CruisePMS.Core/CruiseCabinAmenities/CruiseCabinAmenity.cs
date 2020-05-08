using Abp.Auditing;
using Abp.Domain.Entities;
using CruisePMS.CruiseCabinAmenitiesGroups;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.CruiseCabinAmenities
{
	[Table("AppCruiseCabinAmenity")]
	[Audited]
	public class CruiseCabinAmenity : Entity, IMayHaveTenant
	{
		public int? TenantId { get; set; }


		public virtual string CabinAmenity { get; set; }


		public virtual int? CruiseCabinAmenitiesGroupsId { get; set; }

		[ForeignKey("CruiseCabinAmenitiesGroupsId")]
		public CruiseCabinAmenitiesGroup CruiseCabinAmenitiesGroupsFk { get; set; }

	}
}
