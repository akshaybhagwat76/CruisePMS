using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using CruisePMS.Authorization;
using CruisePMS.Common.Dto;
using CruisePMS.Configuration;
using CruisePMS.CruiseItineraries;
using CruisePMS.CruiseItineraryDetails.Dtos;
using CruisePMS.Localization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;

namespace CruisePMS.CruiseItineraryDetails
{
    [AbpAuthorize(AppPermissions.Pages_CruiseItineraryDetails)]
    public class CruiseItineraryDetailsAppService : CruisePMSAppServiceBase, ICruiseItineraryDetailsAppService
    {
        private readonly IRepository<CruiseItineraryDetail> _cruiseItineraryDetailsRepository;
        private readonly IRepository<CruiseItinerary> _lookup_cruiseItinerariesRepository;
        private readonly IRepository<ip2location_city_multilingual> _citiesRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CruiseItineraryDetailsAppService(IUnitOfWorkManager unitOfWorkManager, IRepository<CruiseItineraryDetail> cruiseItineraryDetailsRepository, IRepository<CruiseItinerary> lookup_cruiseItinerariesRepository,
            IRepository<ip2location_city_multilingual> citiesRepository)
        {
            _cruiseItineraryDetailsRepository = cruiseItineraryDetailsRepository;
            _citiesRepository = citiesRepository;
            _lookup_cruiseItinerariesRepository = lookup_cruiseItinerariesRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<PagedResultDto<GetCruiseItineraryDetailsForViewDto>> GetAll(GetAllCruiseItineraryDetailsInput input)
        {
            string defaultLanguage = "en";
            var currentLang = SettingManager.GetSettingValueForUser(AppSettings.DefaultCurrentLanguage, null, AbpSession.UserId.Value);

            if (!string.IsNullOrEmpty(currentLang))
            {
                defaultLanguage = currentLang;
            }

            var filteredcities = _citiesRepository.GetAll().Where(x => x.lang_code.ToLower() == defaultLanguage.ToLower()).ToList();
            List<GetCity> GetCityList = new List<GetCity>();
            foreach (var filteredcity in filteredcities)
            {
                GetCity getCity = new GetCity();
                getCity.Id = filteredcity.Id;
                getCity.lang_city_name = filteredcity.lang_city_name;
                GetCityList.Add(getCity);
            }
            var filteredCruiseItineraryDetails = _cruiseItineraryDetailsRepository.GetAll()
                        .Include(e => e.CruiseItinerariesFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruiseItinerariesItineraryNameFilter), e => e.CruiseItinerariesFk != null && e.CruiseItinerariesFk.ItineraryName.ToLower() == input.CruiseItinerariesItineraryNameFilter.ToLower().Trim())
                        .Where(x => x.CruiseItinerariesId == input.ItineraryId);



            var pagedAndFilteredCruiseItineraryDetails = filteredCruiseItineraryDetails
                .OrderBy(input.Sorting ?? "day asc")
                .PageBy(input);

            var cruiseItineraryDetails = from o in pagedAndFilteredCruiseItineraryDetails
                                         join o1 in _lookup_cruiseItinerariesRepository.GetAll() on o.CruiseItinerariesId equals (int)o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         select new GetCruiseItineraryDetailsForViewDto()
                                         {
                                             CruiseItineraryDetails = new CruiseItineraryDetailsDto
                                             {
                                                 Day = o.Day,
                                                 PortID = updateRecords(o.PortID, GetCityList),
                                                 Breakfast = o.Breakfast,
                                                 Lunch = o.Lunch,
                                                 AfternoonSnack = o.AfternoonSnack,
                                                 Dinner = o.Dinner,
                                                 CaptainDinner = o.CaptainDinner,
                                                 LiveMusic = o.LiveMusic,
                                                 OnAnchor = o.OnAnchor,
                                                 Note = o.Note,
                                                 Id = o.Id
                                             },
                                             CruiseItinerariesItineraryName = s1 == null ? "" : s1.ItineraryName.ToString()
                                         };

            var totalCount = await filteredCruiseItineraryDetails.CountAsync();

            return new PagedResultDto<GetCruiseItineraryDetailsForViewDto>(
                totalCount,
                await cruiseItineraryDetails.ToListAsync()
            );
        }


        public string updateRecords(string Ids, List<GetCity> getCities)
        {
            string portName = string.Empty;
            string[] portIdArray = Ids.Split(',');
            foreach (var portId in portIdArray)
            {
                var foundItem = getCities.FirstOrDefault(x => x.Id == Convert.ToInt32(portId));
                portName = portName + foundItem.lang_city_name + ',';
            }
            return portName.TrimEnd(',');
        }


        public async Task<GetCruiseItineraryDetailsForViewDto> GetCruiseItineraryDetailsForView(int id)
        {
            var cruiseItineraryDetails = await _cruiseItineraryDetailsRepository.GetAsync(id);

            var output = new GetCruiseItineraryDetailsForViewDto { CruiseItineraryDetails = ObjectMapper.Map<CruiseItineraryDetailsDto>(cruiseItineraryDetails) };

            if (output.CruiseItineraryDetails.CruiseItinerariesId != null)
            {
                var _lookupCruiseItineraries = await _lookup_cruiseItinerariesRepository.FirstOrDefaultAsync((int)output.CruiseItineraryDetails.CruiseItinerariesId);
                output.CruiseItinerariesItineraryName = _lookupCruiseItineraries.ItineraryName.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseItineraryDetails_Edit)]
        public async Task<GetCruiseItineraryDetailsForEditOutput> GetCruiseItineraryDetailsForEdit(EntityDto input)
        {
            var cruiseItineraryDetails = await _cruiseItineraryDetailsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCruiseItineraryDetailsForEditOutput { CruiseItineraryDetails = ObjectMapper.Map<CreateOrEditCruiseItineraryDetailsDto>(cruiseItineraryDetails) };

            if (output.CruiseItineraryDetails.CruiseItinerariesId != null)
            {
                var _lookupCruiseItineraries = await _lookup_cruiseItinerariesRepository.FirstOrDefaultAsync((int)output.CruiseItineraryDetails.CruiseItinerariesId);
                output.CruiseItinerariesItineraryName = _lookupCruiseItineraries.ItineraryName.ToString();
            }
            if (cruiseItineraryDetails.Photo != null && cruiseItineraryDetails.Photo.Length > 0)
            {
                output.CruiseItineraryDetails.Photo = "data:image/png;base64," + Convert.ToBase64String(cruiseItineraryDetails.Photo);
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCruiseItineraryDetailsDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CruiseItineraryDetails_Create)]
        private async Task Create(CreateOrEditCruiseItineraryDetailsDto input)
        {
            byte[] mapInBytes = new byte[0]; ;
            //var cruiseItineraryDetails = ObjectMapper.Map<CruiseItineraryDetails>(input);
            if (!string.IsNullOrWhiteSpace(input.Photo))
            {
                var imageParts = input.Photo.Split(',').ToList<string>();
                mapInBytes = Convert.FromBase64String(imageParts[1]);
            }
            CruiseItineraryDetail cruiseItineraryDetails = new CruiseItineraryDetail();
            if (mapInBytes != null && mapInBytes.Length > 0)
            {
                cruiseItineraryDetails.Photo = mapInBytes;
            }
            cruiseItineraryDetails.Day = input.Day;
            cruiseItineraryDetails.PortID = input.PortID;
            cruiseItineraryDetails.Breakfast = input.Breakfast;
            cruiseItineraryDetails.Lunch = input.Lunch;
            cruiseItineraryDetails.AfternoonSnack = input.AfternoonSnack;
            cruiseItineraryDetails.Dinner = input.Dinner;
            cruiseItineraryDetails.CaptainDinner = input.CaptainDinner;
            cruiseItineraryDetails.LiveMusic = input.LiveMusic;
            cruiseItineraryDetails.Description = input.Description;
            cruiseItineraryDetails.CruiseItinerariesId = input.CruiseItinerariesId;
            cruiseItineraryDetails.OnAnchor = input.OnAnchor;
            cruiseItineraryDetails.Note = input.Note;

            if (AbpSession.TenantId != null)
            {
                cruiseItineraryDetails.TenantId = (int?)AbpSession.TenantId;
            }
            await _cruiseItineraryDetailsRepository.InsertAsync(cruiseItineraryDetails);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseItineraryDetails_Edit)]
        private async Task Update(CreateOrEditCruiseItineraryDetailsDto input)
        {
            var cruiseItineraryDetails = await _cruiseItineraryDetailsRepository.FirstOrDefaultAsync((int)input.Id);

            byte[] mapInBytes = new byte[0]; ;

            if (!string.IsNullOrWhiteSpace(input.Photo))
            {
                var imageParts = input.Photo.Split(',').ToList<string>();
                mapInBytes = Convert.FromBase64String(imageParts[1]);
            }
            if (mapInBytes != null && mapInBytes.Length > 0)
            {
                cruiseItineraryDetails.Photo = mapInBytes;
            }
            cruiseItineraryDetails.Day = input.Day;
            cruiseItineraryDetails.PortID = input.PortID;
            cruiseItineraryDetails.Breakfast = input.Breakfast;
            cruiseItineraryDetails.Lunch = input.Lunch;
            cruiseItineraryDetails.AfternoonSnack = input.AfternoonSnack;
            cruiseItineraryDetails.Dinner = input.Dinner;
            cruiseItineraryDetails.CaptainDinner = input.CaptainDinner;
            cruiseItineraryDetails.LiveMusic = input.LiveMusic;
            cruiseItineraryDetails.Description = input.Description;
            cruiseItineraryDetails.CruiseItinerariesId = input.CruiseItinerariesId;
            cruiseItineraryDetails.OnAnchor = input.OnAnchor;
            cruiseItineraryDetails.Note = input.Note;

            if (AbpSession.TenantId != null)
            {
                cruiseItineraryDetails.TenantId = (int?)AbpSession.TenantId;
            }
            await _cruiseItineraryDetailsRepository.UpdateAsync(cruiseItineraryDetails);


            //ObjectMapper.Map(input, cruiseItineraryDetails);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseItineraryDetails_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _cruiseItineraryDetailsRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseItineraryDetails)]
        public async Task<PagedResultDto<CruiseItineraryDetailsCruiseItinerariesLookupTableDto>> GetAllCruiseItinerariesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_cruiseItinerariesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.ItineraryName.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var cruiseItinerariesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CruiseItineraryDetailsCruiseItinerariesLookupTableDto>();
            foreach (var cruiseItineraries in cruiseItinerariesList)
            {
                lookupTableDtoList.Add(new CruiseItineraryDetailsCruiseItinerariesLookupTableDto
                {
                    Id = (int)cruiseItineraries.Id,
                    DisplayName = cruiseItineraries.ItineraryName?.ToString()
                });
            }

            return new PagedResultDto<CruiseItineraryDetailsCruiseItinerariesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }




    }
}
