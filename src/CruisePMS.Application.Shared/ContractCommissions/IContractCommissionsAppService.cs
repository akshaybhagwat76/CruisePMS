using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.Common.Dto;
using CruisePMS.ContractCommissions.Dtos;
using System.Threading.Tasks;

namespace CruisePMS.ContractCommissions
{
	public interface IContractCommissionsAppService : IApplicationService
	{
		Task<PagedResultDto<GetCruiseContractCommissionsForViewDto>> GetAll(GetAllCruiseContractCommissionsInput input);

		Task<GetCruiseContractCommissionsForViewDto> GetCruiseContractCommissionsForView(int id);

		Task<GetCruiseContractCommissionsForEditOutput> GetCruiseContractCommissionsForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCruiseContractCommissionsDto input);

		Task Delete(EntityDto input);


		Task<PagedResultDto<CruiseContractCommissionsCruisesLookupTableDto>> GetAllCruisesForLookupTable(GetAllForLookupTableInput input);

		Task<PagedResultDto<CruiseContractCommissionsCruiseShipsLookupTableDto>> GetAllCruiseShipsForLookupTable(GetAllForLookupTableInput input);

		Task<PagedResultDto<CruiseContractCommissionsCruiseContractLookupTableDto>> GetAllCruiseContractForLookupTable(GetAllForLookupTableInput input);

		Task<PagedResultDto<CruiseContractCommissionsCruiseServicesLookupTableDto>> GetAllCruiseServicesForLookupTable(GetAllForLookupTableInput input);

	}
}
