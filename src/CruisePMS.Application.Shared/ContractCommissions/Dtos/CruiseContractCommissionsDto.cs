using Abp.Application.Services.Dto;

namespace CruisePMS.ContractCommissions.Dtos
{
    public class CruiseContractCommissionsDto : EntityDto
    {
        public string CreditCurency { get; set; }

        public int CommissionType { get; set; }

        public decimal Commission { get; set; }

        public int? CruisesId { get; set; }

        public int? CruiseShipsId { get; set; }

        public int? CruiseContractId { get; set; }

        public int? CruiseServicesId { get; set; }


    }
}
