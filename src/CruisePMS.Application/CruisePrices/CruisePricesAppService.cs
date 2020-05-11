using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using CruisePMS.Authorization;
using CruisePMS.CharterDepartures;
using CruisePMS.Common.Dto;
using CruisePMS.Configuration;
using CruisePMS.CruiseDepartures;
using CruisePMS.CruiseFares;
using CruisePMS.CruiseItineraries;
using CruisePMS.CruiseMasterAmenities;
using CruisePMS.CruisePrices.Dtos;
using CruisePMS.Cruises;
using CruisePMS.CruiseServiceGroups;
using CruisePMS.CruiseServices;
using CruisePMS.CruiseServiceUnits;
using CruisePMS.CruiseShipCabins;
using CruisePMS.CruiseShipDecks;
using CruisePMS.CruiseShips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;


namespace CruisePMS.CruisePrices
{
    [AbpAuthorize(AppPermissions.Pages_CruisePrices)]
    public class CruisePricesAppService : CruisePMSAppServiceBase, ICruisePricesAppService
    {
        //CharterDepartures



        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<CruiseShipDeck> _cruiseShipDecksRepository;
        private readonly IRepository<CruiseShip> _cruiseShipsRepository;
        private readonly IRepository<CruisePrice> _cruisePricesRepository;
        private readonly IRepository<CruiseFare, int> _lookup_cruiseFaresRepository;
        private readonly IRepository<CruiseDeparture, int> _lookup_cruiseDeparturesRepository;
        private readonly IRepository<CruiseShipCabin, int> _lookup_cruiseShipCabinsRepository;
        private readonly IRepository<CruiseService, int> _lookup_cruiseServicesRepository;
        private readonly IRepository<CruiseShip, int> _lookup_cruiseShipsRepository;
        private readonly IRepository<Cruise> _cruisesRepository;
        private readonly IRepository<MasterAmenities> _cruiseMasterAmenitiesRepository;
        private readonly IRepository<CharterDeparture> _charterDeparturesRepository;
        private readonly IRepository<CruiseServiceUnit> _cruiseServiceUnitsRepository;
        private readonly IRepository<CruiseItinerary, int> _lookup_cruiseItinerariesRepository;
        private readonly IRepository<CruiseService> _cruiseServicesRepository;
        private readonly IRepository<CruiseItinerary> _cruiseItinerariesRepository;
        private readonly IRepository<CruiseServiceGroup> _cruiseServiceGroupsRepository;
        public CruisePricesAppService(IRepository<CruiseServiceGroup> cruiseServiceGroupsRepository, IRepository<CruiseItinerary> cruiseItinerariesRepository, IRepository<CruiseService> cruiseServicesRepository, IUnitOfWorkManager unitOfWorkManager, IRepository<CruiseItinerary> lookup_cruiseItinerariesRepository, IRepository<CruiseServiceUnit> cruiseServiceUnitsRepository, IRepository<CharterDeparture> charterDeparturesRepository, IRepository<CruisePrice> cruisePricesRepository,IRepository<CruiseFare, int> lookup_cruiseFaresRepository, IRepository<CruiseDeparture, int> lookup_cruiseDeparturesRepository, IRepository<CruiseShipCabin, int> lookup_cruiseShipCabinsRepository, IRepository<CruiseService, int> lookup_cruiseServicesRepository, IRepository<CruiseShip, int> lookup_cruiseShipsRepository, IRepository<CruiseShip> cruiseShipsRepository, IRepository<Cruise> cruisesRepository, IRepository<CruiseShipDeck> cruiseShipDecksRepository, IRepository<MasterAmenities> cruiseMasterAmenitiesRepository)
        {

            _cruiseServicesRepository = cruiseServicesRepository;
            _charterDeparturesRepository = charterDeparturesRepository;
            _cruisePricesRepository = cruisePricesRepository;
            _lookup_cruiseFaresRepository = lookup_cruiseFaresRepository;
            _lookup_cruiseDeparturesRepository = lookup_cruiseDeparturesRepository;
            _lookup_cruiseShipCabinsRepository = lookup_cruiseShipCabinsRepository;
            _lookup_cruiseServicesRepository = lookup_cruiseServicesRepository;
            _lookup_cruiseShipsRepository = lookup_cruiseShipsRepository;
            _cruiseShipsRepository = cruiseShipsRepository;
            _cruisesRepository = cruisesRepository;
            _cruiseShipDecksRepository = cruiseShipDecksRepository;
            _cruiseMasterAmenitiesRepository = cruiseMasterAmenitiesRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _cruiseServiceUnitsRepository = cruiseServiceUnitsRepository;
            _lookup_cruiseItinerariesRepository = lookup_cruiseItinerariesRepository;
            _cruiseItinerariesRepository = cruiseItinerariesRepository;
            _cruiseServiceGroupsRepository = cruiseServiceGroupsRepository;
        }

        public async Task<PagedResultDto<GetCruisePricesForViewDto>> GetAll(GetAllCruisePricesInput input)
        {


            string defaultCurrentLanguage = await SettingManager.GetSettingValueForUserAsync(AppSettings.DefaultCurrentLanguage, AbpSession.ToUserIdentifier());
            if (string.IsNullOrWhiteSpace(defaultCurrentLanguage))
            { defaultCurrentLanguage = "EN"; }

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

            var filteredCruisePrices = _cruisePricesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CurrencyId.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruiseServicesServiceNameFilter), e => e.CruiseServicesFk != null && e.CruiseServicesFk.ServiceNaFk.DisplayName.ToLower() == input.CruiseServicesServiceNameFilter.ToLower().Trim())
                        .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == false, e => (e.PriceYear == workingYear || e.PriceYear == workingYear + 1))
                        .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == true, e => (e.PriceYear == workingYear || e.PriceYear == workingYear + 1 || e.PriceYear <= workingYear))
                        .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == true, e => (e.PriceYear == workingYear || e.PriceYear <= workingYear))
                        .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == false, e => e.PriceYear == workingYear)
                        .Where(x => x.CruiseId == input.CruiseId);



            var pagedAndFilteredCruisePrices = filteredCruisePrices
                .OrderBy(x => x.SeasonGroup).ThenByDescending(x => x.CruiseServicesId)
                .PageBy(input);
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            var cruisePrices = from o in pagedAndFilteredCruisePrices

                               join o1 in _lookup_cruiseFaresRepository.GetAll() on o.CruiseFaresId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               join o3 in _lookup_cruiseShipCabinsRepository.GetAll() on o.CruiseShipCabinsId equals o3.Id into j3
                               from s3 in j3.DefaultIfEmpty()

                               join o4 in _lookup_cruiseServicesRepository.GetAll() on o.CruiseServicesId equals o4.Id into j4
                               from s4 in j4.DefaultIfEmpty()



                               join o8 in _cruiseServiceUnitsRepository.GetAll() on s4.CruiseServiceUnitsId equals o8.Id into j8
                               from s8 in j8.DefaultIfEmpty()
                                   // service unit name Id
                               join o9 in _cruiseMasterAmenitiesRepository.GetAll() on s8.ServiceUnit equals o9.Id into j9
                               from s9 in j9.DefaultIfEmpty()

                               join o5 in _cruiseMasterAmenitiesRepository.GetAll() on s3.CruiseShipDecksId equals o5.Id into j5
                               from s5 in j5.DefaultIfEmpty()

                                   // deack name 
                               join o6 in _cruiseMasterAmenitiesRepository.GetAll() on o.DeckId equals o6.Id into j6
                               from s6 in j6.DefaultIfEmpty()

                                   // cabin type ID
                               join o7 in _cruiseMasterAmenitiesRepository.GetAll().Where(x => x.Lang.ToUpper() == defaultCurrentLanguage.ToUpper()) on o.CabinTypeId equals o7.Id into j7
                               from s7 in j7.DefaultIfEmpty()


                               select new GetCruisePricesForViewDto()
                               {
                                   CruisePrices = new CruisePricesDto
                                   {
                                       UnitPrice = o.UnitPrice,
                                       CurrencyId = o.CurrencyId,
                                       SeasonGroup = o.SeasonGroup,
                                       DeckName = s6 == null ? "" : s6.DisplayName.ToString(),
                                       CabintypeName = s7 == null ? "" : s7.DisplayName.ToString(),
                                       Id = o.Id
                                   },
                                   CruiseFaresFareName = s1 == null ? "" : s1.FareName.ToString(),
                                   CruiseShipCabinsCabinCategoryId = s5 == null ? "" : s5.DisplayName.ToString(),
                                   CruiseServicesServiceName = s4 == null ? "" : s4.ServiceNaFk.DisplayName.ToString(),
                                   CruiseServicesServiceUnit = s9 == null ? "" : s9.DisplayName.ToString()

                               };



            var totalCount = await filteredCruisePrices.CountAsync();

            return new PagedResultDto<GetCruisePricesForViewDto>(
                totalCount,
                await cruisePrices.ToListAsync()
            );
        }

        public async Task<GetCruisePricesForViewDto> GetCruisePricesForView(int id)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            var cruisePrices = await _cruisePricesRepository.GetAsync(id);

            var output = new GetCruisePricesForViewDto { CruisePrices = ObjectMapper.Map<CruisePricesDto>(cruisePrices) };

            if (output.CruisePrices.CruiseFaresId != null)
            {
                var _lookupCruiseFares = await _lookup_cruiseFaresRepository.FirstOrDefaultAsync((int)output.CruisePrices.CruiseFaresId);
                output.CruiseFaresFareName = _lookupCruiseFares.FareName.ToString();
            }

            if (output.CruisePrices.CruiseDeparturesId != null)
            {
                var _lookupCruiseDepartures = await _lookup_cruiseDeparturesRepository.FirstOrDefaultAsync((int)output.CruisePrices.CruiseDeparturesId);
                output.CruiseDeparturesDepartureDate = _lookupCruiseDepartures.DepartureDate.ToString();
            }

            if (output.CruisePrices.CruiseShipCabinsId != null)
            {
                var _lookupCruiseShipCabins = await _lookup_cruiseShipCabinsRepository.FirstOrDefaultAsync((int)output.CruisePrices.CruiseShipCabinsId);
                output.CruiseShipCabinsCabinCategoryId = _lookupCruiseShipCabins.CabinCategoryId.ToString();
            }

            if (output.CruisePrices.CruiseServicesId != null)
            {
                var _lookupCruiseServices = await _lookup_cruiseServicesRepository.FirstOrDefaultAsync((int)output.CruisePrices.CruiseServicesId);
                output.CruiseServicesServiceName = _lookupCruiseServices.ServiceName.ToString();
            }

            if (output.CruisePrices.CruiseShipsId != null)
            {
                var _lookupCruiseShips = await _lookup_cruiseShipsRepository.FirstOrDefaultAsync((int)output.CruisePrices.CruiseShipsId);
                output.CruiseShipsCruiseShipName = _lookupCruiseShips.CruiseShipName.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CruisePrices_Edit)]
        public async Task<GetCruisePricesForEditOutput> GetCruisePricesForEdit(EntityDto input)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            var cruisePrices = await _cruisePricesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCruisePricesForEditOutput { CruisePrices = ObjectMapper.Map<CreateOrEditCruisePricesDto>(cruisePrices) };

            if (output.CruisePrices.CruiseFaresId != null)
            {
                var _lookupCruiseFares = await _lookup_cruiseFaresRepository.FirstOrDefaultAsync((int)output.CruisePrices.CruiseFaresId);
                output.CruiseFaresFareName = _lookupCruiseFares.FareName.ToString();
            }

            if (output.CruisePrices.CruiseDeparturesId != null)
            {
                var _lookupCruiseDepartures = await _lookup_cruiseDeparturesRepository.FirstOrDefaultAsync((int)output.CruisePrices.CruiseDeparturesId);
                output.CruiseDeparturesDepartureDate = _lookupCruiseDepartures.DepartureDate.ToString();
            }

            if (output.CruisePrices.CruiseShipCabinsId != null)
            {
                var _lookupCruiseShipCabins = await _lookup_cruiseShipCabinsRepository.FirstOrDefaultAsync((int)output.CruisePrices.CruiseShipCabinsId);
                output.CruiseShipCabinsCabinCategoryId = _lookupCruiseShipCabins.CabinCategoryId.ToString();
            }

            if (output.CruisePrices.CruiseServicesId != null)
            {
                var _lookupCruiseServices = await _lookup_cruiseServicesRepository.FirstOrDefaultAsync((int)output.CruisePrices.CruiseServicesId);
                output.CruiseServicesServiceName = _lookupCruiseServices.ServiceName.ToString();
            }

            if (output.CruisePrices.CruiseShipsId != null)
            {
                var _lookupCruiseShips = await _lookup_cruiseShipsRepository.FirstOrDefaultAsync((int)output.CruisePrices.CruiseShipsId);
                output.CruiseShipsCruiseShipName = _lookupCruiseShips.CruiseShipName.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCruisePricesDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CruisePrices_Create)]
        private async Task Create(CreateOrEditCruisePricesDto input)
        {
            var cruisePrices = ObjectMapper.Map<CruisePrice>(input);


            if (AbpSession.TenantId != null)
            {
                cruisePrices.TenantId = (int?)AbpSession.TenantId;
            }


            await _cruisePricesRepository.InsertAsync(cruisePrices);
        }

        [AbpAuthorize(AppPermissions.Pages_CruisePrices_Edit)]
        private async Task Update(CreateOrEditCruisePricesDto input)
        {
            var cruisePrices = await _cruisePricesRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, cruisePrices);
        }

        [AbpAuthorize(AppPermissions.Pages_CruisePrices_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _cruisePricesRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_CruisePrices_Create)]
        public PriceScreenRecordDto GetCreatePriceScreenRecord(int cruiseId)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            int workingYear = DateTime.Now.Year;

            try
            {
                IEnumerable<ISettingValue> listSettings = SettingManager.GetAllSettingValuesForUserAsync(AbpSession.ToUserIdentifier()).Result;
                if (listSettings != null)
                {

                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault() != null)
                    {
                        workingYear = Convert.ToInt32(listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault().Value);
                    }
                }
            }
            catch (Exception)
            {

            }

            PriceScreenRecordDto priceScreenRecordDto = new PriceScreenRecordDto();
            try
            {
                string defaultCurrency = SettingManager.GetSettingValueForApplicationAsync(AppSettings.DefaultCurrency).Result;

                //var fares = _lookup_cruiseFaresRepository.GetAll().Where(x => x.Id == fareId).FirstOrDefault();
                var seasonGroups = _lookup_cruiseDeparturesRepository.GetAll().Where(x => x.CruisesId == cruiseId && x.DepartureYear == workingYear).
                   Select(m => new { m.SeasonGroup }).Distinct();
                var cruises = _cruisesRepository.GetAll().Where(x => x.Id == cruiseId).FirstOrDefault();
                var deckIds = _lookup_cruiseShipCabinsRepository.GetAll().Where(x => x.CruiseShipsId == cruises.CruiseShipsId)
                    .Select(m => new { m.CruiseShipDecksId, m.Id, m.CabinTypeID }).Distinct();

                var _lookupCruiseServices = _cruiseServicesRepository.FirstOrDefault((int)cruises.CruiseServicesId);
                var serviceUnit = _cruiseServiceUnitsRepository.FirstOrDefault((int)_lookupCruiseServices.CruiseServiceUnitsId);
                var masterAmenities = _cruiseMasterAmenitiesRepository.GetAll().Where(x => x.Id == serviceUnit.ServiceUnit.Value).FirstOrDefault();


                // var distinctgroupSubscriptions = deckIds.GroupBy(m => new { m.CruiseShipDecksId, m.Id, m.CabinTypeID })
                //                                .Select(group => group.First()).ToList();
                List<CheckDuplicate> checkDuplicatesDecks = new List<CheckDuplicate>();
                foreach (var deckId in deckIds)
                {
                    var match = checkDuplicatesDecks.FirstOrDefault(x => x.CabinTypeID == deckId.CabinTypeID && x.CruiseShipDecksId == deckId.CruiseShipDecksId);
                    if (match == null)
                    {
                        CheckDuplicate checkDuplicatevalues = new CheckDuplicate();
                        checkDuplicatevalues.CruiseShipDecksId = deckId.CruiseShipDecksId.Value;
                        checkDuplicatevalues.Id = deckId.Id;
                        checkDuplicatevalues.CabinTypeID = deckId.CabinTypeID;
                        checkDuplicatesDecks.Add(checkDuplicatevalues);
                    }
                }
                priceScreenRecordDto.FareId = 0;
                priceScreenRecordDto.CruiseId = cruiseId;
                priceScreenRecordDto.CruiseShipsId = cruises.CruiseShipsId.Value;
                priceScreenRecordDto.DefaultCurency = defaultCurrency;
                priceScreenRecordDto.ServiceUnit = masterAmenities.DisplayName.ToString();

                priceScreenRecordDto.SeasonGroup = new List<SeasonGroups>();
                foreach (var item in seasonGroups)
                {
                    SeasonGroups objSeasonGroup = new SeasonGroups();
                    objSeasonGroup.SeasonGroupName = item.SeasonGroup;
                    priceScreenRecordDto.SeasonGroup.Add(objSeasonGroup);
                }
                priceScreenRecordDto.ShipCabinDetail = new List<ShipCabinDetails>();
                foreach (var deckId in checkDuplicatesDecks)
                {
                    ShipCabinDetails objShipCabinDetails = new ShipCabinDetails();

                    //var ShipDecks = _cruiseShipDecksRepository.GetAll().Where(x => x.Id == deckId.CruiseShipDecksId).FirstOrDefault();
                    var deckName = _cruiseMasterAmenitiesRepository.GetAll().Where(x => x.Id == deckId.CruiseShipDecksId).Select(m => new { m.DisplayName }).FirstOrDefault();
                    var cabinType = _cruiseMasterAmenitiesRepository.GetAll().Where(x => x.Id == deckId.CabinTypeID).Select(m => new { m.DisplayName }).FirstOrDefault();

                    objShipCabinDetails.CruiseShipDecksId = deckId.CruiseShipDecksId;
                    objShipCabinDetails.Id = deckId.Id;
                    objShipCabinDetails.CabinTypeID = deckId.CabinTypeID;
                    objShipCabinDetails.DeckName = deckName.DisplayName;
                    objShipCabinDetails.CabinType = cabinType.DisplayName;

                    priceScreenRecordDto.ShipCabinDetail.Add(objShipCabinDetails);
                }
            }
            catch (Exception ex)
            {

            }
            return priceScreenRecordDto;
        }

        public CruisePriceModalsScreen CruisePriceModalsScreen(GetCruisePriceModalsScreen getCruisePriceModalsScreen)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            int workingYear = DateTime.Now.Year;

            try
            {
                IEnumerable<ISettingValue> listSettings = SettingManager.GetAllSettingValuesForUserAsync(AbpSession.ToUserIdentifier()).Result;
                if (listSettings != null)
                {
                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault() != null)
                    {
                        workingYear = Convert.ToInt32(listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault().Value);
                    }
                }
            }
            catch
            {

            }
            CruisePriceModalsScreen cruisePriceModalsScreen = new CruisePriceModalsScreen();
            var cruises = _cruisesRepository.GetAll().Where(x => x.Id == getCruisePriceModalsScreen.cruiseId).FirstOrDefault();
            string defaultCurrency = SettingManager.GetSettingValueForApplicationAsync(AppSettings.DefaultCurrency).Result;
            var seasonGroups = _lookup_cruiseDeparturesRepository.GetAll().Where(x => x.CruisesId == getCruisePriceModalsScreen.cruiseId && x.DepartureYear == workingYear).
               Select(m => new { m.SeasonGroup }).Distinct();
            var _lookupCruiseServices = _cruiseServicesRepository.FirstOrDefault((int)cruises.CruiseServicesId);
            var serviceUnit = _cruiseServiceUnitsRepository.FirstOrDefault((int)_lookupCruiseServices.CruiseServiceUnitsId);
            var masterAmenities = _cruiseMasterAmenitiesRepository.GetAll().Where(x => x.Id == serviceUnit.ServiceUnit.Value).FirstOrDefault();

            cruisePriceModalsScreen.CruiseId = getCruisePriceModalsScreen.cruiseId;
            cruisePriceModalsScreen.CruiseShipsId = cruises.CruiseShipsId.Value;
            cruisePriceModalsScreen.DefaultCurency = defaultCurrency;
            if (getCruisePriceModalsScreen.serviceGroupId == 5 || getCruisePriceModalsScreen.serviceGroupId == 7)
            {
                cruisePriceModalsScreen.DefaultCurency = "%";
            }
            cruisePriceModalsScreen.ServiceUnit = masterAmenities.DisplayName.ToString();
            // bind SeasonGroups
            cruisePriceModalsScreen.SeasonGroup = new List<SeasonGroups>();
            foreach (var item in seasonGroups)
            {
                SeasonGroups objSeasonGroup = new SeasonGroups();
                objSeasonGroup.SeasonGroupName = item.SeasonGroup;
                cruisePriceModalsScreen.SeasonGroup.Add(objSeasonGroup);
            }
            var _serviceList = _cruiseServicesRepository.GetAll().Where(x => x.CruiseServiceGroupsId == getCruisePriceModalsScreen.serviceGroupId);
            cruisePriceModalsScreen.ServiceList = new List<ServiceList>();
            foreach (var item in _serviceList)
            {
                ServiceList serviceList = new ServiceList();
                var masterAmenities_ServiecName = _cruiseMasterAmenitiesRepository.GetAll().Where(x => x.Id == item.ServiceName).FirstOrDefault();
                serviceList.ServiceName = masterAmenities_ServiecName.DisplayName;
                serviceList.ServiceId = item.Id;
                serviceList.CruiseServiceGroupsId = getCruisePriceModalsScreen.serviceGroupId;

                cruisePriceModalsScreen.ServiceList.Add(serviceList);
            }

            return cruisePriceModalsScreen;
        }

        [AbpAuthorize(AppPermissions.Pages_CruisePrices_Create)]
        public async Task SavePriceScreenRecord(List<SavePriceScreenRecordDto> items)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            int workingYear = DateTime.Now.Year;

            try
            {
                IEnumerable<ISettingValue> listSettings = SettingManager.GetAllSettingValuesForUserAsync(AbpSession.ToUserIdentifier()).Result;
                if (listSettings != null)
                {

                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault() != null)
                    {
                        workingYear = Convert.ToInt32(listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault().Value);
                    }
                }
            }
            catch (Exception)
            {

            }


            var firstElement = items.First();
            string[] seasonGroupList = firstElement.SeasonGroup.Split(',');

            string defaultCurrency = await SettingManager.GetSettingValueForApplicationAsync(AppSettings.DefaultCurrency);
            foreach (var item in items)
            {

                var objCruise = _cruisesRepository.GetAll().Where(x => x.Id == item.CruiseId).Where(x => x.CruiseYear == workingYear).FirstOrDefault();

                var _lookupCruiseServices = _cruiseServicesRepository.FirstOrDefault((int)objCruise.CruiseServicesId);
                int countOfseason = 1;
                foreach (var season in seasonGroupList)
                {
                    CruisePrice cruisePrices = new CruisePrice();
                    //cruisePrices.CruiseFaresId = item.FareId;
                    cruisePrices.CruiseServicesId = objCruise.CruiseServicesId.Value;
                    cruisePrices.ServiceUnitId = _lookupCruiseServices.CruiseServiceUnitsId;
                    //cruisePrices.CruiseShipCabinsId = item.CabinId;
                    cruisePrices.CruiseId = item.CruiseId;


                    cruisePrices.CurrencyId = defaultCurrency;
                    cruisePrices.DeckId = item.CruiseShipDecksId;
                    cruisePrices.CabinTypeId = item.CabinTypeID;
                    cruisePrices.PriceYear = item.PriceYear;
                    if (AbpSession.TenantId != null)
                    {
                        cruisePrices.TenantId = (int?)AbpSession.TenantId;
                    }
                    switch (countOfseason)
                    {
                        // season one
                        case 1:
                            if (item.TextBoxValue1)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice1);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                        // season second
                        case 2:
                            if (item.TextBoxValue2)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice2);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                        // season third
                        case 3:
                            if (item.TextBoxValue3)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice3);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                        // season forth
                        case 4:
                            if (item.TextBoxValue4)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice4);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                        // season fifth
                        case 5:
                            if (item.TextBoxValue5)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice5);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                        // season six
                        case 6:
                            if (item.TextBoxValue6)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice6);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                    }

                    await _cruisePricesRepository.InsertAsync(cruisePrices);
                    countOfseason++;
                }


            }
        }




        [AbpAuthorize(AppPermissions.Pages_CruisePrices_Create)]
        public async Task SavePriceModalScreenRecord(List<SavePriceScreenRecordDto> items)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            int workingYear = DateTime.Now.Year;

            try
            {
                IEnumerable<ISettingValue> listSettings = SettingManager.GetAllSettingValuesForUserAsync(AbpSession.ToUserIdentifier()).Result;
                if (listSettings != null)
                {

                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault() != null)
                    {
                        workingYear = Convert.ToInt32(listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault().Value);
                    }
                }
            }
            catch (Exception)
            {

            }


            var firstElement = items.First();
            string[] seasonGroupList = firstElement.SeasonGroup.Split(',');

            string defaultCurrency = await SettingManager.GetSettingValueForApplicationAsync(AppSettings.DefaultCurrency);
            foreach (var item in items)
            {

                var objCruise = _cruisesRepository.GetAll().Where(x => x.Id == item.CruiseId).Where(x => x.CruiseYear == workingYear).FirstOrDefault();

                var _lookupCruiseServices = _cruiseServicesRepository.FirstOrDefault((int)objCruise.CruiseServicesId);
                int countOfseason = 1;
                foreach (var season in seasonGroupList)
                {
                    CruisePrice cruisePrices = new CruisePrice();
                    cruisePrices.CruiseServicesId = item.CruiseServiceId;
                    cruisePrices.ServiceUnitId = _lookupCruiseServices.CruiseServiceUnitsId;
                    //cruisePrices.CruiseShipCabinsId = item.CabinId;
                    cruisePrices.CurrencyId = item.DefaultCurency;
                    cruisePrices.PriceYear = item.PriceYear;
                    cruisePrices.CruiseId = item.CruiseId;
                    if (AbpSession.TenantId != null)
                    {
                        cruisePrices.TenantId = (int?)AbpSession.TenantId;
                    }
                    switch (countOfseason)
                    {
                        // season one
                        case 1:
                            if (item.TextBoxValue1)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice1);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                        // season second
                        case 2:
                            if (item.TextBoxValue2)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice2);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                        // season third
                        case 3:
                            if (item.TextBoxValue3)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice3);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                        // season forth
                        case 4:
                            if (item.TextBoxValue4)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice4);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                        // season fifth
                        case 5:
                            if (item.TextBoxValue5)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice5);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                        // season six
                        case 6:
                            if (item.TextBoxValue6)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice6);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                    }

                    await _cruisePricesRepository.InsertAsync(cruisePrices);
                    countOfseason++;
                }


            }
        }

       


        [AbpAuthorize(AppPermissions.Pages_CruisePrices_Create)]
        public PriceScreenRecordDto GetCreateCharterPriceScreenRecord(int fareId)
        {
            int workingYear = DateTime.Now.Year;
            try
            {
                IEnumerable<ISettingValue> listSettings = SettingManager.GetAllSettingValuesForUserAsync(AbpSession.ToUserIdentifier()).Result;
                if (listSettings != null)
                {

                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault() != null)
                    {
                        workingYear = Convert.ToInt32(listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault().Value);
                    }
                }
            }
            catch (Exception)
            {

            }
            PriceScreenRecordDto priceScreenRecordDto = new PriceScreenRecordDto();
            try
            {
                string defaultCurrency = SettingManager.GetSettingValueForApplicationAsync(AppSettings.DefaultCurrency).Result;
                var fares = _lookup_cruiseFaresRepository.GetAll().Where(x => x.Id == fareId).FirstOrDefault();
                var seasonGroups = _charterDeparturesRepository.GetAll().
                    Where(x => x.CruiseShipId == fares.ShipId && x.DepartureYear == workingYear).
                   Select(m => new { m.SeasonGroup }).Distinct();


                priceScreenRecordDto.FareId = fareId;
                priceScreenRecordDto.CruiseShipsId = fares.ShipId.Value;
                priceScreenRecordDto.DefaultCurency = defaultCurrency;
                priceScreenRecordDto.SeasonGroup = new List<SeasonGroups>();
                foreach (var item in seasonGroups)
                {
                    SeasonGroups objSeasonGroup = new SeasonGroups();
                    objSeasonGroup.SeasonGroupName = item.SeasonGroup;

                    priceScreenRecordDto.SeasonGroup.Add(objSeasonGroup);
                }
            }
            catch (Exception ex)
            {
            }
            return priceScreenRecordDto;
        }


        [AbpAuthorize(AppPermissions.Pages_CruisePrices_Create)]
        public async Task SaveCharterPriceScreenRecord(List<SavePriceScreenRecordDto> items)
        {
            int workingYear = DateTime.Now.Year;

            try
            {
                IEnumerable<ISettingValue> listSettings = SettingManager.GetAllSettingValuesForUserAsync(AbpSession.ToUserIdentifier()).Result;
                if (listSettings != null)
                {

                    if (listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault() != null)
                    {
                        workingYear = Convert.ToInt32(listSettings.Where(o => o.Name == AppSettings.UserManagement.WorkingYear).SingleOrDefault().Value);
                    }
                }
            }
            catch (Exception)
            {

            }


            var firstElement = items.First();
            string[] seasonGroupList = firstElement.SeasonGroup.Split(',');

            string defaultCurrency = await SettingManager.GetSettingValueForApplicationAsync(AppSettings.DefaultCurrency);
            foreach (var item in items)
            {

                int countOfseason = 1;
                foreach (var season in seasonGroupList)
                {


                    CruisePrice cruisePrices = new CruisePrice();
                    cruisePrices.CruiseFaresId = item.FareId;
                    cruisePrices.ShipId = item.CruiseShipsId;
                    cruisePrices.CruiseServicesId = 6;
                    cruisePrices.CurrencyId = defaultCurrency;
                    cruisePrices.PriceYear = item.PriceYear;
                    if (AbpSession.TenantId != null)
                    {
                        cruisePrices.TenantId = (int?)AbpSession.TenantId;
                    }
                    switch (countOfseason)
                    {
                        // season one
                        case 1:
                            if (item.TextBoxValue1)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice1);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                        // season second
                        case 2:
                            if (item.TextBoxValue2)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice2);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                        // season third
                        case 3:
                            if (item.TextBoxValue3)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice3);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                        // season forth
                        case 4:
                            if (item.TextBoxValue4)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice4);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                        // season fifth
                        case 5:
                            if (item.TextBoxValue5)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice5);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                        // season six
                        case 6:
                            if (item.TextBoxValue6)
                            {
                                cruisePrices.UnitPrice = Convert.ToDecimal(item.UnitPrice6);
                                cruisePrices.SeasonGroup = season;
                            }
                            break;
                    }

                    await _cruisePricesRepository.InsertAsync(cruisePrices);
                    countOfseason++;
                }


            }
        }

        [AbpAuthorize(AppPermissions.Pages_CruisePrices)]
        public async Task<PagedResultDto<CruisePricesCruiseFaresLookupTableDto>> GetAllCruiseFaresForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_cruiseFaresRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FareName.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var cruiseFaresList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CruisePricesCruiseFaresLookupTableDto>();
            foreach (var cruiseFares in cruiseFaresList)
            {
                lookupTableDtoList.Add(new CruisePricesCruiseFaresLookupTableDto
                {
                    Id = cruiseFares.Id,
                    DisplayName = cruiseFares.FareName?.ToString()
                });
            }

            return new PagedResultDto<CruisePricesCruiseFaresLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CruisePrices)]
        public async Task<PagedResultDto<CruisePricesCruiseDeparturesLookupTableDto>> GetAllCruiseDeparturesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_cruiseDeparturesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DepartureDate.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var cruiseDeparturesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CruisePricesCruiseDeparturesLookupTableDto>();
            foreach (var cruiseDepartures in cruiseDeparturesList)
            {
                lookupTableDtoList.Add(new CruisePricesCruiseDeparturesLookupTableDto
                {
                    Id = cruiseDepartures.Id,
                    DisplayName = cruiseDepartures.DepartureDate.ToString()
                });
            }

            return new PagedResultDto<CruisePricesCruiseDeparturesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CruisePrices)]
        public async Task<PagedResultDto<CruisePricesCruiseShipCabinsLookupTableDto>> GetAllCruiseShipCabinsForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_cruiseShipCabinsRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.CabinCategoryId.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var cruiseShipCabinsList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CruisePricesCruiseShipCabinsLookupTableDto>();
            foreach (var cruiseShipCabins in cruiseShipCabinsList)
            {
                lookupTableDtoList.Add(new CruisePricesCruiseShipCabinsLookupTableDto
                {
                    Id = cruiseShipCabins.Id,
                    DisplayName = cruiseShipCabins.CabinCategoryId?.ToString()
                });
            }

            return new PagedResultDto<CruisePricesCruiseShipCabinsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CruisePrices)]
        public async Task<PagedResultDto<CruisePricesCruiseServicesLookupTableDto>> GetAllCruiseServicesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_cruiseServicesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.ServiceName.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var cruiseServicesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CruisePricesCruiseServicesLookupTableDto>();
            foreach (var cruiseServices in cruiseServicesList)
            {
                lookupTableDtoList.Add(new CruisePricesCruiseServicesLookupTableDto
                {
                    Id = cruiseServices.Id,
                    DisplayName = cruiseServices.ServiceName?.ToString()
                });
            }

            return new PagedResultDto<CruisePricesCruiseServicesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CruisePrices)]
        public async Task<PagedResultDto<CruisePricesCruiseShipsLookupTableDto>> GetAllCruiseShipsForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_cruiseShipsRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.CruiseShipName.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var cruiseShipsList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CruisePricesCruiseShipsLookupTableDto>();
            foreach (var cruiseShips in cruiseShipsList)
            {
                lookupTableDtoList.Add(new CruisePricesCruiseShipsLookupTableDto
                {
                    Id = cruiseShips.Id,
                    DisplayName = cruiseShips.CruiseShipName?.ToString()
                });
            }

            return new PagedResultDto<CruisePricesCruiseShipsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }


        [AbpAuthorize(AppPermissions.Pages_CruisePrices_Delete)]
        public async Task DeleteCruisePriceById(EntityDto input)
        {

            await _cruisePricesRepository.DeleteAsync(w => w.CruiseServicesId == 1);
        }
        [AbpAuthorize(AppPermissions.Pages_CruisePrices_Delete)]
        public async Task DeleteAllCruisePriceById(EntityDto input)
        {
            await _cruisePricesRepository.DeleteAsync(w => w.CruiseId == input.Id);
        }

    }
}
