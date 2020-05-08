using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.Contracts.Dtos
{
	public class CreateOrEditCruiseContractDto : EntityDto<int?>
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

		public string supplierName { get; set; }
		public string AgentTenantName { get; set; }
		public int AgentTenantId { get; set; }
		public string AgentName { get; set; }
		public long AgentId { get; set; }



	}
}
