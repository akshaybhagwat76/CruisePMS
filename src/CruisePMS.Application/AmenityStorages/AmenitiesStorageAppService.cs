using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using CruisePMS.AmenityStorages.Dtos;
using CruisePMS.Authorization;
using CruisePMS.Common.Dto;
using CruisePMS.CruiseMasterAmenities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace CruisePMS.AmenityStorages
{
    [AbpAuthorize(AppPermissions.Pages_CruiseAmenitiesStorage)]
    public class CruiseAmenitiesStorageAppService : CruisePMSAppServiceBase, IAmenitiesStorageAppService
    {
        private readonly IRepository<AmenityStorage, long> _cruiseAmenitiesStorageRepository;
        private readonly IRepository<MasterAmenities, int> _lookup_cruiseMasterAmenitiesRepository;


        public CruiseAmenitiesStorageAppService(IRepository<AmenityStorage, long> cruiseAmenitiesStorageRepository, IRepository<MasterAmenities, int> lookup_cruiseMasterAmenitiesRepository)
        {
            _cruiseAmenitiesStorageRepository = cruiseAmenitiesStorageRepository;
            _lookup_cruiseMasterAmenitiesRepository = lookup_cruiseMasterAmenitiesRepository;

        }

        public async Task<PagedResultDto<GetCruiseAmenitiesStorageForViewDto>> GetAll(GetAllCruiseAmenitiesStorageInput input)
        {

            var filteredCruiseAmenitiesStorage = _cruiseAmenitiesStorageRepository.GetAll()
                        .Include(e => e.MasterAmenitiesFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruiseMasterAmenitiesDisplayNameFilter), e => e.MasterAmenitiesFk != null && e.MasterAmenitiesFk.DisplayName.ToLower() == input.CruiseMasterAmenitiesDisplayNameFilter.ToLower().Trim())
                        .Where(x => x.SourceId == input.SourceId && x.SectionId == input.SectionId);

            var pagedAndFilteredCruiseAmenitiesStorage = filteredCruiseAmenitiesStorage
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var cruiseAmenitiesStorage = from o in pagedAndFilteredCruiseAmenitiesStorage
                                         join o1 in _lookup_cruiseMasterAmenitiesRepository.GetAll() on o.MasterAmenitiesId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         select new GetCruiseAmenitiesStorageForViewDto()
                                         {
                                             CruiseAmenitiesStorage = new CruiseAmenitiesStorageDto
                                             {
                                                 SourceId = o.SourceId,
                                                 Id = o.Id
                                             },
                                             CruiseMasterAmenitiesDisplayName = s1 == null ? "" : s1.DisplayName.ToString()
                                         };

            var totalCount = await filteredCruiseAmenitiesStorage.CountAsync();

            return new PagedResultDto<GetCruiseAmenitiesStorageForViewDto>(
                totalCount,
                await cruiseAmenitiesStorage.ToListAsync()
            );
        }

        public async Task<GetCruiseAmenitiesStorageForViewDto> GetCruiseAmenitiesStorageForView(long id)
        {
            var cruiseAmenitiesStorage = await _cruiseAmenitiesStorageRepository.GetAsync(id);

            var output = new GetCruiseAmenitiesStorageForViewDto { CruiseAmenitiesStorage = ObjectMapper.Map<CruiseAmenitiesStorageDto>(cruiseAmenitiesStorage) };

            if (output.CruiseAmenitiesStorage.MasterAmenitiesId != null)
            {
                var _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.CruiseAmenitiesStorage.MasterAmenitiesId);
                output.CruiseMasterAmenitiesDisplayName = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseAmenitiesStorage_Edit)]
        public async Task<GetCruiseAmenitiesStorageForEditOutput> GetCruiseAmenitiesStorageForEdit(EntityDto<long> input)
        {
            var cruiseAmenitiesStorage = await _cruiseAmenitiesStorageRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCruiseAmenitiesStorageForEditOutput { CruiseAmenitiesStorage = ObjectMapper.Map<CreateOrEditCruiseAmenitiesStorageDto>(cruiseAmenitiesStorage) };

            if (output.CruiseAmenitiesStorage.MasterAmenitiesId != null)
            {
                var _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.CruiseAmenitiesStorage.MasterAmenitiesId);
                output.CruiseMasterAmenitiesDisplayName = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCruiseAmenitiesStorageDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseAmenitiesStorage_Create)]
        private async Task Create(CreateOrEditCruiseAmenitiesStorageDto input)
        {
            var cruiseAmenitiesStorage = ObjectMapper.Map<AmenityStorage>(input);


            if (AbpSession.TenantId != null)
            {
                cruiseAmenitiesStorage.TenantId = (int?)AbpSession.TenantId;
            }


            await _cruiseAmenitiesStorageRepository.InsertAsync(cruiseAmenitiesStorage);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseAmenitiesStorage_Edit)]
        private async Task Update(CreateOrEditCruiseAmenitiesStorageDto input)
        {
            var cruiseAmenitiesStorage = await _cruiseAmenitiesStorageRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, cruiseAmenitiesStorage);
            // ObjectMapper.Map<CreateOrEditCruiseAmenitiesStorageDto>(input);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseAmenitiesStorage_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _cruiseAmenitiesStorageRepository.DeleteAsync(input.Id);
        }


        [AbpAuthorize(AppPermissions.Pages_CruiseAmenitiesStorage)]
        public async Task<PagedResultDto<CruiseAmenitiesStorageCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_cruiseMasterAmenitiesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DisplayName.ToString().Contains(input.Filter)
               ).Where(x => x.ParentId == input.Parentid);

            var totalCount = await query.CountAsync();

            var cruiseMasterAmenitiesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CruiseAmenitiesStorageCruiseMasterAmenitiesLookupTableDto>();
            foreach (var cruiseMasterAmenities in cruiseMasterAmenitiesList)
            {
                lookupTableDtoList.Add(new CruiseAmenitiesStorageCruiseMasterAmenitiesLookupTableDto
                {
                    Id = cruiseMasterAmenities.Id,
                    DisplayName = cruiseMasterAmenities.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<CruiseAmenitiesStorageCruiseMasterAmenitiesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }


        [AbpAuthorize(AppPermissions.Pages_CruiseAmenitiesStorage)]
        public async Task<PagedResultDto<CruiseAmenitiesStorageCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForListBox(int parentid)
        {

            var query = _lookup_cruiseMasterAmenitiesRepository.GetAll().Where(x => x.ParentId == parentid)
                .OrderBy("DisplayName asc");

            var totalCount = await query.CountAsync();

            var cruiseMasterAmenitiesList = await query.ToListAsync();

            var lookupTableDtoList = new List<CruiseAmenitiesStorageCruiseMasterAmenitiesLookupTableDto>();
            foreach (var cruiseMasterAmenities in cruiseMasterAmenitiesList)
            {
                lookupTableDtoList.Add(new CruiseAmenitiesStorageCruiseMasterAmenitiesLookupTableDto
                {
                    Id = cruiseMasterAmenities.Id,
                    DisplayName = cruiseMasterAmenities.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<CruiseAmenitiesStorageCruiseMasterAmenitiesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseAmenitiesStorage)]
        public async Task<PagedResultDto<MasterAmenitiesForListBoxLookupTableDto>> GetAllSavedRecordsFromDatabase(int sectionId, int sourceId)
        {
            //var query = _lookup_cruiseMasterAmenitiesRepository.GetAll().WhereIf(
            //       !string.IsNullOrWhiteSpace(input.Filter),
            //      e => e.DisplayName.ToString().Contains(input.Filter)
            //   );

            var filteredCruiseAmenitiesStorage = _cruiseAmenitiesStorageRepository.GetAll().Where(x => x.SectionId == sectionId && x.SourceId == sourceId);

            var cruiseAmenitiesStorage = from o in filteredCruiseAmenitiesStorage

                                         join o1 in _lookup_cruiseMasterAmenitiesRepository.GetAll() on o.MasterAmenitiesId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()
                                         orderby ("s1.DisplayName asc")
                                         select new
                                         {
                                             storage = o,
                                             masterAmenities = s1
                                         };
            var totalCount = await cruiseAmenitiesStorage.CountAsync();

            var cruiseMasterAmenitiesList = await cruiseAmenitiesStorage.ToListAsync();

            var lookupTableDtoList = new List<MasterAmenitiesForListBoxLookupTableDto>();
            foreach (var cruiseMasterAmenities in cruiseMasterAmenitiesList)
            {
                lookupTableDtoList.Add(new MasterAmenitiesForListBoxLookupTableDto
                {
                    Id = cruiseMasterAmenities.masterAmenities.Id,
                    DisplayName = cruiseMasterAmenities.masterAmenities.DisplayName?.ToString(),
                    CruiseAmenitiesStorageId = cruiseMasterAmenities.storage.Id,
                    MasterAmenitiesId = cruiseMasterAmenities.storage.MasterAmenitiesId,
                    SectionId = cruiseMasterAmenities.storage.SectionId.Value,
                    SourceId = cruiseMasterAmenities.storage.SourceId

                });
            }

            return new PagedResultDto<MasterAmenitiesForListBoxLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }



        [AbpAuthorize(AppPermissions.Pages_CruiseAmenitiesStorage_Create)]
        public async Task SaveCruiseAmenitiesStorage(List<SaveCruiseAmenitiesStorageDto> collection)
        {
            var Firstvalue = collection.FirstOrDefault();
            var filteredCruiseAmenitiesStorage = _cruiseAmenitiesStorageRepository.GetAll().Where(x => x.SectionId == Firstvalue.SectionId && x.SourceId == Firstvalue.SourceId).ToList();

            foreach (var item in collection)
            {
                AmenityStorage cruiseAmenitiesStorage = new AmenityStorage();
                if (AbpSession.TenantId != null)
                {
                    cruiseAmenitiesStorage.TenantId = (int?)AbpSession.TenantId;
                }
                cruiseAmenitiesStorage.SourceId = item.SourceId;
                cruiseAmenitiesStorage.SectionId = item.SectionId;
                cruiseAmenitiesStorage.Filter = item.Filter;
                cruiseAmenitiesStorage.MasterAmenitiesId = item.MasterAmenities;
                var found = filteredCruiseAmenitiesStorage.Find(
                    ele => ele.SourceId == item.SourceId
                    &&
                    ele.SectionId == item.SectionId
                    &&
                    ele.MasterAmenitiesId == item.MasterAmenities

                );
                if (found == null)
                {
                    await _cruiseAmenitiesStorageRepository.InsertAsync(cruiseAmenitiesStorage);
                }


            }





        }

    }
}
