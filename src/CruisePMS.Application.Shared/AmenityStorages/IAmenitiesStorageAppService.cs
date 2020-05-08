using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.AmenityStorages.Dtos;
using CruisePMS.Common.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CruisePMS.AmenityStorages
{
    public interface IAmenitiesStorageAppService : IApplicationService
    {
        Task<PagedResultDto<GetCruiseAmenitiesStorageForViewDto>> GetAll(GetAllCruiseAmenitiesStorageInput input);

        Task<GetCruiseAmenitiesStorageForViewDto> GetCruiseAmenitiesStorageForView(long id);

        Task<GetCruiseAmenitiesStorageForEditOutput> GetCruiseAmenitiesStorageForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCruiseAmenitiesStorageDto input);

        Task Delete(EntityDto<long> input);


        Task<PagedResultDto<CruiseAmenitiesStorageCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<CruiseAmenitiesStorageCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForListBox(int parentid);

        Task SaveCruiseAmenitiesStorage(List<SaveCruiseAmenitiesStorageDto> input);

        Task<PagedResultDto<MasterAmenitiesForListBoxLookupTableDto>> GetAllSavedRecordsFromDatabase(int sectionId, int sourceId);
    }
}
