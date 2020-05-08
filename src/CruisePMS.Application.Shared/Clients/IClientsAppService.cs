using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.Clients.Dtos;
using CruisePMS.Common.Dto;
using System.Threading.Tasks;

namespace CruisePMS.Clients
{
    public interface IClientsAppService : IApplicationService
    {
        Task<PagedResultDto<GetClientsForViewDto>> GetAll(GetAllClientsInput input);

        Task<GetClientsForViewDto> GetClientsForView(int id);

        Task<GetClientsForEditOutput> GetClientsForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditClientsDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<ClientsLookupTableDto>> GetAllPassengerForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ClientsCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(GetAllForLookupTableInput input);
    }
}
