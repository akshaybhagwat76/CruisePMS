using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.AgePolicies.Dtos;
using CruisePMS.CruiseMasterAmenities.Dtos;
using System.Threading.Tasks;
namespace CruisePMS.AgePolicies
{
    public interface IAgePoliciesAppService : IApplicationService
    {
        Task<PagedResultDto<GetAgePoliciesForViewDto>> GetAll(GetAllAgePoliciesInput input);
        Task<GetAgePoliciesForViewDto> GetAgePoliciesForView(long id);
        Task<GetAgePoliciesForEditOutput> GetAgePoliciesForEdit(EntityDto<long> input);
        Task CreateOrEdit(CreateOrEditAgePoliciesDto input);
        Task Delete(EntityDto<long> input);
        Task<PagedResultDto<AgePoliciesCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(GetAllForLookupTableInput input);
    }
}
