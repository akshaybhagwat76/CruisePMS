using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.CruiseTenantTypes.Dtos;
using System.Threading.Tasks;

namespace CruisePMS.CruiseTenantTypes
{
    public interface ITenantTypesAppService: IApplicationService
    {

        Task<PagedResultDto<GetTenantTypesForViewDto>> GetAll(GetAllTenantTypesInput input);

        Task<GetTenantTypesForViewDto> GetTenantTypesForView(int id);

        Task<GetTenantTypesForEditOutput> GetTenantTypesForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTenantTypesDto input);

        Task Delete(EntityDto input);

    }
}
