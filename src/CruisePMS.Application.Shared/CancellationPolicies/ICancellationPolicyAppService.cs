using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.CancellationPolicies.Dtos;
using CruisePMS.Common.Dto;
using System.Threading.Tasks;

namespace CruisePMS.CancellationPolicies
{ 
	public interface ICancellationPolicyAppService : IApplicationService
	{
		Task<PagedResultDto<GetCancellationPolicyForViewDto>> GetAll(GetAllCancellationPolicyInput input);

		Task<GetCancellationPolicyForViewDto> GetCancellationPolicyForView(int id);

		Task<GetCancellationPolicyForEditOutput> GetCancellationPolicyForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCancellationPolicyDto input);

		Task Delete(EntityDto input);

		Task<PagedResultDto<CancellationPolicyCruiseServicesLookupTableDto>> GetAllCruiseServicesForLookupTable(GetAllForLookupTableInput input);

		Task<PagedResultDto<CancellationPolicyCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(GetAllForLookupTableInput input);

	}
}
