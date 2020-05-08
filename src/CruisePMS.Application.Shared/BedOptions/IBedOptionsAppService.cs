using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.BedOptions.Dtos;
using CruisePMS.CruiseMasterAmenities.Dtos;
using System.Threading.Tasks;
namespace CruisePMS.BedOptions
{
    public interface IBedOptionsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBedOptionsForViewDto>> GetAll(GetAllBedOptionsInput input);

        Task<GetBedOptionsForViewDto> GetBedOptionsForView(int id);

        Task<GetBedOptionsForEditOutput> GetBedOptionsForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditBedOptionsDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<BedOptionsCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(GetAllForLookupTableInput input);

    }
}
