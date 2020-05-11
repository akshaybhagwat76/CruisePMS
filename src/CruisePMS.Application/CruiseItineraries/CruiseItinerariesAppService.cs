using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using CruisePMS.Authorization;
using CruisePMS.Configuration;
using CruisePMS.CruiseItineraries.Dtos;
using CruisePMS.CruiseMasterAmenities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;

namespace CruisePMS.CruiseItineraries
{
    [AbpAuthorize(AppPermissions.Pages_CruiseItineraries)]
    public class CruiseItinerariesAppService : CruisePMSAppServiceBase
    {
        private readonly IRepository<CruiseItinerary> _cruiseItinerariesRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<MasterAmenities, int> _lookup_cruiseMasterAmenitiesRepository;
        public CruiseItinerariesAppService(IRepository<MasterAmenities, int> lookup_cruiseMasterAmenitiesRepository, IRepository<CruiseItinerary> cruiseItinerariesRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _cruiseItinerariesRepository = cruiseItinerariesRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _lookup_cruiseMasterAmenitiesRepository = lookup_cruiseMasterAmenitiesRepository;
        }

        public async Task<PagedResultDto<GetCruiseItinerariesForViewDto>> GetAll(GetAllCruiseItinerariesInput input)
        {

            var filteredCruiseItineraries = _cruiseItinerariesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ItineraryName.Contains(input.Filter) || e.ItineraryCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ItineraryNameFilter), e => e.ItineraryName.ToLower() == input.ItineraryNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ItineraryCodeFilter), e => e.ItineraryCode.ToLower() == input.ItineraryCodeFilter.ToLower().Trim());

            var pagedAndFilteredCruiseItineraries = filteredCruiseItineraries
                .OrderBy(input.Sorting ?? "ItineraryName asc")
                .PageBy(input);


            var cruiseItineraries = from o in pagedAndFilteredCruiseItineraries

                                    join o6 in _lookup_cruiseMasterAmenitiesRepository.GetAll() on o.OnBoardService equals o6.Id into j6
                                    from s1 in j6.DefaultIfEmpty()

                                    select new GetCruiseItinerariesForViewDto()
                                    {
                                        CruiseItineraries = new CruiseItinerariesDto
                                        {
                                            ItineraryName = o.ItineraryName,
                                            ItineraryCode = o.ItineraryCode,
                                            Id = o.Id,
                                        },
                                        OnBoardServiceName = s1 == null ? "" : s1.DisplayName.ToString(),
                                    };

            var totalCount = await filteredCruiseItineraries.CountAsync();

            return new PagedResultDto<GetCruiseItinerariesForViewDto>(
                totalCount,
                await cruiseItineraries.ToListAsync()
            );
        }



        public async Task<GetAllOnBoardService> GetAllOnBoardServices()
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                GetAllOnBoardService getAllOnBoardService = new GetAllOnBoardService();

                var allMasterAmenitiesRepository = _lookup_cruiseMasterAmenitiesRepository.GetAll().Where(x => x.ParentId == 279);
                foreach (var masterAmenities in allMasterAmenitiesRepository)
                {
                    OnBoardService onBoardService = new OnBoardService();
                    onBoardService.Id = masterAmenities.Id;
                    onBoardService.DisplayName = masterAmenities.DisplayName;
                    getAllOnBoardService.OnBoardServices.Add(onBoardService);
                }


                return getAllOnBoardService;
            }
        }

        public async Task<GetCruiseItinerariesForViewDto> GetCruiseItinerariesForView(int id)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            var cruiseItineraries = await _cruiseItinerariesRepository.GetAsync(id);

            var output = new GetCruiseItinerariesForViewDto { CruiseItineraries = ObjectMapper.Map<CruiseItinerariesDto>(cruiseItineraries) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseItineraries_Edit)]
        public async Task<GetCruiseItinerariesForEditOutput> GetCruiseItinerariesForEdit(EntityDto input)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            var cruiseItineraries = await _cruiseItinerariesRepository.FirstOrDefaultAsync(input.Id);
            var output = new GetCruiseItinerariesForEditOutput { CruiseItineraries = ObjectMapper.Map<CreateOrEditCruiseItinerariesDto>(cruiseItineraries) };
            if (cruiseItineraries.ItineraryMap != null && cruiseItineraries.ItineraryMap.Length > 0)
            {
                output.CruiseItineraries.ItineraryMap = "data:image/png;base64," + Convert.ToBase64String(cruiseItineraries.ItineraryMap);
            }
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCruiseItinerariesDto input)
        {
            if (input.Id == 0)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseItineraries_Create)]
        private async Task Create(CreateOrEditCruiseItinerariesDto input)
        {
            string defaultCurrentLanguage = await SettingManager.GetSettingValueForUserAsync(AppSettings.DefaultCurrentLanguage, AbpSession.ToUserIdentifier());
            if (string.IsNullOrWhiteSpace(defaultCurrentLanguage))
            { defaultCurrentLanguage = "EN"; }
            var imageParts = input.ItineraryMap.Split(',').ToList<string>();

            byte[] mapInBytes = Convert.FromBase64String(imageParts[1]);

            CruiseItinerary cruiseItineraries = new CruiseItinerary
            {
                ItineraryMap = mapInBytes,
                ItineraryCode = input.ItineraryCode,
                ItineraryName = input.ItineraryName,
                Description = input.Description,
                OnBoardService = input.OnBoardService
            };

            if (AbpSession.TenantId != null)
            {
                cruiseItineraries.TenantId = (int?)AbpSession.TenantId;
            }
            cruiseItineraries.Lang = defaultCurrentLanguage.ToUpper();

            await _cruiseItinerariesRepository.InsertAsync(cruiseItineraries);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseItineraries_Edit)]
        private async Task Update(CreateOrEditCruiseItinerariesDto input)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            string defaultCurrentLanguage = await SettingManager.GetSettingValueForUserAsync(AppSettings.DefaultCurrentLanguage, AbpSession.ToUserIdentifier());
            if (string.IsNullOrWhiteSpace(defaultCurrentLanguage))
            { defaultCurrentLanguage = "EN"; }
            var cruiseItineraries = await _cruiseItinerariesRepository.FirstOrDefaultAsync((int)input.Id);

            var imageParts = input.ItineraryMap.Split(',').ToList<string>();
            byte[] mapInBytes = Convert.FromBase64String(imageParts[1]);
            cruiseItineraries.ItineraryMap = mapInBytes;
            cruiseItineraries.ItineraryCode = input.ItineraryCode;
            cruiseItineraries.ItineraryName = input.ItineraryName;
            cruiseItineraries.Description = input.Description;
            cruiseItineraries.Lang = defaultCurrentLanguage.ToUpper();
            cruiseItineraries.OnBoardService = input.OnBoardService;
            await _cruiseItinerariesRepository.UpdateAsync(cruiseItineraries);
            // ObjectMapper.Map(input, cruiseItineraries);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseItineraries_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _cruiseItinerariesRepository.DeleteAsync(input.Id);
        }

    }
}
