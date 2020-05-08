using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using CruisePMS.Authorization;
using CruisePMS.CruiseDepartures.Dtos;
using CruisePMS.Cruises;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using CruisePMS.Configuration;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;
using CruisePMS.Common.Dto;

namespace CruisePMS.CruiseDepartures
{
    [AbpAuthorize(AppPermissions.Pages_CruiseDepartures)]
    public class CruiseDeparturesAppService : CruisePMSAppServiceBase, ICruiseDeparturesAppService
    {
        private readonly IRepository<CruiseDeparture> _cruiseDeparturesRepository;
        private readonly IRepository<Cruise, int> _lookup_cruisesRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Cruise> _cruisesRepository;

        public CruiseDeparturesAppService(IRepository<Cruise> cruisesRepository, IUnitOfWorkManager unitOfWorkManager, IRepository<CruiseDeparture> cruiseDeparturesRepository,IRepository<Cruise, int> lookup_cruisesRepository)
        {
            _cruiseDeparturesRepository = cruiseDeparturesRepository;
            _lookup_cruisesRepository = lookup_cruisesRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _cruisesRepository = cruisesRepository;
        }

        public async Task<PagedResultDto<GetCruiseDeparturesForViewDto>> GetAll(GetAllCruiseDeparturesInput input)
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


            IQueryable<CruiseDeparture> filteredCruiseDepartures = _cruiseDeparturesRepository.GetAll()
                        .Include(e => e.CruisesFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SeasonGroup.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruisesTenantIdFilter), e => e.CruisesFk != null && e.CruisesFk.TenantId == Convert.ToInt32(input.CruisesTenantIdFilter))
                        .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == false, e => (e.DepartureYear == workingYear || e.DepartureYear == workingYear + 1))
                        .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == true, e => (e.DepartureYear == workingYear || e.DepartureYear == workingYear + 1 || e.DepartureYear <= workingYear))
                        .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == true, e => (e.DepartureYear == workingYear || e.DepartureYear <= workingYear))
                        .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == false, e => e.DepartureYear == workingYear)
                        .Where(x => x.CruisesId == input.CruiseId);


            if (input.DayNumber < 7) // all days
            {
                filteredCruiseDepartures = _cruiseDeparturesRepository.GetAll()
                        .Include(e => e.CruisesFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SeasonGroup.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruisesTenantIdFilter), e => e.CruisesFk != null && e.CruisesFk.TenantId == Convert.ToInt32(input.CruisesTenantIdFilter))
                        .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == false, e => (e.DepartureYear == workingYear || e.DepartureYear == workingYear + 1))
                        .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == true, e => (e.DepartureYear == workingYear || e.DepartureYear == workingYear + 1 || e.DepartureYear <= workingYear))
                        .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == true, e => (e.DepartureYear == workingYear || e.DepartureYear <= workingYear))
                        .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == false, e => e.DepartureYear == workingYear)
                        .Where(x => x.CruisesId == input.CruiseId && x.DepartureDate.DayOfWeek == (DayOfWeek)input.DayNumber);

            }



            var pagedAndFilteredCruiseDepartures = filteredCruiseDepartures
            .OrderBy(input.Sorting ?? "SeasonGroup asc")
            .PageBy(input);

            var cruiseDepartures = from o in pagedAndFilteredCruiseDepartures
                                   join o1 in _lookup_cruisesRepository.GetAll() on o.CruisesId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   select new GetCruiseDeparturesForViewDto()
                                   {
                                       CruiseDepartures = new CruiseDeparturesDto
                                       {
                                           DepartureYear = o.DepartureYear,
                                           SeasonGroup = o.SeasonGroup,
                                           DepartureDate = o.DepartureDate,
                                           IsDeleted = o.IsDeleted,
                                           Id = o.Id
                                       },
                                       CruisesTenantId = s1 == null ? "" : s1.TenantId.ToString()
                                   };

            var totalCount = await filteredCruiseDepartures.CountAsync();

            return new PagedResultDto<GetCruiseDeparturesForViewDto>(
                totalCount,
                await cruiseDepartures.ToListAsync()
            );
        }

        public async Task<GetCruiseDeparturesForViewDto> GetCruiseDeparturesForView(int id)
        {
            var cruiseDepartures = await _cruiseDeparturesRepository.GetAsync(id);

            var output = new GetCruiseDeparturesForViewDto { CruiseDepartures = ObjectMapper.Map<CruiseDeparturesDto>(cruiseDepartures) };

            if (output.CruiseDepartures.CruisesId != null)
            {
                var _lookupCruises = await _lookup_cruisesRepository.FirstOrDefaultAsync((int)output.CruiseDepartures.CruisesId);
                output.CruisesTenantId = _lookupCruises.TenantId.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseDepartures_Edit)]
        public async Task<GetCruiseDeparturesForEditOutput> GetCruiseDeparturesForEdit(EntityDto input)
        {
            var cruiseDepartures = await _cruiseDeparturesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCruiseDeparturesForEditOutput { CruiseDepartures = ObjectMapper.Map<CreateOrEditCruiseDeparturesDto>(cruiseDepartures) };

            if (output.CruiseDepartures.CruisesId != null)
            {
                var _lookupCruises = await _lookup_cruisesRepository.FirstOrDefaultAsync((int)output.CruiseDepartures.CruisesId);
                output.CruisesTenantId = _lookupCruises.TenantId.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseDepartures_Create)]
        public async Task CreateFromCruiseDefaultSeasons(List<CreateDeparturesDto> collection)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);

            var firstElement = collection.First();
            Cruise cruises = await _cruisesRepository.GetAsync(firstElement.CruiseId);
            collection = collection.OrderBy(i => i.Id).ToList();
            foreach (var item in collection)
            {
                if (item.IsChecked)
                {

                    CruiseDeparture cruiseDepartures = new CruiseDeparture();

                    if (AbpSession.TenantId != null)
                    {
                        cruiseDepartures.TenantId = (int?)AbpSession.TenantId;
                    }

                    var cruiseDepartureDateime = Convert.ToDateTime(item.DepartureDate.ToString("MM/dd/yyyy") + " " + cruises.CheckIn.ToString("HH:mm") + ":00.0000000");

                    cruiseDepartures.DepartureDate = cruiseDepartureDateime;
                    cruiseDepartures.DepartureYear = item.DepartureYear;
                    cruiseDepartures.SeasonGroup = item.SeasonGroup;
                    cruiseDepartures.CruisesId = item.CruiseId;
                    await _cruiseDeparturesRepository.InsertAsync(cruiseDepartures);
                }
            }

        }
        [AbpAuthorize(AppPermissions.Pages_CruiseDepartures_Create)]
        public async Task CreateEditedFromCruiseDefaultSeasons(List<CreateDeparturesDto> collection)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);


            var firstElement = collection.First();
            await _cruiseDeparturesRepository.DeleteAsync(x => x.CruisesId == firstElement.CruiseId);
            Cruise cruises = await _cruisesRepository.GetAsync(firstElement.CruiseId);
            collection = collection.OrderBy(i => i.Id).ToList();
            foreach (var item in collection)
            {
                if (item.IsChecked)
                {

                    CruiseDeparture cruiseDepartures = new CruiseDeparture();

                    if (AbpSession.TenantId != null)
                    {
                        cruiseDepartures.TenantId = (int?)AbpSession.TenantId;
                    }

                    var cruiseDepartureDateime = Convert.ToDateTime(item.DepartureDate.ToString("MM/dd/yyyy") + " " + cruises.CheckIn.ToString("HH:mm") + ":00.0000000");

                    cruiseDepartures.DepartureDate = cruiseDepartureDateime;
                    cruiseDepartures.DepartureYear = item.DepartureYear;
                    cruiseDepartures.SeasonGroup = item.SeasonGroup;
                    cruiseDepartures.CruisesId = item.CruiseId;
                    await _cruiseDeparturesRepository.InsertAsync(cruiseDepartures);
                }
            }

        }
        public InsertdRecordsByCruiseId GetRecordsOfDepartureByCruiseId(int cruiseId)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            InsertdRecordsByCruiseId insertdRecordsByCruiseId = new InsertdRecordsByCruiseId();
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
            IQueryable<CruiseDeparture> filteredCruiseDepartures = _cruiseDeparturesRepository.GetAll()
                       .Include(e => e.CruisesFk)
                       .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == false, e => (e.DepartureYear == workingYear || e.DepartureYear == workingYear + 1))
                       .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == true, e => (e.DepartureYear == workingYear || e.DepartureYear == workingYear + 1 || e.DepartureYear <= workingYear))
                       .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == true, e => (e.DepartureYear == workingYear || e.DepartureYear <= workingYear))
                       .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == false, e => e.DepartureYear == workingYear)
                       .Where(x => x.CruisesId == cruiseId);


            foreach (var item in filteredCruiseDepartures)
            {
                InsertedRecordsDto insertedRecordsDto = new InsertedRecordsDto();
                insertedRecordsDto.CruiseId = item.CruisesId.Value;
                insertedRecordsDto.DepartureDate = item.DepartureDate;
                insertedRecordsDto.DepartureYear = item.DepartureYear;
                insertedRecordsDto.Id = item.Id;
                insertedRecordsDto.SeasonGroup = item.SeasonGroup;

                insertdRecordsByCruiseId.insertedRecordsDtos.Add(insertedRecordsDto);

            }





            return insertdRecordsByCruiseId;
        }


        public async Task CreateOrEdit(CreateOrEditCruiseDeparturesDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CruiseDepartures_Create)]
        private async Task Create(CreateOrEditCruiseDeparturesDto input)
        {
            var cruiseDepartures = ObjectMapper.Map<CruiseDeparture>(input);


            if (AbpSession.TenantId != null)
            {
                cruiseDepartures.TenantId = (int?)AbpSession.TenantId;
            }


            await _cruiseDeparturesRepository.InsertAsync(cruiseDepartures);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseDepartures_Edit)]
        private async Task Update(CreateOrEditCruiseDeparturesDto input)
        {
            var cruiseDepartures = await _cruiseDeparturesRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, cruiseDepartures);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseDepartures_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _cruiseDeparturesRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseDepartures)]
        public async Task<PagedResultDto<CruiseDeparturesCruisesLookupTableDto>> GetAllCruisesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_cruisesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.TenantId.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var cruisesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CruiseDeparturesCruisesLookupTableDto>();
            foreach (var cruises in cruisesList)
            {
                lookupTableDtoList.Add(new CruiseDeparturesCruisesLookupTableDto
                {
                    Id = cruises.Id,
                    DisplayName = cruises.TenantId?.ToString()
                });
            }

            return new PagedResultDto<CruiseDeparturesCruisesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}
