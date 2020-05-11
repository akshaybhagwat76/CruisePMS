using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using CruisePMS.Authorization;
using CruisePMS.Common.Dto;
using System.Linq.Dynamic.Core;
using CruisePMS.Configuration;
using CruisePMS.CruiseItineraries;
using CruisePMS.CruiseMasterAmenities;
using CruisePMS.Cruises.Dtos;
using CruisePMS.CruiseServices;
using CruisePMS.CruiseShips;
using CruisePMS.CruiseThemes;
using CruisePMS.Localization;
using CruisePMS.MultiTenancy;
using CruisePMS.MultiTenancy.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruisePMS.Cruises
{
    [AbpAuthorize(AppPermissions.Pages_Cruises)]
    public class CruisesAppService : CruisePMSAppServiceBase, ICruisesAppService
    {
        private readonly IRepository<Cruise> _cruisesRepository;
        private readonly IRepository<CruiseShip, int> _lookup_cruiseShipsRepository;
        private readonly IRepository<CruiseTheme, int> _lookup_cruiseThemesRepository;
        private readonly IRepository<CruiseService, int> _lookup_cruiseServicesRepository;
        private readonly IRepository<CruiseItinerary, int> _lookup_CruiseItineraryRepository;
        private readonly IRepository<ip2location_city_multilingual, int> _lookup_cruiseportRepository;
        private readonly IRepository<MasterAmenities, int> _lookup_cruiseMasterAmenitiesRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CruisesAppService(IRepository<Cruise> cruisesRepository, IRepository<CruiseShip, int> lookup_cruiseShipsRepository, IRepository<CruiseTheme, int> lookup_cruiseThemesRepository, IRepository<CruiseService, int> lookup_cruiseServicesRepository, IRepository<CruiseItinerary> lookup_CruiseItineraryRepository, IUnitOfWorkManager unitOfWorkManager, IRepository<ip2location_city_multilingual, int> lookup_cruiseportRepository, IRepository<MasterAmenities, int> lookup_cruiseMasterAmenitiesRepository)
        {
            _cruisesRepository = cruisesRepository;
            _unitOfWorkManager = unitOfWorkManager;

            _lookup_cruiseShipsRepository = lookup_cruiseShipsRepository;
            _lookup_cruiseThemesRepository = lookup_cruiseThemesRepository;
            _lookup_cruiseServicesRepository = lookup_cruiseServicesRepository;
            _lookup_CruiseItineraryRepository = lookup_CruiseItineraryRepository;
            _lookup_cruiseportRepository = lookup_cruiseportRepository;
            _lookup_cruiseMasterAmenitiesRepository = lookup_cruiseMasterAmenitiesRepository;
        }

        public async Task<int> GetWorkingYear()
        {
            int workingYear = DateTime.Now.Year;

            try
            {
                IEnumerable<ISettingValue> listSettings = SettingManager.GetAllSettingValuesForUserAsync(AbpSession.ToUserIdentifier()).Result;
                if (listSettings != null)
                {
                    workingYear = Convert.ToInt32(listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).FirstOrDefault().Value);
                }
            }
            catch (Exception)
            {

            }

            return workingYear;
        }

        public async Task<PagedResultDto<GetCruisesForViewDto>> GetAllShipes()
        {
            try
            {
                IEnumerable<ISettingValue> listSettings = SettingManager.GetAllSettingValuesForUserAsync(AbpSession.ToUserIdentifier()).Result.Where(o => o.Name == AppSettings.UserManagement.ShowDeletedYearData);
                if (listSettings != null)
                {
                    if (listSettings.SingleOrDefault() != null)
                    {
                        if (Convert.ToBoolean(listSettings.SingleOrDefault().Value))
                        {
                            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            IQueryable<Cruise> filteredCruises = _cruisesRepository.GetAll()
                                    .Include(e => e.CruiseShipsFk)
                                    .Include(e => e.CruiseItinerariesFk);

            IQueryable<GetCruisesForViewDto> cruises = (from o in filteredCruises
                                                        join o1 in _lookup_cruiseShipsRepository.GetAll() on o.CruiseShipsId equals o1.Id into j1
                                                        from s1 in j1.DefaultIfEmpty()

                                                        join o2 in _lookup_cruiseThemesRepository.GetAll() on o.CruiseThemesId equals o2.Id into j2
                                                        from s2 in j2.DefaultIfEmpty()

                                                        join o3 in _lookup_cruiseServicesRepository.GetAll() on o.CruiseServicesId equals o3.Id into j3
                                                        from s3 in j3.DefaultIfEmpty()

                                                        join o4 in _lookup_CruiseItineraryRepository.GetAll() on o.CruiseItinerariesId equals o4.Id into j4
                                                        from s4 in j4.DefaultIfEmpty()

                                                        select new GetCruisesForViewDto()
                                                        {
                                                            Cruises = new CruisesDto
                                                            {
                                                                CruiseDuration = o.CruiseDuration,
                                                                CruiseIsEnabled = o.CruiseIsEnabled,
                                                                CruiseIsEnabledB2B = o.CruiseIsEnabledB2B,
                                                                DisableForApi = o.DisableForApi,
                                                                CheckIn = o.CheckIn,
                                                                Checkout = o.Checkout,
                                                                Id = o.Id,
                                                                IsDeleted = o.IsDeleted
                                                            },
                                                            CruiseShipsCruiseShipName = s1 == null ? "" : s1.CruiseShipName.ToString(),
                                                            CruiseThemesCruiseThemeName = s2 == null ? "" : s2.CruiseThemeName.ToString(),
                                                            CruiseServicesServiceName = s3 == null ? "" : s3.ServiceName.ToString(),
                                                            CruiseItinerariesItineraryName = s4 == null ? "" : s4.ItineraryName.ToString()
                                                        }).OrderByDescending(o => o.Cruises.Id);
            var totalCount = await filteredCruises.CountAsync();
            return new PagedResultDto<GetCruisesForViewDto>(
               totalCount,
               await cruises.ToListAsync()
           );
        }
        public async Task<PagedResultDto<TenantListDto>> GetAllTenant()
        {
            IQueryable<Tenant> query = TenantManager.Tenants;
            int totalCount = await query.CountAsync();
            List<Tenant> Alltenants = await query.ToListAsync();
            List<TenantListDto> GetTenantList = new List<TenantListDto>();
            foreach (Tenant GetTenant in Alltenants)
            {
                GetTenantList.Add(new TenantListDto
                {
                    Id = GetTenant.Id,
                    TenancyName = GetTenant.TenancyName
                });
            }
            return new PagedResultDto<TenantListDto>(totalCount, GetTenantList.ToList());
        }

        public async Task<PagedResultDto<IP2LocationCitiesDto>> GetAllPorts()
        {
            try
            {
                IEnumerable<ISettingValue> listSettings = SettingManager.GetAllSettingValuesForUserAsync(AbpSession.ToUserIdentifier()).Result.Where(o => o.Name == AppSettings.UserManagement.ShowDeletedYearData);
                if (listSettings != null)
                {
                    if (listSettings.SingleOrDefault() != null)
                    {
                        if (Convert.ToBoolean(listSettings.SingleOrDefault().Value))
                        {
                            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            IQueryable<Cruise> filteredCruises = _cruisesRepository.GetAll()
                                    .Include(e => e.CruiseShipsFk)
                                    .Include(e => e.CruiseItinerariesFk);

            ICollection<IP2LocationCitiesDto> cruises = (from o in filteredCruises
                                                         join o5 in _lookup_cruiseportRepository.GetAll() on o.CruiseStartPort equals o5.Id
                                                         into j5
                                                         from s5 in j5

                                                         select new IP2LocationCitiesDto()
                                                         {
                                                             Id = s5.Id,
                                                             city_name = s5.city_name
                                                         }).GroupBy(n => new { n.Id, n.lang_city_name })
                                                        .Select(g => g.FirstOrDefault())
                                                        .ToList();

            return new PagedResultDto<IP2LocationCitiesDto>(
               cruises.Count(),
               cruises.ToList()
            );
        }




        public async Task<PagedResultDto<GetCruisesForViewDto>> GetAll(GetAllCruisesInput input)
        {
            //_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);

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


            IQueryable<Cruise> filteredCruises = _cruisesRepository.GetAll()
                        .Include(e => e.CruiseShipsFk)
                        .Include(e => e.CruiseItinerariesFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruiseShipsCruiseShipNameFilter), e => e.CruiseShipsFk != null && e.CruiseShipsFk.CruiseShipName.ToLower() == input.CruiseShipsCruiseShipNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruiseThemesCruiseThemeNameFilter), e => e.CruiseThemesFk != null && e.CruiseThemesFk.CruiseThemeNaFk.DisplayName.ToLower() == input.CruiseShipsCruiseShipNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.CruiseItinerariesFk.ItineraryName.Contains(input.Filter))
                        .WhereIf(input.OneWay == true, e => (e.CruiseStartPort != e.CruiseEndPort))
                        .WhereIf(input.cruiseDurationFilter > 0, e => (e.CruiseDuration == input.cruiseDurationFilter))
                        .WhereIf(input.aPIFilter == true, e => (e.DisableForApi == input.aPIFilter))
                        .WhereIf(input.virtualFilter == true, e => (e.VirtualCruise == input.virtualFilter))
                        .WhereIf(input.B2BFilter == true, e => (e.CruiseIsEnabledB2B == input.B2BFilter))
                        .WhereIf(input.cruiseIsEnabledFilter == true, e => (e.CruiseIsEnabled == input.cruiseIsEnabledFilter))
                        .WhereIf(input.transferIncludedFilter == true, e => (e.TransferIncluded == input.transferIncludedFilter))
                        .WhereIf(input.cruisePortFilter > 0, e => (e.CruiseStartPort == input.cruisePortFilter))
                        .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == false, e => (e.CruiseYear == workingYear || e.CruiseYear == workingYear + 1))
                        .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == true, e => (e.CruiseYear == workingYear || e.CruiseYear == workingYear + 1 || e.CruiseYear <= workingYear))
                        .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == true, e => (e.CruiseYear == workingYear || e.CruiseYear <= workingYear))
                        .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == false, e => e.CruiseYear == workingYear);


            IQueryable<Cruise> pagedAndFilteredCruises = filteredCruises
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);


            IQueryable<GetCruisesForViewDto> cruises = (from o in pagedAndFilteredCruises
                                                        join o1 in _lookup_cruiseShipsRepository.GetAll() on o.CruiseShipsId equals o1.Id into j1
                                                        from s1 in j1.DefaultIfEmpty()

                                                        join o2 in _lookup_cruiseThemesRepository.GetAll() on o.CruiseThemesId equals o2.Id into j2
                                                        from s2 in j2.DefaultIfEmpty()



                                                        join o3 in _lookup_cruiseServicesRepository.GetAll() on o.CruiseServicesId equals o3.Id into j3
                                                        from s3 in j3.DefaultIfEmpty()

                                                        join o4 in _lookup_CruiseItineraryRepository.GetAll() on o.CruiseItinerariesId equals o4.Id into j4
                                                        from s4 in j4.DefaultIfEmpty()

                                                        join o5 in _lookup_cruiseportRepository.GetAll() on o.CruiseStartPort equals o5.Id into j5
                                                        from s5 in j5.DefaultIfEmpty()

                                                        join o6 in _lookup_cruiseportRepository.GetAll() on o.CruiseEndPort equals o6.Id into j6
                                                        from s6 in j6.DefaultIfEmpty()

                                                        join o7 in _lookup_cruiseMasterAmenitiesRepository.GetAll() on s2.CruiseThemeName equals o7.Id into j7
                                                        from s7 in j7.DefaultIfEmpty()



                                                        select new GetCruisesForViewDto()
                                                        {
                                                            Cruises = new CruisesDto
                                                            {
                                                                CruiseDuration = o.CruiseDuration,
                                                                CruiseStartPort = s5.lang_city_name,
                                                                CruiseEndPort = s6.lang_city_name,
                                                                CruiseIsEnabled = o.CruiseIsEnabled,
                                                                CruiseIsEnabledB2B = o.CruiseIsEnabledB2B,
                                                                DisableForApi = o.DisableForApi,
                                                                CheckIn = o.CheckIn,
                                                                Checkout = o.Checkout,
                                                                Id = o.Id,
                                                                IsDeleted = o.IsDeleted,
                                                                CruiseYear = o.CruiseYear,
                                                                BookingEmail = o.BookingEmail,
                                                                TransferIncluded = o.TransferIncluded,
                                                                VirtualCruise = o.VirtualCruise,

                                                            },
                                                            CruiseShipsCruiseShipName = s1 == null ? "" : s1.CruiseShipName.ToString(),
                                                            CruiseThemesCruiseThemeName = s7 == null ? "" : s7.DisplayName.ToString(),
                                                            CruiseServicesServiceName = s3 == null ? "" : s3.ServiceName.ToString(),
                                                            CruiseItinerariesItineraryName = s4 == null ? "" : s4.ItineraryName.ToString()
                                                        }).OrderByDescending(o => o.Cruises.Id);

            int totalCount = await filteredCruises.CountAsync();

            return new PagedResultDto<GetCruisesForViewDto>(
                totalCount,
                await cruises.ToListAsync()
            );
        }

        public async Task<GetCruisesForViewDto> GetCruisesForView(int id)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            Cruise cruises = await _cruisesRepository.GetAsync(id);

            GetCruisesForViewDto output = new GetCruisesForViewDto { Cruises = ObjectMapper.Map<CruisesDto>(cruises) };

            if (output.Cruises.CruiseShipsId != null)
            {
                CruiseShip _lookupCruiseShips = await _lookup_cruiseShipsRepository.FirstOrDefaultAsync((int)output.Cruises.CruiseShipsId);
                output.CruiseShipsCruiseShipName = _lookupCruiseShips.CruiseShipName.ToString();
            }

            if (output.Cruises.CruiseThemesId != null)
            {
                CruiseTheme _lookupCruiseThemes = await _lookup_cruiseThemesRepository.FirstOrDefaultAsync((int)output.Cruises.CruiseThemesId);
                var _masterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)_lookupCruiseThemes.CruiseThemeName);
                output.CruiseThemesCruiseThemeName = _masterAmenities.DisplayName.ToString();
            }

            if (output.Cruises.CruiseServicesId != null)
            {
                CruiseService _lookupCruiseServices = await _lookup_cruiseServicesRepository.FirstOrDefaultAsync((int)output.Cruises.CruiseServicesId);
                output.CruiseServicesServiceName = _lookupCruiseServices.ServiceName.ToString();
            }

            if (output.Cruises.CruiseItinerariesId != null)
            {
                CruiseItinerary _lookupCruiseItinerary = await _lookup_CruiseItineraryRepository.FirstOrDefaultAsync((int)output.Cruises.CruiseItinerariesId);
                output.CruiseItinerariesItineraryName = _lookupCruiseItinerary.ItineraryName.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Cruises_Edit)]
        public async Task<GetCruisesForEditOutput> GetCruisesForEdit(EntityDto input)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            GetCruisesForEditOutput output = new GetCruisesForEditOutput();
            try
            {


                Cruise cruises = await _cruisesRepository.FirstOrDefaultAsync(input.Id);

                output = new GetCruisesForEditOutput { Cruises = ObjectMapper.Map<CreateOrEditCruisesDto>(cruises) };

                if (output.Cruises.CruiseShipsId != null)
                {
                    CruiseShip _lookupCruiseShips = await _lookup_cruiseShipsRepository.FirstOrDefaultAsync((int)output.Cruises.CruiseShipsId);
                    output.CruiseShipsCruiseShipName = _lookupCruiseShips.CruiseShipName.ToString();
                }
                if (output.Cruises.CruiseServicesId != null)
                {
                    CruiseService _lookupCruiseServices = await _lookup_cruiseServicesRepository.FirstOrDefaultAsync((int)output.Cruises.CruiseServicesId);
                    output.CruiseServicesServiceName = _lookupCruiseServices.ServiceName.ToString();
                }

                if (output.Cruises.CruiseItinerariesId != null)
                {
                    CruiseItinerary _lookupCruiseItinerary = await _lookup_CruiseItineraryRepository.FirstOrDefaultAsync((int)output.Cruises.CruiseItinerariesId);
                    output.CruiseItinerariesItineraryName = _lookupCruiseItinerary.ItineraryName.ToString();
                }

                if (output.Cruises.CruiseThemesId != null)
                {
                    CruiseTheme _lookupCruiseThemes = await _lookup_cruiseThemesRepository.FirstOrDefaultAsync((int)output.Cruises.CruiseThemesId);
                    var _masterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)_lookupCruiseThemes.CruiseThemeName);
                    output.CruiseThemesCruiseThemeName = _masterAmenities.DisplayName.ToString();

                }



                return output;
            }
            catch (Exception ex)
            {
                return output;
            }
        }

        public async Task CreateOrEdit(CreateOrEditCruisesDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Cruises_Create)]
        private async Task Create(CreateOrEditCruisesDto input)
        {
            Cruise cruises = new Cruise
            {
                CheckIn = DateTime.ParseExact(input.CheckIn, "MM/dd/yyyy HH:mm",
                                              CultureInfo.InvariantCulture),
                Checkout = DateTime.ParseExact(input.Checkout, "MM/dd/yyyy HH:mm",
                                              CultureInfo.InvariantCulture),
                CruiseDuration = input.CruiseDuration,
                CruiseStartPort = input.CruiseStartPort,
                CruiseEndPort = input.CruiseEndPort,
                CruiseIsEnabled = input.CruiseIsEnabled,
                CruiseIsEnabledB2B = input.CruiseIsEnabledB2B,
                CruiseItinerariesId = input.CruiseItinerariesId,
                CruiseServicesId = input.CruiseServicesId,
                CruiseShipsId = input.CruiseShipsId,
                CruiseThemesId = input.CruiseThemesId,
                CruiseYear = input.CruiseYear,
                Cruise_Airport = input.Cruise_Airport,
                DepositType = input.DepositType,
                DisableForApi = input.DisableForApi,
                FreeInternet = input.FreeInternet,
                BookingEmail = input.BookingEmail,
                TransferIncluded = input.TransferIncluded,
                CruiseOperatorId = input.CruiseOperatorId,
                StandardDeposit = input.StandardDeposit,
                VirtualCruise = input.VirtualCruise
            };

            if (AbpSession.TenantId != null)
            {
                cruises.TenantId = AbpSession.TenantId;
            }

            await _cruisesRepository.InsertAsync(cruises);
        }

        [AbpAuthorize(AppPermissions.Pages_Cruises_Edit)]
        private async Task Update(CreateOrEditCruisesDto input)
        {
            Cruise cruises = await _cruisesRepository.FirstOrDefaultAsync((int)input.Id);

            cruises.CheckIn = DateTime.ParseExact(input.CheckIn, "MM/dd/yyyy HH:mm",
                                             CultureInfo.InvariantCulture);
            cruises.Checkout = DateTime.ParseExact(input.Checkout, "MM/dd/yyyy HH:mm",
                                              CultureInfo.InvariantCulture);

            cruises.CruiseDuration = input.CruiseDuration;
            cruises.CruiseStartPort = input.CruiseStartPort;
            cruises.CruiseEndPort = input.CruiseEndPort;

            cruises.CruiseIsEnabled = input.CruiseIsEnabled;
            cruises.CruiseIsEnabledB2B = input.CruiseIsEnabledB2B;
            cruises.CruiseItinerariesId = input.CruiseItinerariesId;
            cruises.CruiseServicesId = input.CruiseServicesId;
            cruises.CruiseShipsId = input.CruiseShipsId;
            cruises.CruiseThemesId = input.CruiseThemesId;
            cruises.CruiseYear = input.CruiseYear;

            cruises.Cruise_Airport = input.Cruise_Airport;
            cruises.DepositType = input.DepositType;
            cruises.DisableForApi = input.DisableForApi;
            cruises.FreeInternet = input.FreeInternet;

            cruises.StandardDeposit = input.StandardDeposit;
            cruises.VirtualCruise = input.VirtualCruise;
            cruises.BookingEmail = input.BookingEmail;
            cruises.TransferIncluded = input.TransferIncluded;
            cruises.CruiseOperatorId = input.CruiseOperatorId;
            if (AbpSession.TenantId != null)
            {
                cruises.TenantId = AbpSession.TenantId;
            }

            await _cruisesRepository.UpdateAsync(cruises);

            //ObjectMapper.Map(input, cruises);
        }

        [AbpAuthorize(AppPermissions.Pages_Cruises_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _cruisesRepository.DeleteAsync(input.Id);
        }

        #region Moved to CommonPickupAppServices
        [AbpAuthorize(AppPermissions.Pages_Cruises)]
        public async Task<PagedResultDto<CruisesCruiseShipsLookupTableDto>> GetAllCruiseShipsForLookupTable(GetAllForLookupTableInput input)
        {



            IQueryable<CruiseShip> query = _lookup_cruiseShipsRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
               e => e.CruiseShipName.ToString().Contains(input.Filter)
            );

            int totalCount = await query.CountAsync();

            List<CruiseShip> cruiseShipsList = await query
                .PageBy(input)
                .ToListAsync();

            List<CruisesCruiseShipsLookupTableDto> lookupTableDtoList = new List<CruisesCruiseShipsLookupTableDto>();
            foreach (CruiseShip cruiseShips in cruiseShipsList)
            {
                lookupTableDtoList.Add(new CruisesCruiseShipsLookupTableDto
                {
                    Id = cruiseShips.Id,
                    DisplayName = cruiseShips.CruiseShipName?.ToString()
                });
            }

            return new PagedResultDto<CruisesCruiseShipsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );

        }
        #endregion
        #region Moved to CommonPickupAppServices
        [AbpAuthorize(AppPermissions.Pages_Cruises)]
        public async Task<PagedResultDto<CruisesCruiseThemesLookupTableDto>> GetAllCruiseThemesForLookupTable(GetAllForLookupTableInput input)
        {

            IQueryable<CruiseTheme> query = _lookup_cruiseThemesRepository.GetAll()
                                    .Include(e => e.CruiseThemeNaFk)
                                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CruiseThemeNaFk.DisplayName.Contains(input.Filter));

            int totalCount = await query.CountAsync();

            List<CruiseTheme> cruiseThemesList = await query
                .PageBy(input)
                .ToListAsync();

            List<CruisesCruiseThemesLookupTableDto> lookupTableDtoList = new List<CruisesCruiseThemesLookupTableDto>();
            foreach (CruiseTheme cruiseThemes in cruiseThemesList)
            {
                lookupTableDtoList.Add(new CruisesCruiseThemesLookupTableDto
                {
                    Id = cruiseThemes.Id,
                    DisplayName = _lookup_cruiseMasterAmenitiesRepository.GetAll().Where(o => o.Id == cruiseThemes.CruiseThemeName && o.ParentId == 72).FirstOrDefault()?.DisplayName
                });
            }

            return new PagedResultDto<CruisesCruiseThemesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );

        }
        #endregion
        [AbpAuthorize(AppPermissions.Pages_Cruises)]
        public async Task<PagedResultDto<CruisesCruiseServicesLookupTableDto>> GetAllCruiseServicesForLookupTable(GetAllForLookupTableInput input)
        {
            IQueryable<CruiseService> query = _lookup_cruiseServicesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.ServiceName.ToString().Contains(input.Filter)
                );

            int totalCount = await query.CountAsync();

            List<CruiseService> cruiseServicesList = await query
                .PageBy(input)
                .ToListAsync();

            List<CruisesCruiseServicesLookupTableDto> lookupTableDtoList = new List<CruisesCruiseServicesLookupTableDto>();
            foreach (CruiseService cruiseServices in cruiseServicesList)
            {
                lookupTableDtoList.Add(new CruisesCruiseServicesLookupTableDto
                {
                    Id = cruiseServices.Id,
                    DisplayName = cruiseServices.ServiceName?.ToString()
                });
            }

            return new PagedResultDto<CruisesCruiseServicesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        #region Moved to CommonPickupAppServices
        [AbpAuthorize(AppPermissions.Pages_Cruises)]
        [UnitOfWork]
        public async Task<PagedResultDto<CruisesCruiseItinerariesLookupTableDto>> GetAllCruiseItinerariesForLookupTable(GetAllForLookupTableInput input)
        {


            IQueryable<CruiseItinerary> query = _lookup_CruiseItineraryRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.ItineraryName.ToString().Contains(input.Filter)
                );

            int totalCount = await query.CountAsync();

            List<CruiseItinerary> CruiseItineraryList = await query
                .PageBy(input)
                .ToListAsync();

            List<CruisesCruiseItinerariesLookupTableDto> lookupTableDtoList = new List<CruisesCruiseItinerariesLookupTableDto>();
            foreach (CruiseItinerary CruiseItinerary in CruiseItineraryList)
            {
                lookupTableDtoList.Add(new CruisesCruiseItinerariesLookupTableDto
                {
                    Id = CruiseItinerary.Id,
                    DisplayName = CruiseItinerary.ItineraryName?.ToString()
                });
            }

            return new PagedResultDto<CruisesCruiseItinerariesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        #endregion

    }
}
