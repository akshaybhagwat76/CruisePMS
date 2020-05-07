using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using System.Linq.Dynamic.Core;

using Abp.Runtime.Session;
using CruisePMS.Authorization;
using CruisePMS.Configuration;
using CruisePMS.CruiseDefaultSeasons.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Globalization;
using Newtonsoft.Json;

namespace CruisePMS.CruiseDefaultSeasons
{
    [AbpAuthorize(AppPermissions.Pages_CruisesAllDeparture)]
    public class CruiseDefaultSeasonsAppService : CruisePMSAppServiceBase, ICruiseDefaultSeasonsAppService
    {
        private readonly IRepository<CruiseDefaultSeason> _cruiseDefaultSeasonsRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CruiseDefaultSeasonsAppService(IUnitOfWorkManager unitOfWorkManager, IRepository<CruiseDefaultSeason> cruiseDefaultSeasonsRepository)
        {
            _cruiseDefaultSeasonsRepository = cruiseDefaultSeasonsRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<PagedResultDto<GetCruiseDefaultSeasonsForViewDto>> GetAll(GetAllCruiseDefaultSeasonsInput input)
        {

            int workingYear = DateTime.Now.Year;
            bool showNextYear = false;
            bool showHistoryData = false;
            try
            {
                IEnumerable<ISettingValue> listSettings = SettingManager.GetAllSettingValuesForUserAsync(AbpSession.ToUserIdentifier()).Result;
                if (listSettings != null)
                {
                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.ShowDeletedYearData).SingleOrDefault() != null)
                    {
                        if (Convert.ToBoolean(listSettings.Where(o => o.Name == AppSettings.UserManagement.ShowDeletedYearData).SingleOrDefault().Value))
                        {
                            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete);
                        }
                    }
                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.ShowNextYearData).SingleOrDefault() != null)
                    {
                        if (Convert.ToBoolean(listSettings.Where(o => o.Name == AppSettings.UserManagement.ShowNextYearData).SingleOrDefault().Value))
                        {
                            showNextYear = true;
                        }
                    }
                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.ShowHistoryData).SingleOrDefault() != null)
                    {
                        if (Convert.ToBoolean(listSettings.Where(o => o.Name == AppSettings.UserManagement.ShowHistoryData).SingleOrDefault().Value))
                        {
                            showHistoryData = true;
                        }
                    }
                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault() != null)
                    {
                        workingYear = Convert.ToInt32(listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault().Value);
                    }
                }
            }
            catch (Exception)
            {

            }






            IQueryable<CruiseDefaultSeason> filteredCruiseDefaultSeasons = filteredCruiseDefaultSeasons = _cruiseDefaultSeasonsRepository.GetAll()
                          .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SeasonGroup.Contains(input.Filter))
                          .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == false, e => (e.DepartureYear == workingYear || e.DepartureYear == workingYear + 1))
                          .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == true, e => (e.DepartureYear == workingYear || e.DepartureYear == workingYear + 1 || e.DepartureYear <= workingYear))
                          .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == true, e => (e.DepartureYear == workingYear || e.DepartureYear <= workingYear))
                          .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == false, e => e.DepartureYear == workingYear);




            if (input.DayNumber < 7) // all days
            {
                filteredCruiseDefaultSeasons = _cruiseDefaultSeasonsRepository.GetAll()
                          .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SeasonGroup.Contains(input.Filter))
                          .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == false, e => (e.DepartureYear == workingYear || e.DepartureYear == workingYear + 1))
                          .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == true, e => (e.DepartureYear == workingYear || e.DepartureYear == workingYear + 1 || e.DepartureYear <= workingYear))
                          .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == true, e => (e.DepartureYear == workingYear || e.DepartureYear <= workingYear))
                          .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == false, e => e.DepartureYear == workingYear)
                          .Where(x => x.DepartureDate.DayOfWeek == (DayOfWeek)input.DayNumber);
            }

            var pagedAndFilteredCruiseDefaultSeasons = filteredCruiseDefaultSeasons
                .OrderBy(input.Sorting ?? "SeasonGroup asc")
                .PageBy(input);

            var cruiseDefaultSeasons = from o in pagedAndFilteredCruiseDefaultSeasons
                                       group o by new { o.DepartureYear, o.SeasonGroup, o.Id, o.DepartureDate } into g
                                       select new GetCruiseDefaultSeasonsForViewDto()
                                       {
                                           CruiseDefaultSeasons = new CruiseDefaultSeasonsDto
                                           {
                                               DepartureYear = g.Key.DepartureYear,
                                               SeasonGroup = g.Key.SeasonGroup,
                                               DepartureDate = g.Key.DepartureDate,
                                               Id = g.Key.Id
                                           }
                                       };

            var totalCount = await filteredCruiseDefaultSeasons.CountAsync();

            return new PagedResultDto<GetCruiseDefaultSeasonsForViewDto>(
                totalCount,
                await cruiseDefaultSeasons.ToListAsync()
            );
        }


        public async Task<PagedResultDto<GetCruiseDefaultSeasonsForViewDto>> GetAllSavedDates()
        {

            int workingYear = DateTime.Now.Year;
            bool showNextYear = false;
            bool showHistoryData = false;
            try
            {
                IEnumerable<ISettingValue> listSettings = SettingManager.GetAllSettingValuesForUserAsync(AbpSession.ToUserIdentifier()).Result;
                if (listSettings != null)
                {
                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.ShowDeletedYearData).SingleOrDefault() != null)
                    {
                        if (Convert.ToBoolean(listSettings.Where(o => o.Name == AppSettings.UserManagement.ShowDeletedYearData).SingleOrDefault().Value))
                        {
                            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete);
                        }
                    }
                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.ShowNextYearData).SingleOrDefault() != null)
                    {
                        if (Convert.ToBoolean(listSettings.Where(o => o.Name == AppSettings.UserManagement.ShowNextYearData).SingleOrDefault().Value))
                        {
                            showNextYear = true;
                        }
                    }
                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.ShowHistoryData).SingleOrDefault() != null)
                    {
                        if (Convert.ToBoolean(listSettings.Where(o => o.Name == AppSettings.UserManagement.ShowHistoryData).SingleOrDefault().Value))
                        {
                            showHistoryData = true;
                        }
                    }
                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault() != null)
                    {
                        workingYear = Convert.ToInt32(listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault().Value);
                    }
                }
            }
            catch (Exception)
            {

            }
            IQueryable<CruiseDefaultSeason> filteredCruiseDefaultSeasons = filteredCruiseDefaultSeasons = _cruiseDefaultSeasonsRepository.GetAll()
                          .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == false, e => (e.DepartureYear == workingYear || e.DepartureYear == workingYear + 1))
                          .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == true, e => (e.DepartureYear == workingYear || e.DepartureYear == workingYear + 1 || e.DepartureYear <= workingYear))
                          .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == true, e => (e.DepartureYear == workingYear || e.DepartureYear <= workingYear))
                          .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == false, e => e.DepartureYear == workingYear);

            var pagedAndFilteredCruiseDefaultSeasons = filteredCruiseDefaultSeasons
                      .OrderBy("SeasonGroup asc");


            var cruiseDefaultSeasons = from o in pagedAndFilteredCruiseDefaultSeasons
                                       group o by new { o.DepartureYear, o.SeasonGroup, o.Id, o.DepartureDate } into g
                                       select new GetCruiseDefaultSeasonsForViewDto()
                                       {
                                           CruiseDefaultSeasons = new CruiseDefaultSeasonsDto
                                           {
                                               DepartureYear = g.Key.DepartureYear,
                                               SeasonGroup = g.Key.SeasonGroup,
                                               DepartureDate = g.Key.DepartureDate,
                                               Id = g.Key.Id
                                           }
                                       };

            var totalCount = await filteredCruiseDefaultSeasons.CountAsync();

            return new PagedResultDto<GetCruiseDefaultSeasonsForViewDto>(
                totalCount,
                await cruiseDefaultSeasons.ToListAsync()
            );
        }


        public async Task<PagedResultDto<GetCruiseDefaultSeasonsForViewDto>> GetAllCruiseDefaultSeasons(int dayNumber)
        {
            int workingYear = DateTime.Now.Year;
            bool showNextYear = false;
            bool showHistoryData = false;
            try
            {
                IEnumerable<ISettingValue> listSettings = SettingManager.GetAllSettingValuesForUserAsync(AbpSession.ToUserIdentifier()).Result;
                if (listSettings != null)
                {
                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.ShowNextYearData).SingleOrDefault() != null)
                    {
                        if (Convert.ToBoolean(listSettings.Where(o => o.Name == AppSettings.UserManagement.ShowNextYearData).SingleOrDefault().Value))
                        {
                            showNextYear = true;
                        }
                    }
                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.ShowHistoryData).SingleOrDefault() != null)
                    {
                        if (Convert.ToBoolean(listSettings.Where(o => o.Name == AppSettings.UserManagement.ShowHistoryData).SingleOrDefault().Value))
                        {
                            showHistoryData = true;
                        }
                    }
                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault() != null)
                    {
                        workingYear = Convert.ToInt32(listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault().Value);
                    }
                }
            }
            catch (Exception)
            {

            }
            IQueryable<CruiseDefaultSeason> filteredCruiseDefaultSeasons = _cruiseDefaultSeasonsRepository.GetAll().Take(100)
                .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == false, e => (e.DepartureYear == workingYear || e.DepartureYear == workingYear + 1))
                .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == true, e => (e.DepartureYear == workingYear || e.DepartureYear == workingYear + 1 || e.DepartureYear <= workingYear))
                .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == true, e => (e.DepartureYear == workingYear || e.DepartureYear <= workingYear))
                .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == false, e => e.DepartureYear == workingYear)
                .Where(x => x.DepartureYear == workingYear);

            if (dayNumber < 7) // all days
            {
                filteredCruiseDefaultSeasons = _cruiseDefaultSeasonsRepository.GetAll().Take(100)
                .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == false, e => (e.DepartureYear == workingYear || e.DepartureYear == workingYear + 1))
                .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == true, e => (e.DepartureYear == workingYear || e.DepartureYear == workingYear + 1 || e.DepartureYear <= workingYear))
                .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == true, e => (e.DepartureYear == workingYear || e.DepartureYear <= workingYear))
                .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == false, e => e.DepartureYear == workingYear)
                .Where(x => x.DepartureYear == workingYear && x.DepartureDate.DayOfWeek == (DayOfWeek)dayNumber);
            }

            var pagedAndFilteredCruiseDefaultSeasons = filteredCruiseDefaultSeasons
            .OrderBy("SeasonGroup asc");

            var cruiseDefaultSeasons = from o in pagedAndFilteredCruiseDefaultSeasons
                                       select new GetCruiseDefaultSeasonsForViewDto()
                                       {
                                           CruiseDefaultSeasons = new CruiseDefaultSeasonsDto
                                           {
                                               DepartureYear = o.DepartureYear,
                                               SeasonGroup = o.SeasonGroup,
                                               DepartureDate = o.DepartureDate,
                                               Id = o.Id
                                           }
                                       };

            var totalCount = await filteredCruiseDefaultSeasons.CountAsync();

            return new PagedResultDto<GetCruiseDefaultSeasonsForViewDto>(
                totalCount,
                await cruiseDefaultSeasons.ToListAsync()
            );
        }


        public async Task<GetCruiseDefaultSeasonsForViewDto> GetCruiseDefaultSeasonsForView(int id)
        {
            var cruiseDefaultSeasons = await _cruiseDefaultSeasonsRepository.GetAsync(id);

            var output = new GetCruiseDefaultSeasonsForViewDto { CruiseDefaultSeasons = ObjectMapper.Map<CruiseDefaultSeasonsDto>(cruiseDefaultSeasons) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseDefaultSeasons_Edit)]
        public async Task<GetCruiseDefaultSeasonsForEditOutput> GetCruiseDefaultSeasonsForEdit(EntityDto input)
        {
            var cruiseDefaultSeasons = await _cruiseDefaultSeasonsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCruiseDefaultSeasonsForEditOutput { CruiseDefaultSeasons = ObjectMapper.Map<CreateOrEditCruiseDefaultSeasonsDto>(cruiseDefaultSeasons) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCruiseDefaultSeasonsDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CruiseDefaultSeasons_Create)]
        private async Task Create(CreateOrEditCruiseDefaultSeasonsDto input)
        {
            var cruiseDefaultSeasons = ObjectMapper.Map<CruiseDefaultSeason>(input);


            if (AbpSession.TenantId != null)
            {
                cruiseDefaultSeasons.TenantId = (int?)AbpSession.TenantId;
            }


            await _cruiseDefaultSeasonsRepository.InsertAsync(cruiseDefaultSeasons);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseDefaultSeasons_Edit)]
        private async Task Update(CreateOrEditCruiseDefaultSeasonsDto input)
        {
            var cruiseDefaultSeasons = await _cruiseDefaultSeasonsRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, cruiseDefaultSeasons);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseDefaultSeasons_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _cruiseDefaultSeasonsRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseDefaultSeasons_Create)]
        public async Task CreateCruiseDefaultSeasons(string collection)
        {
            var myList = JsonConvert.DeserializeObject<List<CreateCruiseDefaultSeasonLoop>>(collection);
            foreach (var item in myList)
            {
                CruiseDefaultSeason cruiseDefaultSeasons = new CruiseDefaultSeason();
                if (AbpSession.TenantId != null)
                {
                    cruiseDefaultSeasons.TenantId = (int?)AbpSession.TenantId;
                }
                cruiseDefaultSeasons.CreatorUserId = (int)AbpSession.UserId;
                cruiseDefaultSeasons.DepartureDate = DateTime.ParseExact(item.DepartureDate, "MM/dd/yyyy",
                                             CultureInfo.InvariantCulture);
                cruiseDefaultSeasons.DepartureYear = item.DepartureYear;
                cruiseDefaultSeasons.SeasonGroup = item.SeasonGroup;
                await _cruiseDefaultSeasonsRepository.InsertAsync(cruiseDefaultSeasons);
            }
        }


    }
}
