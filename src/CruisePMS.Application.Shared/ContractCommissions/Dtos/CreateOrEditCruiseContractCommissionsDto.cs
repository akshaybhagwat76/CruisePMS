using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CruisePMS.ContractCommissions.Dtos
{
    public class CreateOrEditCruiseContractCommissionsDto : EntityDto<int?>
    {
		public decimal Commission { get; set; }


		public string CreditCurency { get; set; }


		public int CommissionType { get; set; }


		public int? CruisesId { get; set; }

		public int? CruiseShipsId { get; set; }

		public int? CruiseContractId { get; set; }

		public int? CruiseServicesId { get; set; }
	}
}
