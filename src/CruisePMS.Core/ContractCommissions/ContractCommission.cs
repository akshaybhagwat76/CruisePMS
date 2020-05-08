using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CruisePMS.Contracts;
using CruisePMS.Cruises;
using CruisePMS.CruiseServices;
using CruisePMS.CruiseShips;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CruisePMS.ContractCommissions
{
	[Table("AppCruiseContractCommission")]
	[Audited]
	public class ContractCommission : Entity, IMayHaveTenant, IFullAudited
	{
		public int? TenantId { get; set; }


		public virtual decimal Commission { get; set; }

		public virtual string CreditCurency { get; set; }

		public virtual int CommissionType { get; set; }


		public virtual int? CruisesId { get; set; }

		[ForeignKey("CruisesId")]
		public Cruise CruisesFk { get; set; }

		public virtual int? CruiseShipsId { get; set; }

		[ForeignKey("CruiseShipsId")]
		public CruiseShip CruiseShipsFk { get; set; }

		public virtual int? CruiseContractId { get; set; }

		[ForeignKey("CruiseContractId")]
		public Contract CruiseContractFk { get; set; }

		public virtual int? CruiseServicesId { get; set; }

		[ForeignKey("CruiseServicesId")]
		public CruiseService CruiseServicesFk { get; set; }

		public long? DeleterUserId { get; set; }
		public DateTime? DeletionTime { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime CreationTime { get; set; }
		long? ICreationAudited.CreatorUserId { get; set; }
		long? IModificationAudited.LastModifierUserId { get; set; }
		DateTime? IHasModificationTime.LastModificationTime { get; set; }
	}
}
