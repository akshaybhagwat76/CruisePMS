using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.Common.Dto;
using CruisePMS.CruiseItineraryDetails.Dtos;
using CruisePMS.CruisePhotos.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CruisePMS.CruisePhotos
{
    public interface ICruisePhotosAppService : IApplicationService
    {
        Task<PagedResultDto<GetCruisePhotosForViewDto>> GetAll(GetAllCruisePhotosInput input);

        Task<GetCruisePhotosForViewDto> GetCruisePhotosForView(long id);

        Task<GetCruisePhotosForEditOutput> GetCruisePhotosForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCruisePhotosDto input);

        Task Delete(EntityDto<long> input);

        Task<PagedResultDto<CruisePhotosCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(GetAllForLookupTableInput input);

        Task SaveCruisePhotos(List<SaveCruisePhotos> collectionList);
    }
}
