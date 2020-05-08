using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using CruisePMS.Authorization;
using CruisePMS.BookingStatuses.Dtos;
using CruisePMS.Common.Dto;
using CruisePMS.CruiseMasterAmenities;
using CruisePMS.CruiseMasterAmenities.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace CruisePMS.CruiseBookingStatuses
{
    [AbpAuthorize(AppPermissions.Pages_CruiseBookingStatus)]
    public class CruiseBookingStatusAppService : CruisePMSAppServiceBase, ICruiseBookingStatusAppService
    {
        private readonly IRepository<CruiseBookingStatus> _cruiseBookingStatusRepository;
        private readonly IRepository<MasterAmenities, int> _lookup_cruiseMasterAmenitiesRepository;


        public CruiseBookingStatusAppService(IRepository<CruiseBookingStatus> cruiseBookingStatusRepository, IRepository<MasterAmenities, int> lookup_cruiseMasterAmenitiesRepository)
        {
            _cruiseBookingStatusRepository = cruiseBookingStatusRepository;
            _lookup_cruiseMasterAmenitiesRepository = lookup_cruiseMasterAmenitiesRepository;
        }

        public async Task<PagedResultDto<GetCruiseBookingStatusForViewDto>> GetAll(GetAllCruiseBookingStatusInput input)
        {
            var filteredCruiseBookingStatus = _cruiseBookingStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.StatusName.Contains(input.Filter) || e.StatusColor.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusNameFilter), e => e.StatusName.ToLower() == input.StatusNameFilter.ToLower().Trim());

            var pagedAndFilteredCruiseBookingStatus = filteredCruiseBookingStatus
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var cruiseBookingStatus = from o in pagedAndFilteredCruiseBookingStatus
                                      join j in _lookup_cruiseMasterAmenitiesRepository.GetAll() on o.StatusName equals j.Id.ToString()
                                      into j5
                                      from j in j5
                                      select new GetCruiseBookingStatusForViewDto()
                                      {
                                          CruiseBookingStatus = new CruiseBookingStatusDto
                                          {
                                              StatusName = j.DisplayName,
                                              StatusColor = o.StatusColor,
                                              StatusShort = o.StatusShort,
                                              Id = o.Id
                                          }
                                      };

            cruiseBookingStatus = cruiseBookingStatus.OrderBy(x => Convert.ToInt32(x.CruiseBookingStatus.StatusShort));
            var totalCount = await filteredCruiseBookingStatus.CountAsync();

            return new PagedResultDto<GetCruiseBookingStatusForViewDto>(
                totalCount,
                await cruiseBookingStatus.ToListAsync()
            );
        }

        public async Task<GetCruiseBookingStatusForViewDto> GetCruiseBookingStatusForView(int id)
        {
            var cruiseBookingStatus = await _cruiseBookingStatusRepository.GetAsync(id);

            var output = new GetCruiseBookingStatusForViewDto { CruiseBookingStatus = ObjectMapper.Map<CruiseBookingStatusDto>(cruiseBookingStatus) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseBookingStatus_Edit)]
        public async Task<GetCruiseBookingStatusForEditOutput> GetCruiseBookingStatusForEdit(EntityDto input)
        {
            var cruiseBookingStatus = await _cruiseBookingStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCruiseBookingStatusForEditOutput { CruiseBookingStatus = new CreateOrEditCruiseBookingStatusDto() };
            output.CruiseBookingStatus.Id = cruiseBookingStatus.Id;
            output.CruiseBookingStatus.StatusColor = cruiseBookingStatus.StatusColor;
            output.CruiseBookingStatus.StatusName = cruiseBookingStatus.StatusName;
            output.CruiseBookingStatus.StatusShort = cruiseBookingStatus.StatusShort;
            output.CruiseBookingStatus._StatusName = _lookup_cruiseMasterAmenitiesRepository.GetAll().Where(o => o.Id.ToString() == cruiseBookingStatus.StatusName).FirstOrDefault().DisplayName; ;

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCruiseBookingStatusDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CruiseBookingStatus_Create)]
        private async Task Create(CreateOrEditCruiseBookingStatusDto input)
        {
            var cruiseBookingStatus = ObjectMapper.Map<CruiseBookingStatus>(input);


            if (AbpSession.TenantId != null)
            {
                cruiseBookingStatus.TenantId = (int?)AbpSession.TenantId;
            }


            await _cruiseBookingStatusRepository.InsertAsync(cruiseBookingStatus);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseBookingStatus_Edit)]
        private async Task Update(CreateOrEditCruiseBookingStatusDto input)
        {
            var cruiseBookingStatus = await _cruiseBookingStatusRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, cruiseBookingStatus);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseBookingStatus_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _cruiseBookingStatusRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_CruisePhotos)]
        public async Task<PagedResultDto<CruisePhotosCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(Common.Dto.GetAllForLookupTableInput input)
        {
            var query = _lookup_cruiseMasterAmenitiesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.ToString().Contains(input.Filter)
               ).Where(x => x.ParentId == 78);

            var totalCount = await query.CountAsync();

            var cruiseMasterAmenitiesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CruisePhotosCruiseMasterAmenitiesLookupTableDto>();
            foreach (var cruiseMasterAmenities in cruiseMasterAmenitiesList)
            {
                lookupTableDtoList.Add(new CruisePhotosCruiseMasterAmenitiesLookupTableDto
                {
                    Id = cruiseMasterAmenities.Id,
                    DisplayName = cruiseMasterAmenities.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<CruisePhotosCruiseMasterAmenitiesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}
