using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using CruisePMS.Authorization;
using CruisePMS.Common.Dto;
using CruisePMS.Configuration;
using CruisePMS.CruiseDepartures;
using CruisePMS.CruiseDepartures.Dtos;
using CruisePMS.CruiseFares.Dtos;
using CruisePMS.CruiseMasterAmenities;
using CruisePMS.Cruises;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;

namespace CruisePMS.CruiseFares
{
    [AbpAuthorize(AppPermissions.Pages_CruiseFares)]
    public class CruiseFaresAppService : CruisePMSAppServiceBase, ICruiseFaresAppService
    {
        private readonly IRepository<CruiseFare> _cruiseFaresRepository;
        private readonly IRepository<Cruise, int> _lookup_cruisesRepository;
        private readonly IRepository<MasterAmenities, int> _lookup_cruiseMasterAmenitiesRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Cruise> _cruisesRepository;
        private readonly IRepository<CruiseDeparture> _cruiseDeparturesRepository;

        public CruiseFaresAppService(IRepository<CruiseDeparture> cruiseDeparturesRepository, IRepository<Cruise> cruisesRepository, IUnitOfWorkManager unitOfWorkManager, IRepository<CruiseFare> cruiseFaresRepository, IRepository<Cruise, int> lookup_cruisesRepository, IRepository<MasterAmenities, int> lookup_cruiseMasterAmenitiesRepository)
        {
            _cruiseFaresRepository = cruiseFaresRepository;
            _lookup_cruisesRepository = lookup_cruisesRepository;
            _lookup_cruiseMasterAmenitiesRepository = lookup_cruiseMasterAmenitiesRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _cruisesRepository = cruisesRepository;
            _cruiseDeparturesRepository = cruiseDeparturesRepository;
        }

        public async Task<PagedResultDto<GetCruiseFaresForViewDto>> GetAll(GetAllCruiseFaresInput input)
        {

            var filteredCruiseFares = _cruiseFaresRepository.GetAll()
                        .Include(e => e.CruisesFk)
                        .Include(e => e.FareNaFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinActivateDaysBeforeDepartureFilter != null, e => e.ActivateDaysBeforeDeparture >= input.MinActivateDaysBeforeDepartureFilter)
                        .WhereIf(input.MaxActivateDaysBeforeDepartureFilter != null, e => e.ActivateDaysBeforeDeparture <= input.MaxActivateDaysBeforeDepartureFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruiseMasterAmenitiesDisplayNameFilter), e => e.FareNaFk != null && e.FareNaFk.DisplayName.ToLower() == input.CruiseMasterAmenitiesDisplayNameFilter.ToLower().Trim())
                        .Where(x => x.CruisesId == input.CruiseId && x.FareName == input.SourceId);
            //SourceId
            var pagedAndFilteredCruiseFares = filteredCruiseFares
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var cruiseFares = from o in pagedAndFilteredCruiseFares
                              join o1 in _lookup_cruisesRepository.GetAll() on o.CruisesId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              join o2 in _lookup_cruiseMasterAmenitiesRepository.GetAll() on o.FareName equals o2.Id into j2
                              from s2 in j2.DefaultIfEmpty()

                              select new GetCruiseFaresForViewDto()
                              {
                                  CruiseFares = new CruiseFaresDto
                                  {
                                      IsMainFare = o.IsMainFare,
                                      FareStartDate = o.FareStartDate,
                                      FareEndDate = o.FareEndDate,
                                      Discount = o.Discount,
                                      ActivateDaysBeforeDeparture = o.ActivateDaysBeforeDeparture,
                                      FareName = o.FareName,
                                      Id = o.Id,
                                      FullPaymentRequired = o.FullPaymentRequired,
                                      DepartureDate = o.DepartureDate.Value,
                                      DepartureId = o.DepartureId.Value
                                  },
                                  CruisesTenantId = s1 == null ? "" : s1.TenantId.ToString(),
                                  CruiseMasterAmenitiesDisplayName = s2 == null ? "" : s2.DisplayName.ToString()
                              };

            var totalCount = await filteredCruiseFares.CountAsync();

            return new PagedResultDto<GetCruiseFaresForViewDto>(
                totalCount,
                await cruiseFares.OrderByDescending(x => x.CruiseMasterAmenitiesDisplayName).ToListAsync()
            );
        }

        public InsertdRecordsByCruiseId GetRecordsOfCruiseDeparturesDatesDeparture(int cruiseId)
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
            var filteredCruiseFares = _cruiseFaresRepository.GetAll()
                      .Include(e => e.CruisesFk)
                      .Include(e => e.FareNaFk)
                      .Where(x => x.CruisesId == cruiseId);
            if (filteredCruiseFares.Count() > 0)
            {
                foreach (var item in filteredCruiseDepartures)
                {
                    var checkInList = filteredCruiseFares.Where(x => x.DepartureDate.Value.Date == item.DepartureDate.Date).FirstOrDefault();
                    if (checkInList == null)
                    {
                        InsertedRecordsDto insertedRecordsDto = new InsertedRecordsDto();
                        insertedRecordsDto.CruiseId = item.CruisesId.Value;
                        insertedRecordsDto.DepartureDate = item.DepartureDate;
                        insertedRecordsDto.DepartureYear = item.DepartureYear;
                        insertedRecordsDto.Id = item.Id;
                        insertedRecordsDto.SeasonGroup = item.SeasonGroup;
                        insertdRecordsByCruiseId.insertedRecordsDtos.Add(insertedRecordsDto);
                    }
                }
            }
            else
            {
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
            }




            return insertdRecordsByCruiseId;
        }

        public async Task<PagedResultDto<GetCruiseFaresForViewDto>> GetAllCharterfaresByShip(GetAllCruiseFaresInput input)
        {

            var filteredCruiseFares = _cruiseFaresRepository.GetAll()
                        .Include(e => e.CruisesFk)
                        .Include(e => e.FareNaFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinActivateDaysBeforeDepartureFilter != null, e => e.ActivateDaysBeforeDeparture >= input.MinActivateDaysBeforeDepartureFilter)
                        .WhereIf(input.MaxActivateDaysBeforeDepartureFilter != null, e => e.ActivateDaysBeforeDeparture <= input.MaxActivateDaysBeforeDepartureFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruiseMasterAmenitiesDisplayNameFilter), e => e.FareNaFk != null && e.FareNaFk.DisplayName.ToLower() == input.CruiseMasterAmenitiesDisplayNameFilter.ToLower().Trim())
                        .Where(x => x.ShipId == input.ShipId);

            var pagedAndFilteredCruiseFares = filteredCruiseFares
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var cruiseFares = from o in pagedAndFilteredCruiseFares
                              join o1 in _lookup_cruisesRepository.GetAll() on o.CruisesId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              join o2 in _lookup_cruiseMasterAmenitiesRepository.GetAll() on o.FareName equals o2.Id into j2
                              from s2 in j2.DefaultIfEmpty()

                              select new GetCruiseFaresForViewDto()
                              {
                                  CruiseFares = new CruiseFaresDto
                                  {
                                      IsMainFare = o.IsMainFare,
                                      FareStartDate = o.FareStartDate,
                                      FareEndDate = o.FareEndDate,
                                      Discount = o.Discount,
                                      ActivateDaysBeforeDeparture = o.ActivateDaysBeforeDeparture,
                                      FareName = o.FareName,
                                      Id = o.Id,
                                      FullPaymentRequired = o.FullPaymentRequired
                                  },
                                  CruisesTenantId = s1 == null ? "" : s1.TenantId.ToString(),
                                  CruiseMasterAmenitiesDisplayName = s2 == null ? "" : s2.DisplayName.ToString()
                              };

            var totalCount = await filteredCruiseFares.CountAsync();

            return new PagedResultDto<GetCruiseFaresForViewDto>(
                totalCount,
                await cruiseFares.OrderByDescending(x => x.CruiseMasterAmenitiesDisplayName).ToListAsync()
            );
        }



        public async Task<GetCruiseFaresForViewDto> GetCruiseFaresForView(int id)
        {
            var cruiseFares = await _cruiseFaresRepository.GetAsync(id);

            var output = new GetCruiseFaresForViewDto { CruiseFares = ObjectMapper.Map<CruiseFaresDto>(cruiseFares) };

            if (output.CruiseFares.CruisesId != null)
            {
                var _lookupCruises = await _lookup_cruisesRepository.FirstOrDefaultAsync((int)output.CruiseFares.CruisesId);
                output.CruisesTenantId = _lookupCruises.TenantId.ToString();
            }

            if (output.CruiseFares.FareName != null)
            {
                var _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.CruiseFares.FareName);
                output.CruiseMasterAmenitiesDisplayName = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            return output;
        }
        //CharterDepartures
        [AbpAuthorize(AppPermissions.Pages_CruiseFares_Edit)]
        public async Task<GetCruiseFaresForEditOutput> GetCruiseFaresForEdit(EntityDto input)
        {
            var cruiseFares = await _cruiseFaresRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCruiseFaresForEditOutput { CruiseFares = ObjectMapper.Map<CreateOrEditCruiseFaresDto>(cruiseFares) };

            if (output.CruiseFares.CruisesId != null)
            {
                var _lookupCruises = await _lookup_cruisesRepository.FirstOrDefaultAsync((int)output.CruiseFares.CruisesId);
                output.CruisesTenantId = _lookupCruises.TenantId.ToString();
            }

            if (output.CruiseFares.FareName != null)
            {
                var _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.CruiseFares.FareName);
                output.CruiseMasterAmenitiesDisplayName = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCruiseFaresDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CruiseFares_Create)]
        private async Task Create(CreateOrEditCruiseFaresDto input)
        {
            var cruiseFares = ObjectMapper.Map<CruiseFare>(input);


            if (AbpSession.TenantId != null)
            {
                cruiseFares.TenantId = (int?)AbpSession.TenantId;
            }


            await _cruiseFaresRepository.InsertAsync(cruiseFares);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseFares_Edit)]
        private async Task Update(CreateOrEditCruiseFaresDto input)
        {
            var cruiseFares = await _cruiseFaresRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, cruiseFares);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseFares_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _cruiseFaresRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseFares)]
        public async Task<PagedResultDto<CruiseFaresCruisesLookupTableDto>> GetAllCruisesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_cruisesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.TenantId.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var cruisesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CruiseFaresCruisesLookupTableDto>();
            foreach (var cruises in cruisesList)
            {
                lookupTableDtoList.Add(new CruiseFaresCruisesLookupTableDto
                {
                    Id = cruises.Id,
                    DisplayName = cruises.TenantId?.ToString()
                });
            }

            return new PagedResultDto<CruiseFaresCruisesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseFares)]
        public async Task<PagedResultDto<CruiseFaresCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_cruiseMasterAmenitiesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DisplayName.ToString().Contains(input.Filter)
               ).Where(x => x.ParentId == 58);

            var totalCount = await query.CountAsync();

            var cruiseMasterAmenitiesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CruiseFaresCruiseMasterAmenitiesLookupTableDto>();
            foreach (var cruiseMasterAmenities in cruiseMasterAmenitiesList)
            {
                lookupTableDtoList.Add(new CruiseFaresCruiseMasterAmenitiesLookupTableDto
                {
                    Id = cruiseMasterAmenities.Id,
                    DisplayName = cruiseMasterAmenities.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<CruiseFaresCruiseMasterAmenitiesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}
