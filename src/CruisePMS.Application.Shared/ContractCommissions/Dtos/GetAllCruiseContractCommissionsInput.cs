using Abp.Application.Services.Dto;
namespace CruisePMS.ContractCommissions.Dtos
{
    public class GetAllCruiseContractCommissionsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxCommissionTypeFilter { get; set; }
        public int? MinCommissionTypeFilter { get; set; }

        public int ContractId { get; set; }

        public string CruisesCruiseNameFilter { get; set; }

        public string CruiseShipsCruiseShipNameFilter { get; set; }

        public string CruiseContractContractNameFilter { get; set; }

        public string CruiseServicesServiceNameFilter { get; set; }
    }
}
