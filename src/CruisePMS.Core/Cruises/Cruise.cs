using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CruisePMS.CruiseItineraries;
using CruisePMS.CruiseServices;
using CruisePMS.CruiseShips;
using CruisePMS.CruiseThemes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.Cruises
{
	[Table("AppCruises")]
	[Audited]
	public class Cruise : Entity, IMayHaveTenant, IFullAudited
	{
		public int? TenantId { get; set; }

		public virtual int CruiseDuration { get; set; }

		public virtual int CruiseStartPort { get; set; }

		public virtual int CruiseEndPort { get; set; }

		[Required]
		public virtual string Cruise_Airport { get; set; }

		public virtual bool CruiseIsEnabled { get; set; }

		public virtual bool CruiseIsEnabledB2B { get; set; }

		public virtual bool DisableForApi { get; set; }

		public virtual decimal StandardDeposit { get; set; }

		public virtual int DepositType { get; set; }

		public virtual DateTime CheckIn { get; set; }

		public virtual DateTime Checkout { get; set; }

		public virtual int? CruiseShipsId { get; set; }

		[ForeignKey("CruiseShipsId")]
		public CruiseShip CruiseShipsFk { get; set; }

		public virtual int? CruiseThemesId { get; set; }

		[ForeignKey("CruiseThemesId")]
		public CruiseTheme CruiseThemesFk { get; set; }

		public virtual int? CruiseServicesId { get; set; }

		[ForeignKey("CruiseServicesId")]
		public CruiseService CruiseServicesFk { get; set; }

		public virtual int? CruiseItinerariesId { get; set; }

		[ForeignKey("CruiseItinerariesId")]
		public CruiseItinerary CruiseItinerariesFk { get; set; }

		public long? DeleterUserId { get; set; }
		public DateTime? DeletionTime { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime CreationTime { get; set; }
		long? ICreationAudited.CreatorUserId { get; set; }
		long? IModificationAudited.LastModifierUserId { get; set; }
		DateTime? IHasModificationTime.LastModificationTime { get; set; }

		public virtual bool VirtualCruise { get; set; }

		public virtual int CruiseYear { get; set; }

		public virtual bool FreeInternet { get; set; }

		public virtual bool TransferIncluded { get; set; }
		public virtual int? CruiseOperatorId { get; set; }

		public virtual string BookingEmail { get; set; }
	}
}
