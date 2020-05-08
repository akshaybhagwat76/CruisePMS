using Abp.Auditing;
using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace CruisePMS.Contracts
{
	[Table("AppContract")]
	[Audited]
	public class Contract : Entity, IMayHaveTenant
	{
		public int? TenantId { get; set; }


		public virtual DateTime? ContractDate { get; set; }

		public virtual bool ContractDisable { get; set; }

		public virtual long UserId { get; set; }

		public virtual long TenantsRecipient { get; set; }

		public virtual int? ContractDcoId { get; set; }

		public virtual string Season { get; set; }

		public virtual DateTime contractCreatedate { get; set; }

		public virtual long TenantSupplierId { get; set; }

		public virtual long SuplierUserId { get; set; }

		public virtual DateTime? ConfirmDate { get; set; }

		public virtual bool ContractEnable { get; set; }

	}
}
