using Abp.Application.Services.Dto;
using System;
namespace CruisePMS.Contracts.Dtos
{
	public class CruiseContractDto : EntityDto
	{
		public DateTime? ContractDate { get; set; }

		public bool ContractDisable { get; set; }

		public long UserId { get; set; }

		public long TenantsRecipient { get; set; }

		public int? ContractDcoId { get; set; }

		public string Season { get; set; }

		public DateTime contractCreatedate { get; set; }

		public long TenantSupplierId { get; set; }

		public long SuplierUserId { get; set; }

		public DateTime? ConfirmDate { get; set; }

		public bool ContractEnable { get; set; }


		public string TenantSupplier { get; set; }
		public string TenantRecipient { get; set; }
		public string SuplierUserName { get; set; }
		public string RecipientUserName { get; set; }

	}
}
