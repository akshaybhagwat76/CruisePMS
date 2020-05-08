using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using CruisePMS.Authorization;
using CruisePMS.Authorization.Users;
using CruisePMS.ContractCommissions.Dtos;
using CruisePMS.Contracts;
using CruisePMS.CruiseCabinAmenities;
using CruisePMS.CruiseItineraries;
using CruisePMS.CruiseMasterAmenities;
using CruisePMS.Cruises;
using CruisePMS.CruiseServices;
using CruisePMS.CruiseShips;
using CruisePMS.MainServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using CruisePMS.Common.Dto;

namespace CruisePMS.ContractCommissions
{
    [AbpAuthorize(AppPermissions.Pages_CruiseContractCommissions)]
    public class CruiseContractCommissionsAppService : CruisePMSAppServiceBase, IContractCommissionsAppService
    {
        private readonly IRepository<ContractCommission> _cruiseContractCommissionsRepository;
        private readonly IRepository<Cruise, int> _lookup_cruisesRepository;
        private readonly IRepository<CruiseShip, int> _lookup_cruiseShipsRepository;
        private readonly IRepository<Contract, int> _lookup_cruiseContractRepository;
        private readonly IRepository<CruiseService, int> _lookup_cruiseServicesRepository;
        private readonly IRepository<CruiseItinerary, int> _lookup_cruiseItinerariesRepository;
        private readonly IRepository<MainService> _lookup_mainServicesRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<CruiseCabinAmenity> _cruiseCabinAmenitiesRepository;
        private readonly IRepository<MasterAmenities> _cruiseMasterAmenitiesRepository;
        private readonly UserManager _userManager;
        public CruiseContractCommissionsAppService(UserManager userManager, IRepository<MasterAmenities> cruiseMasterAmenitiesRepository, 
            IRepository<CruiseCabinAmenity> cruiseCabinAmenitiesRepository, 
            IUnitOfWorkManager unitOfWorkManager, IRepository<MainService> lookup_mainServicesRepository,
            IRepository<CruiseItinerary> lookup_cruiseItinerariesRepository, IRepository<ContractCommission> cruiseContractCommissionsRepository,
             IRepository<Cruise, int> lookup_cruisesRepository, IRepository<CruiseShip, int> lookup_cruiseShipsRepository, IRepository<Contract, int> lookup_cruiseContractRepository, IRepository<CruiseService, int> lookup_cruiseServicesRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _cruiseContractCommissionsRepository = cruiseContractCommissionsRepository;
            _lookup_cruisesRepository = lookup_cruisesRepository;
            _lookup_cruiseShipsRepository = lookup_cruiseShipsRepository;
            _lookup_cruiseContractRepository = lookup_cruiseContractRepository;
            _lookup_cruiseServicesRepository = lookup_cruiseServicesRepository;
            _lookup_cruiseItinerariesRepository = lookup_cruiseItinerariesRepository;
            _lookup_mainServicesRepository = lookup_mainServicesRepository;
            _cruiseCabinAmenitiesRepository = cruiseCabinAmenitiesRepository;
            _cruiseMasterAmenitiesRepository = cruiseMasterAmenitiesRepository;
            _userManager = userManager;
        }

        public async Task<PagedResultDto<GetCruiseContractCommissionsForViewDto>> GetAll(GetAllCruiseContractCommissionsInput input)
        {

            IQueryable<CruiseService> cruiseServices = null;
            IQueryable<CruiseItinerary> cruiseItineraries = null;
            IQueryable<CruiseShip> cruiseShips = null;
            IQueryable<MasterAmenities> cruiseMasterAmenities = null;
            IQueryable<Cruise> cruises = null;
            IQueryable<ContractCommission> filteredCruiseContractCommissions = null;
            IQueryable<Contract> cruiseContractRepository = null;


            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                cruiseServices = _lookup_cruiseServicesRepository.GetAll();
                cruiseItineraries = _lookup_cruiseItinerariesRepository.GetAll();
                cruiseMasterAmenities = _cruiseMasterAmenitiesRepository.GetAll();
                cruiseShips = _lookup_cruiseShipsRepository.GetAll();
                cruises = _lookup_cruisesRepository.GetAll();


                filteredCruiseContractCommissions = _cruiseContractCommissionsRepository.GetAll().Where(x => x.CruiseContractId == input.ContractId);

                cruiseContractRepository = _lookup_cruiseContractRepository.GetAll();

                // 





                var pagedAndFilteredCruiseContractCommissions = filteredCruiseContractCommissions
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var cruiseContractCommissions = from o in pagedAndFilteredCruiseContractCommissions
                                                join o1 in cruises on o.CruisesId equals o1.Id into j1
                                                from s1 in j1.DefaultIfEmpty()

                                                join o2 in cruiseShips on o.CruiseShipsId equals o2.Id into j2
                                                from s2 in j2.DefaultIfEmpty()

                                                join o3 in cruiseContractRepository on o.CruiseContractId equals o3.Id into j3
                                                from s3 in j3.DefaultIfEmpty()

                                                join o4 in cruiseServices on o.CruiseServicesId equals o4.Id into j4
                                                from s4 in j4.DefaultIfEmpty()

                                                join o5 in cruiseItineraries on s1.CruiseItinerariesId equals o5.Id into j5
                                                from s5 in j5.DefaultIfEmpty()

                                                join o6 in cruiseMasterAmenities on s4.ServiceName equals o6.Id into j6
                                                from s6 in j6.DefaultIfEmpty()


                                                select new GetCruiseContractCommissionsForViewDto()
                                                {
                                                    CruiseContractCommissions = new CruiseContractCommissionsDto
                                                    {
                                                        CreditCurency = o.CreditCurency,
                                                        CommissionType = o.CommissionType,
                                                        Id = o.Id,
                                                        Commission = o.Commission
                                                    },
                                                    CruisesCruiseName = s1 == null ? "" : s5.ItineraryName.ToString(),
                                                    CruiseShipsCruiseShipName = s2 == null ? "" : s2.CruiseShipName.ToString(),
                                                    CruiseContractContractName = s3 == null ? "" : s3.Season.ToString(),
                                                    CruiseServicesServiceName = s4 == null ? "" : s6.DisplayName.ToString()


                                                };

                var totalCount = await filteredCruiseContractCommissions.CountAsync();

                return new PagedResultDto<GetCruiseContractCommissionsForViewDto>(
                    totalCount,
                    await cruiseContractCommissions.ToListAsync()
                );
            }
        }



        public async Task<GetCruiseContractCommissionsForViewDto> GetCruiseContractCommissionsForView(int id)
        {
            var cruiseContractCommissions = await _cruiseContractCommissionsRepository.GetAsync(id);

            var output = new GetCruiseContractCommissionsForViewDto { CruiseContractCommissions = ObjectMapper.Map<CruiseContractCommissionsDto>(cruiseContractCommissions) };

            if (output.CruiseContractCommissions.CruisesId != null)
            {
                var _lookupCruises = await _lookup_cruisesRepository.FirstOrDefaultAsync((int)output.CruiseContractCommissions.CruisesId);
                var _lookupItenarry = await _lookup_cruiseItinerariesRepository.FirstOrDefaultAsync((int)_lookupCruises.CruiseItinerariesId);
                output.CruisesCruiseName = _lookupItenarry.ItineraryName.ToString(); //_lookupCruises.CruiseName.ToString();
            }

            if (output.CruiseContractCommissions.CruiseShipsId != null)
            {
                var _lookupCruiseShips = await _lookup_cruiseShipsRepository.FirstOrDefaultAsync((int)output.CruiseContractCommissions.CruiseShipsId);
                output.CruiseShipsCruiseShipName = _lookupCruiseShips.CruiseShipName.ToString();
            }

            if (output.CruiseContractCommissions.CruiseContractId != null)
            {
                var _lookupCruiseContract = await _lookup_cruiseContractRepository.FirstOrDefaultAsync((int)output.CruiseContractCommissions.CruiseContractId);
                output.CruiseContractContractName = _lookupCruiseContract.TenantId.ToString();
            }

            if (output.CruiseContractCommissions.CruiseServicesId != null)
            {
                var _lookupCruiseServices = await _lookup_cruiseServicesRepository.FirstOrDefaultAsync((int)output.CruiseContractCommissions.CruiseServicesId);
                output.CruiseServicesServiceName = _lookupCruiseServices.ServiceName.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseContractCommissions_Edit)]
        public async Task<GetCruiseContractCommissionsForEditOutput> GetCruiseContractCommissionsForEdit(EntityDto input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var cruiseContractCommissions = await _cruiseContractCommissionsRepository.FirstOrDefaultAsync(input.Id);

                var output = new GetCruiseContractCommissionsForEditOutput { CruiseContractCommissions = ObjectMapper.Map<CreateOrEditCruiseContractCommissionsDto>(cruiseContractCommissions) };

                if (output.CruiseContractCommissions.CruisesId != null)
                {
                    var _lookupCruises = await _lookup_cruisesRepository.FirstOrDefaultAsync((int)output.CruiseContractCommissions.CruisesId);
                    var _lookupItenarry = await _lookup_cruiseItinerariesRepository.FirstOrDefaultAsync((int)_lookupCruises.CruiseItinerariesId);
                    output.CruisesCruiseName = _lookupItenarry.ItineraryName.ToString(); //_lookupCruises.CruiseName.ToString();
                }

                if (output.CruiseContractCommissions.CruiseShipsId != null)
                {
                    var _lookupCruiseShips = await _lookup_cruiseShipsRepository.FirstOrDefaultAsync((int)output.CruiseContractCommissions.CruiseShipsId);
                    output.CruiseShipsCruiseShipName = _lookupCruiseShips.CruiseShipName.ToString();
                }

                //if (output.CruiseContractCommissions.CruiseContractId != null)
                //{
                //    var _lookupCruiseContract = await _lookup_cruiseContractRepository.FirstOrDefaultAsync((int)output.CruiseContractCommissions.CruiseContractId);
                //    output.CruiseContractContractName = _lookupCruiseContract.TenantId.ToString();
                //}

                if (output.CruiseContractCommissions.CruiseServicesId != null)
                {
                    var _lookupCruiseServices = await _lookup_cruiseServicesRepository.FirstOrDefaultAsync((int)output.CruiseContractCommissions.CruiseServicesId);
                    var _lookUpManenites = await _cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)_lookupCruiseServices.ServiceName);
                    output.CruiseServicesServiceName = _lookUpManenites.DisplayName.ToString();
                }

                return output;
            }
        }

        public async Task CreateOrEdit(CreateOrEditCruiseContractCommissionsDto input)
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
        [AbpAuthorize(AppPermissions.Pages_CruiseContractCommissions_Create)]
        protected virtual async Task Create(CreateOrEditCruiseContractCommissionsDto input)
        {
            var cruiseContractCommissions = ObjectMapper.Map<ContractCommission>(input);


            if (AbpSession.TenantId != null)
            {
                cruiseContractCommissions.TenantId = (int?)AbpSession.TenantId;
            }


            await _cruiseContractCommissionsRepository.InsertAsync(cruiseContractCommissions);
        }
        [AbpAuthorize(AppPermissions.Pages_CruiseContractCommissions_Edit)]
        protected virtual async Task Update(CreateOrEditCruiseContractCommissionsDto input)
        {
            var cruiseContractCommissions = await _cruiseContractCommissionsRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, cruiseContractCommissions);
        }
        [AbpAuthorize(AppPermissions.Pages_CruiseContractCommissions_Delete)]
        public async Task Delete(EntityDto input)
        {
            _cruiseContractCommissionsRepository.Delete(input.Id);
        }
       
        [AbpAuthorize(AppPermissions.Pages_CruiseContractCommissions)]
        public async Task<PagedResultDto<CruiseContractCommissionsCruisesLookupTableDto>> GetAllCruisesForLookupTable(GetAllForLookupTableInput input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var query = _lookup_cruisesRepository.GetAll();
                //.WhereIf(
                //       !string.IsNullOrWhiteSpace(input.Filter),
                //      e => e.CruiseName.ToString().Contains(input.Filter)
                //   );


                var totalCount = await query.CountAsync();

                var cruisesList = await query
                    .PageBy(input)
                    .ToListAsync();
                var filteredCruiseContractCommissions = _cruiseContractCommissionsRepository.GetAll().
                                                      Where(x => x.CruiseContractId == input.ContractId && x.TenantId == AbpSession.TenantId).ToList();
                var lookupTableDtoList = new List<CruiseContractCommissionsCruisesLookupTableDto>();
                foreach (var cruises in cruisesList)
                {
                    var _lookupItenarry = await _lookup_cruiseItinerariesRepository.FirstOrDefaultAsync((int)cruises.CruiseItinerariesId);
                    var findFromList = filteredCruiseContractCommissions.Where(x => x.CruisesId == cruises.Id).FirstOrDefault();
                    if (findFromList == null)
                    {
                        lookupTableDtoList.Add(new CruiseContractCommissionsCruisesLookupTableDto
                        {
                            Id = cruises.Id,
                            DisplayName = _lookupItenarry.ItineraryName.ToString()
                        });
                    }
                }
                if (lookupTableDtoList.Count() == 0)
                {
                    totalCount = 0;
                }
                return new PagedResultDto<CruiseContractCommissionsCruisesLookupTableDto>(
                    totalCount,
                    lookupTableDtoList
                );
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseContractCommissions)]
        public async Task<PagedResultDto<CruiseContractCommissionsCruiseShipsLookupTableDto>> GetAllCruiseShipsForLookupTable(GetAllForLookupTableInput input)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _lookup_cruiseShipsRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.CruiseShipName.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var cruiseShipsList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CruiseContractCommissionsCruiseShipsLookupTableDto>();
            var filteredCruiseContractCommissions = _cruiseContractCommissionsRepository.GetAll().
                                               Where(x => x.CruiseContractId == input.ContractId && x.TenantId == AbpSession.TenantId).ToList();


            foreach (var cruiseShips in cruiseShipsList)
            {
                var findFromList = filteredCruiseContractCommissions.Where(x => x.CruiseShipsId == cruiseShips.Id).FirstOrDefault();
                if (findFromList == null)
                {
                    lookupTableDtoList.Add(new CruiseContractCommissionsCruiseShipsLookupTableDto
                    {
                        Id = cruiseShips.Id,
                        DisplayName = cruiseShips.CruiseShipName?.ToString()
                    });
                }
            }

            return new PagedResultDto<CruiseContractCommissionsCruiseShipsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseContractCommissions)]
        public async Task<PagedResultDto<CruiseContractCommissionsCruiseContractLookupTableDto>> GetAllCruiseContractForLookupTable(GetAllForLookupTableInput input)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            var query = _lookup_cruiseContractRepository.GetAll();
            //.WhereIf(
            //       !string.IsNullOrWhiteSpace(input.Filter),
            //      e => e.ContractName.ToString().Contains(input.Filter)
            //   )


            var totalCount = await query.CountAsync();

            var cruiseContractList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CruiseContractCommissionsCruiseContractLookupTableDto>();
            foreach (var cruiseContract in cruiseContractList)
            {
                lookupTableDtoList.Add(new CruiseContractCommissionsCruiseContractLookupTableDto
                {
                    Id = cruiseContract.Id,
                    DisplayName = cruiseContract.Season?.ToString()
                });
            }

            return new PagedResultDto<CruiseContractCommissionsCruiseContractLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseContractCommissions)]
        public async Task<PagedResultDto<CruiseContractCommissionsCruiseServicesLookupTableDto>> GetAllCruiseServicesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_mainServicesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.ServiceName.ToString().Contains(input.Filter)
               ).Where(x => x.IsMainService == true);

            var totalCount = await query.CountAsync();

            var cruiseServicesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CruiseContractCommissionsCruiseServicesLookupTableDto>();


            var filteredCruiseContractCommissions = _cruiseContractCommissionsRepository.GetAll().
                                                    Where(x => x.CruiseContractId == input.ContractId && x.TenantId == AbpSession.TenantId).ToList();


            foreach (var cruiseServices in cruiseServicesList)
            {

                var findFromList = filteredCruiseContractCommissions.Where(x => x.CruiseServicesId == cruiseServices.Id).FirstOrDefault();
                if (findFromList == null)
                {
                    lookupTableDtoList.Add(new CruiseContractCommissionsCruiseServicesLookupTableDto
                    {
                        Id = cruiseServices.Id,
                        DisplayName = cruiseServices.ServiceName?.ToString()
                    });
                }
            }
            if (lookupTableDtoList.Count() == 0)
            {
                totalCount = 0;
            }


            return new PagedResultDto<CruiseContractCommissionsCruiseServicesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}
