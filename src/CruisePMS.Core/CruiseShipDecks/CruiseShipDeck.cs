using Abp.Auditing;
using Abp.Domain.Entities;
using CruisePMS.CruiseMasterAmenities;
using CruisePMS.CruiseShips;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.CruiseShipDecks
{
	[Table("AppCruiseShipDeck")]
	[Audited]
	public class CruiseShipDeck : Entity, IMayHaveTenant
	{
		public int? TenantId { get; set; }


		public virtual int DeckSortOrder { get; set; }


		public virtual int? CruiseShipsId { get; set; }

		[ForeignKey("CruiseShipsId")]
		public CruiseShip CruiseShipsFk { get; set; }

		public virtual int? ShipDeckName { get; set; }

		[ForeignKey("ShipDeckName")]
		public MasterAmenities ShipDeckNaFk { get; set; }

	}
}
