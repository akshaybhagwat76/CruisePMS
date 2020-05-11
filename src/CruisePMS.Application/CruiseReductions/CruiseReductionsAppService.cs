using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using CruisePMS.AgePolicies;
using CruisePMS.Authorization;
using CruisePMS.Common.Dto;
using CruisePMS.Configuration;
using CruisePMS.CruiseMasterAmenities;
using CruisePMS.CruiseReductions.Dtos;
using CruisePMS.CruiseServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace CruisePMS.CruiseReductions
{
    [AbpAuthorize(AppPermissions.Pages_CruiseReductions)]
    public class CruiseReductionsAppService : CruisePMSAppServiceBase, ICruiseReductionsAppService
    {
        private readonly IRepository<CruiseReduction> _cruiseReductionsRepository;
        private readonly IRepository<CruiseService, int> _lookup_cruiseServicesRepository;
        private readonly IRepository<AgePolicy, long> _lookup_agePoliciesRepository;
        private readonly IRepository<MasterAmenities> _cruiseMasterAmenitiesRepository;
        private readonly IRepository<AgePolicy, long> _agePoliciesRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public CruiseReductionsAppService(IRepository<AgePolicy, long> agePoliciesRepository, IUnitOfWorkManager unitOfWork, IRepository<MasterAmenities> cruiseMasterAmenitiesRepository, IRepository<CruiseReduction> cruiseReductionsRepository, IRepository<CruiseService, int> lookup_cruiseServicesRepository, IRepository<AgePolicy, long> lookup_agePoliciesRepository)
        {
            _cruiseReductionsRepository = cruiseReductionsRepository;
            _lookup_cruiseServicesRepository = lookup_cruiseServicesRepository;
            _lookup_agePoliciesRepository = lookup_agePoliciesRepository;
            _cruiseMasterAmenitiesRepository = cruiseMasterAmenitiesRepository;
            _agePoliciesRepository = agePoliciesRepository;
            _unitOfWorkManager = unitOfWork;
        }
        public async Task<PagedResultDto<GetCruiseReductionsForViewDto>> GetAll(GetAllCruiseReductionsInput input)
        {
            IQueryable<CruiseReduction> filteredCruiseReductions = _cruiseReductionsRepository.GetAll()
                        .Include(e => e.ServicesFk)
                        .Include(e => e.AgePoliciesFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruiseServicesServiceNameFilter), e => e.ServicesFk != null && e.ServicesFk.ServiceNaFk.DisplayName.ToLower() == input.CruiseServicesServiceNameFilter.ToLower().Trim());

            IQueryable<CruiseReduction> pagedAndFilteredCruiseReductions = filteredCruiseReductions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            IQueryable<GetCruiseReductionsForViewDto> cruiseReductions = from o in pagedAndFilteredCruiseReductions
                                                                         join o1 in _lookup_cruiseServicesRepository.GetAll() on o.ServicesId equals o1.Id into j1
                                                                         from s1 in j1.DefaultIfEmpty()

                                                                         join o2 in _lookup_agePoliciesRepository.GetAll() on o.AgePoliciesId equals o2.Id into j2
                                                                         from s2 in j2.DefaultIfEmpty()

                                                                         join o3 in _cruiseMasterAmenitiesRepository.GetAll() on s1.ServiceName equals o3.Id into j3
                                                                         from s3 in j3.DefaultIfEmpty()

                                                                         join o4 in _cruiseMasterAmenitiesRepository.GetAll() on o.PassangerNoName equals o4.Id into j4
                                                                         from s4 in j4.DefaultIfEmpty()

                                                                         join o5 in _cruiseMasterAmenitiesRepository.GetAll() on s2.GuestType equals o5.Id into j5
                                                                         from s5 in j5.DefaultIfEmpty()

                                                                         join o6 in _lookup_cruiseServicesRepository.GetAll() on o.ReductionName equals o6.Id into j6
                                                                         from s6 in j6.DefaultIfEmpty()

                                                                         join o7 in _cruiseMasterAmenitiesRepository.GetAll() on s6.ServiceName equals o7.Id into j7
                                                                         from s7 in j7.DefaultIfEmpty()

                                                                         select new GetCruiseReductionsForViewDto()
                                                                         {
                                                                             CruiseReductions = new CruiseReductionsDto
                                                                             {
                                                                                 ReductionAmount = o.ReductionAmount,
                                                                                 ReductionType = o.ReductionType,
                                                                                 ActivateOn = o.ActivateOn,
                                                                                 Id = o.Id
                                                                             },
                                                                             CruiseServicesServiceName = s3 == null ? "" : s3.DisplayName.ToString(),
                                                                             PassengerNoName = s4 == null ? "" : s4.DisplayName.ToString(),
                                                                             AgePolicy = s5 == null ? "" : s5.DisplayName.ToString(),
                                                                             ReductionName = s7 == null ? "" : s7.DisplayName.ToString(),
                                                                             AgePoliciesTenantId = s2 == null ? "" : s2.TenantId.ToString()
                                                                         };

            int totalCount = await filteredCruiseReductions.CountAsync();

            return new PagedResultDto<GetCruiseReductionsForViewDto>(
                totalCount,
                await cruiseReductions.ToListAsync()
            );
        }
        public async Task<GetCruiseReductionsForViewDto> GetCruiseReductionsForView(int id)
        {
            CruiseReduction cruiseReductions = await _cruiseReductionsRepository.GetAsync(id);

            GetCruiseReductionsForViewDto output = new GetCruiseReductionsForViewDto { CruiseReductions = ObjectMapper.Map<CruiseReductionsDto>(cruiseReductions) };

            if (output.CruiseReductions.ServicesId != null)
            {
                CruiseService _lookupCruiseServices = await _lookup_cruiseServicesRepository.FirstOrDefaultAsync(output.CruiseReductions.ServicesId);
                output.CruiseServicesServiceName = _lookupCruiseServices.ServiceName.ToString();
            }

            if (output.CruiseReductions.AgePoliciesId != null)
            {
                AgePolicy _lookupAgePolicies = await _lookup_agePoliciesRepository.FirstOrDefaultAsync(output.CruiseReductions.AgePoliciesId);
                output.AgePoliciesTenantId = _lookupAgePolicies.TenantId.ToString();
            }

            return output;
        }
        [AbpAuthorize(AppPermissions.Pages_CruiseReductions_Edit)]
        public async Task<GetCruiseReductionsForEditOutput> GetCruiseReductionsForEdit(EntityDto input)
        {
            CruiseReduction cruiseReductions = await _cruiseReductionsRepository.FirstOrDefaultAsync(input.Id);

            GetCruiseReductionsForEditOutput output = new GetCruiseReductionsForEditOutput { CruiseReductions = ObjectMapper.Map<CreateOrEditCruiseReductionsDto>(cruiseReductions) };

            if (output.CruiseReductions.ServicesId != null)
            {
                CruiseService _lookupCruiseServices = await _lookup_cruiseServicesRepository.FirstOrDefaultAsync(output.CruiseReductions.ServicesId);
                output.CruiseServicesServiceName = _lookupCruiseServices.ServiceName.ToString();
            }

            if (output.CruiseReductions.AgePoliciesId != null)
            {
                AgePolicy _lookupAgePolicies = await _lookup_agePoliciesRepository.FirstOrDefaultAsync(output.CruiseReductions.AgePoliciesId);
                output.AgePoliciesTenantId = _lookupAgePolicies.TenantId.ToString();
            }

            return output;
        }
        public async Task CreateOrEdit(CreateOrEditCruiseReductionsDto input)
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
        [AbpAuthorize(AppPermissions.Pages_CruiseReductions_Create)]
        private async Task Create(CreateOrEditCruiseReductionsDto input)
        {
            CruiseReduction cruiseReductions = ObjectMapper.Map<CruiseReduction>(input);


            if (AbpSession.TenantId != null)
            {
                cruiseReductions.TenantId = AbpSession.TenantId;
            }


            await _cruiseReductionsRepository.InsertAsync(cruiseReductions);
        }
        [AbpAuthorize(AppPermissions.Pages_CruiseReductions_Edit)]
        private async Task Update(CreateOrEditCruiseReductionsDto input)
        {
            CruiseReduction cruiseReductions = await _cruiseReductionsRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, cruiseReductions);
        }
        [AbpAuthorize(AppPermissions.Pages_CruiseReductions_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _cruiseReductionsRepository.DeleteAsync(input.Id);
        }
        
        [AbpAuthorize(AppPermissions.Pages_CruiseReductions)]
        public async Task<PagedResultDto<CruiseReductionsCruiseServicesLookupTableDto>> GetAllCruiseServicesForLookupTable(GetAllForLookupTableInput input)
        {
            IQueryable<CruiseService> query = _lookup_cruiseServicesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.ServiceName.ToString().Contains(input.Filter)
                );

            int totalCount = await query.CountAsync();

            List<CruiseService> cruiseServicesList = await query
                .PageBy(input)
                .ToListAsync();

            List<CruiseReductionsCruiseServicesLookupTableDto> lookupTableDtoList = new List<CruiseReductionsCruiseServicesLookupTableDto>();
            foreach (CruiseService cruiseServices in cruiseServicesList)
            {
                lookupTableDtoList.Add(new CruiseReductionsCruiseServicesLookupTableDto
                {
                    Id = cruiseServices.Id,
                    DisplayName = cruiseServices.ServiceName?.ToString()
                });
            }

            return new PagedResultDto<CruiseReductionsCruiseServicesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_CruiseReductions)]
        public async Task<PagedResultDto<CruiseReductionsAgePoliciesLookupTableDto>> GetAllAgePoliciesForLookupTable(GetAllForLookupTableInput input)
        {
            IQueryable<AgePolicy> query = _lookup_agePoliciesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.TenantId.ToString().Contains(input.Filter)
                );

            int totalCount = await query.CountAsync();

            List<AgePolicy> agePoliciesList = await query
                .PageBy(input)
                .ToListAsync();

            List<CruiseReductionsAgePoliciesLookupTableDto> lookupTableDtoList = new List<CruiseReductionsAgePoliciesLookupTableDto>();
            foreach (AgePolicy agePolicies in agePoliciesList)
            {
                lookupTableDtoList.Add(new CruiseReductionsAgePoliciesLookupTableDto
                {
                    Id = agePolicies.Id,
                    DisplayName = agePolicies.TenantId?.ToString()
                });
            }

            return new PagedResultDto<CruiseReductionsAgePoliciesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseReductions_Create)]
        public GetReductionsScreenDto GetAllReductions()
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);

            GetReductionsScreenDto getReductionsScreenDto = new GetReductionsScreenDto();

            string defaultCurrentLanguage = SettingManager.GetSettingValueForUserAsync(AppSettings.DefaultCurrentLanguage, AbpSession.ToUserIdentifier()).Result;
            if (string.IsNullOrWhiteSpace(defaultCurrentLanguage))
            { defaultCurrentLanguage = "EN"; }
            var GetReductionServiceGroup = (from o in _cruiseMasterAmenitiesRepository.GetAll()
                                            join o1 in _lookup_cruiseServicesRepository.GetAll() on o.Id equals o1.ServiceName into j1
                                            from s1 in j1.DefaultIfEmpty()
                                            select new SelectReductionServices
                                            {
                                                DisplayName = o.DisplayName,
                                                Id = s1.Id,
                                                ReductionCanBeApplied = s1.ReductionCanBeApplied,
                                                Lang = o.Lang
                                            }).Where(x => x.Lang.ToUpper() == defaultCurrentLanguage.ToUpper() && x.ReductionCanBeApplied == true);

            var Reduction = (from o in _cruiseMasterAmenitiesRepository.GetAll()
                             join o1 in _lookup_cruiseServicesRepository.GetAll() on o.Id equals o1.ServiceName into j1
                             from s1 in j1.DefaultIfEmpty()
                             select new GetReductionName
                             {
                                 DisplayName = o.DisplayName,
                                 Id = s1.Id,
                                 Lang = o.Lang

                             }).Where(x => x.Lang.ToUpper() == defaultCurrentLanguage.ToUpper() && x.Id == 71).FirstOrDefault();

            var AgePlicy = _agePoliciesRepository.GetAll().Where(x => x.TenantId == null).OrderByDescending(x => x.AgeFrom);
            int? tenantId = null;
            if (AbpSession.TenantId != null)
            {
                tenantId = AbpSession.TenantId;
                AgePlicy = _agePoliciesRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).OrderByDescending(x => x.AgeFrom);
            }

            var passengerNumbers = _cruiseMasterAmenitiesRepository.GetAll().Where(x => x.ParentId == 200 && x.Lang.ToUpper() == defaultCurrentLanguage.ToUpper()).ToList();

            // Add one passenger 
            var GetFirstpassenger = passengerNumbers.First();
            foreach (var item in AgePlicy)
            {
                var guestType = _cruiseMasterAmenitiesRepository.GetAll().Where(x => x.Id == item.GuestType).FirstOrDefault();

                ReductionsScreen reductionsScreen = new ReductionsScreen();
                reductionsScreen.Id = 0;
                reductionsScreen.PassangerDisplayName = GetFirstpassenger.DisplayName;
                reductionsScreen.PassangerNo = GetFirstpassenger.Id;
                reductionsScreen.ReductionId = Reduction.Id;
                reductionsScreen.ReductionName = Reduction.DisplayName;
                reductionsScreen.ServiceId = 0;
                reductionsScreen.ReductionType = "%";
                reductionsScreen.ServiceId = 0;
                reductionsScreen.Age = guestType.DisplayName + "(" + item.AgeFrom + "+)";

                reductionsScreen.AgePolicyId = item.Id;

                reductionsScreen.AddBlankRow = false;
                foreach (var grpService in GetReductionServiceGroup)
                {
                    ReductionServiceGroup reductionServiceGroup = new ReductionServiceGroup();
                    reductionServiceGroup.Id = grpService.Id;
                    reductionServiceGroup.DisplayName = grpService.DisplayName;
                    reductionsScreen.reductionServiceGroup.Add(reductionServiceGroup);
                }
                getReductionsScreenDto.reductionsScreen.Add(reductionsScreen);
                break;
            }
            passengerNumbers.RemoveAt(0);
            foreach (var pnNo in passengerNumbers)
            {
                int indexOfAge = 0;
                foreach (var item in AgePlicy)
                {
                    var guestType = _cruiseMasterAmenitiesRepository.GetAll().Where(x => x.Id == item.GuestType).FirstOrDefault();

                    ReductionsScreen reductionsScreen = new ReductionsScreen();
                    reductionsScreen.Id = 0;
                    reductionsScreen.PassangerDisplayName = pnNo.DisplayName;
                    reductionsScreen.PassangerNo = pnNo.Id;
                    reductionsScreen.ReductionId = Reduction.Id;
                    reductionsScreen.ReductionName = Reduction.DisplayName;
                    reductionsScreen.ServiceId = 0;
                    reductionsScreen.ReductionType = "%";
                    reductionsScreen.ServiceId = 0;
                    reductionsScreen.AgePolicyId = item.Id;
                    if (indexOfAge == 0)
                    {
                        reductionsScreen.Age = guestType.DisplayName + "(+" + item.AgeFrom + ")";
                        reductionsScreen.AddBlankRow = true;
                    }
                    else
                    {
                        reductionsScreen.Age = guestType.DisplayName + "(" + item.AgeFrom + "-" + item.AgeTo + ")";

                    }
                    indexOfAge++;
                    foreach (var grpService in GetReductionServiceGroup)
                    {
                        ReductionServiceGroup reductionServiceGroup = new ReductionServiceGroup();
                        reductionServiceGroup.Id = grpService.Id;
                        reductionServiceGroup.DisplayName = grpService.DisplayName;
                        reductionsScreen.reductionServiceGroup.Add(reductionServiceGroup);
                    }
                    getReductionsScreenDto.reductionsScreen.Add(reductionsScreen);

                }
            }


            return getReductionsScreenDto;
        }

        public async Task SaveReductions(List<ReductionsScreen> reductionsScreen)
        {
            int ActivateOn = 1;
            foreach (var item in reductionsScreen)
            {

                if (item.Id > 0)
                {
                    CruiseReduction cruiseReductions = await _cruiseReductionsRepository.FirstOrDefaultAsync((int)item.Id);
                    cruiseReductions.PassangerNoName = item.PassangerNo;
                    cruiseReductions.ReductionName = item.ReductionId;
                    cruiseReductions.ReductionAmount = item.ReductionAmount;
                    cruiseReductions.ReductionType = item.ReductionType;
                    cruiseReductions.ActivateOn = 1;
                    cruiseReductions.ServicesId = item.ServiceId;
                    cruiseReductions.AgePoliciesId = item.AgePolicyId;
                    if (AbpSession.TenantId != null)
                    {
                        cruiseReductions.TenantId = (int?)AbpSession.TenantId;
                    }
                    await _cruiseReductionsRepository.UpdateAsync(cruiseReductions);

                    // Update 
                }
                else
                {
                    if (item.AddBlankRow)
                    {
                        ActivateOn++;
                    }

                    CruiseReduction cruiseReductions = new CruiseReduction();
                    cruiseReductions.PassangerNoName = item.PassangerNo;
                    cruiseReductions.ReductionName = item.ReductionId;
                    cruiseReductions.ReductionAmount = item.ReductionAmount;
                    cruiseReductions.ReductionType = item.ReductionType;
                    cruiseReductions.ActivateOn = ActivateOn;
                    cruiseReductions.ServicesId = item.ServiceId;
                    if (AbpSession.TenantId != null)
                    {
                        cruiseReductions.TenantId = (int?)AbpSession.TenantId;
                    }
                    cruiseReductions.AgePoliciesId = item.AgePolicyId;
                    await _cruiseReductionsRepository.InsertAsync(cruiseReductions);
                    // save 
                }
            }
        }
    }
}
