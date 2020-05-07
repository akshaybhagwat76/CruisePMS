using Abp.Auditing;
using Abp.Domain.Entities;
using CruisePMS.CruiseShipCategories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CruisePMS.CruiseShips
{
	[Table("AppCruiseShip")]
	[Audited]
	public class CruiseShip : Entity, IMayHaveTenant
	{
		public int? TenantId { get; set; }


		[Required]
		public virtual string CruiseShipName { get; set; }

		public virtual int CruiseDecksNumber { get; set; }

		public virtual int? CruiseShipBuided { get; set; }

		public virtual string CruiseShipFlag { get; set; }

		public virtual string CruiseShipHomePort { get; set; }

		public virtual int? CruiseShipRefurbished { get; set; }

		public virtual int? CruiseShipLength { get; set; }

		public virtual int? CruiseShipDraft { get; set; }

		public virtual int? CruiseShipMaxSpeed { get; set; }

		public virtual int? CruiseShipSpeed { get; set; }

		public virtual int? CruiseShipWidth { get; set; }

		public virtual int? CruiseShipCabinsNumber { get; set; }

		public virtual int? CruiseShipCrewNumber { get; set; }

		public virtual int? CruiseShipPassangersNumber { get; set; }

		public virtual string CruiseShipVoltage { get; set; }

		public virtual bool CruiseShipIsEnabled { get; set; }

		public virtual string CruiseShipShortDescription { get; set; }

		public virtual string CruiseShipDescription { get; set; }

		public virtual string Lang { get; set; }
		public virtual int? CruiseShipCategoryId { get; set; }

		[ForeignKey("CruiseShipCategoryId")]
		public CruiseShipCategory CruiseShipCategoryFk { get; set; }

		public virtual int? OwnerTenantId { get; set; }


	}
}
