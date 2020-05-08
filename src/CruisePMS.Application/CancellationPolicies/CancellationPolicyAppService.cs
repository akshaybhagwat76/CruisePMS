using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using CruisePMS.Authorization;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CruisePMS.CruiseServices;
using Abp.Domain.Uow;
using CruisePMS.CancellationPolicies.Dtos;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using CruisePMS.Configuration;
using Abp.Runtime.Session;
using Abp.Linq.Extensions;
using CruisePMS.CruiseMasterAmenities;
using CruisePMS.Common.Dto;

namespace CruisePMS.CancellationPolicies
{
    [AbpAuthorize(AppPermissions.Pages_CancellationPolicy)]
    public class CancellationPolicyAppService : CruisePMSAppServiceBase, ICancellationPolicyAppService
    {
        private readonly IRepository<CancellationPolicy> _cancellationPolicyRepository;
        private readonly IRepository<CruiseService, int> _lookup_cruiseServicesRepository;
        private readonly IRepository<MasterAmenities, int> _lookup_MasterAmenitiesRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;


        public CancellationPolicyAppService(IRepository<CancellationPolicy> cancellationPolicyRepository, IRepository<CruiseService, int> lookup_cruiseServicesRepository, IRepository<MasterAmenities, int> lookup_MasterAmenitiesRepository, IUnitOfWorkManager unitOfWork)
        {
            _cancellationPolicyRepository = cancellationPolicyRepository;
            _lookup_cruiseServicesRepository = lookup_cruiseServicesRepository;
            _lookup_MasterAmenitiesRepository = lookup_MasterAmenitiesRepository;
            _unitOfWorkManager = unitOfWork;
        }


        public async Task<PagedResultDto<GetCancellationPolicyForViewDto>> GetAll(GetAllCancellationPolicyInput input)
        {
            int workingYear = DateTime.Now.Year;
            bool showNextYear = false;
            bool showHistoryData = false;

            try
            {
                var listSettings = SettingManager.GetAllSettingValuesForUserAsync(AbpSession.ToUserIdentifier()).Result;
                if (AbpSession.TenantId != null)
                {
                    listSettings = SettingManager.GetAllSettingValuesForTenantAsync(AbpSession.ToUserIdentifier().TenantId.Value).Result;
                }

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

            IQueryable<CancellationPolicy> filteredCancellationPolicy = _cancellationPolicyRepository.GetAll()
                        .Include(e => e.CruiseServicesFk)
                        .Include(e => e.CancellationPreTeFk)
                        .Include(e => e.CancellationPostTeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CancellationPercentage.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CancellationPercentageFilter), e => e.CancellationPercentage.ToLower() == input.CancellationPercentageFilter.ToLower().Trim())
                        .WhereIf(input.MinDaysFromFilter != null, e => e.DaysFrom >= input.MinDaysFromFilter)
                        .WhereIf(input.MaxDaysFromFilter != null, e => e.DaysFrom <= input.MaxDaysFromFilter)
                        .WhereIf(input.MinDaysToFilter != null, e => e.DaysTo >= input.MinDaysToFilter)
                        .WhereIf(input.MaxDaysToFilter != null, e => e.DaysTo <= input.MaxDaysToFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruiseServicesServiceNameFilter), e => e.CruiseServicesFk != null && e.CruiseServicesFk.ServiceNaFk.DisplayName.ToLower() == input.CruiseServicesServiceNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruiseMasterAmenitiesDisplayNameFilter), e => e.CancellationPreTeFk != null && e.CancellationPreTeFk.DisplayName.ToLower() == input.CruiseMasterAmenitiesDisplayNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruiseMasterAmenitiesDisplayName2Filter), e => e.CancellationPostTeFk != null && e.CancellationPostTeFk.DisplayName.ToLower() == input.CruiseMasterAmenitiesDisplayName2Filter.ToLower().Trim())
                        .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == false, e => (e.SeasonYear == workingYear || e.SeasonYear == workingYear + 1))
                        .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == true, e => (e.SeasonYear == workingYear || e.SeasonYear == workingYear + 1 || e.SeasonYear <= workingYear))
                        .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == true, e => (e.SeasonYear == workingYear || e.SeasonYear <= workingYear))
                        .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == false, e => e.SeasonYear == workingYear);

            IQueryable<CancellationPolicy> pagedAndFilteredCancellationPolicy = filteredCancellationPolicy
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            IQueryable<GetCancellationPolicyForViewDto> cancellationPolicy = from o in pagedAndFilteredCancellationPolicy
                                                                             join o1 in _lookup_cruiseServicesRepository.GetAll() on o.CruiseServicesId equals o1.Id into j1
                                                                             from s1 in j1.DefaultIfEmpty()

                                                                             join o2 in _lookup_MasterAmenitiesRepository.GetAll() on o.CancellationPreText equals o2.Id into j2
                                                                             from s2 in j2.DefaultIfEmpty()

                                                                             join o3 in _lookup_MasterAmenitiesRepository.GetAll() on o.CancellationPostText equals o3.Id into j3
                                                                             from s3 in j3.DefaultIfEmpty()

                                                                             orderby o.DaysFrom descending

                                                                             select new GetCancellationPolicyForViewDto()
                                                                             {
                                                                                 CancellationPolicy = new CancellationPolicyDto
                                                                                 {
                                                                                     CancellationPercentage = o.CancellationPercentage,
                                                                                     DaysFrom = o.DaysFrom,
                                                                                     DaysTo = o.DaysTo,
                                                                                     SeasonYear = o.SeasonYear,
                                                                                     Id = o.Id
                                                                                 },
                                                                                 CruiseServicesServiceName = s1 == null ? "" : s1.ServiceNaFk.DisplayName.ToString(),
                                                                                 CruiseMasterAmenitiesDisplayName = s2 == null ? "" : s2.DisplayName.ToString(),
                                                                                 CruiseMasterAmenitiesDisplayName2 = GetFormattedCancellationStatusAsync(s2 == null ? "" : s2.DisplayName.ToString(), o.CancellationPercentage, o.DaysFrom.ToString(), o.DaysTo.ToString(), "").Result
                                                                             };

            int totalCount = await filteredCancellationPolicy.CountAsync();

            return new PagedResultDto<GetCancellationPolicyForViewDto>(
                totalCount,
                await cancellationPolicy.ToListAsync()
            );
        }

        public async Task<string> GetFormattedCancellationStatusAsync(string main_string, string arg1, string arg2, string arg3, string arg4)
        {
            try
            {
                var StandardDeposit = await SettingManager.GetSettingValueForTenantAsync(AppSettings.TenantManagement.StandardDeposit, AbpSession.GetTenantId());
                var StandardDepositType = await SettingManager.GetSettingValueForTenantAsync(AppSettings.TenantManagement.StandardDepositType, AbpSession.GetTenantId());

                var defaultCurrency = await SettingManager.GetSettingValueForTenantAsync(AppSettings.DefaultCurrency, AbpSession.GetTenantId());

                switch (StandardDepositType)
                {
                    case "0":
                        arg4 = StandardDeposit + " " + defaultCurrency + " ";
                        break;
                    case "1":
                        arg4 = StandardDeposit + "%";
                        break;
                }
            }
            catch (Exception ex)
            {

            }

            return string.Format(main_string, arg1 + "%", arg2, arg3, arg4);
        }

        public async Task<GetCancellationPolicyForViewDto> GetCancellationPolicyForView(int id)
        {
            CancellationPolicy cancellationPolicy = await _cancellationPolicyRepository.GetAsync(id);

            GetCancellationPolicyForViewDto output = new GetCancellationPolicyForViewDto { CancellationPolicy = ObjectMapper.Map<CancellationPolicyDto>(cancellationPolicy) };

            if (output.CancellationPolicy.CruiseServicesId != null)
            {
                CruiseService _lookupCruiseServices = await _lookup_cruiseServicesRepository.FirstOrDefaultAsync((int)output.CancellationPolicy.CruiseServicesId);
                output.CruiseServicesServiceName = _lookupCruiseServices.ServiceName.ToString();
            }

            if (output.CancellationPolicy.CancellationPreText != null)
            {
                MasterAmenities _lookupMasterAmenities = await _lookup_MasterAmenitiesRepository.FirstOrDefaultAsync((int)output.CancellationPolicy.CancellationPreText);
                output.CruiseMasterAmenitiesDisplayName = _lookupMasterAmenities.DisplayName.ToString();
            }

            if (output.CancellationPolicy.CancellationPostText != null)
            {
                MasterAmenities _lookupMasterAmenities = await _lookup_MasterAmenitiesRepository.FirstOrDefaultAsync((int)output.CancellationPolicy.CancellationPostText);
                output.CruiseMasterAmenitiesDisplayName2 = _lookupMasterAmenities.DisplayName.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CancellationPolicy_Edit)]
        public async Task<GetCancellationPolicyForEditOutput> GetCancellationPolicyForEdit(EntityDto input)
        {
            CancellationPolicy cancellationPolicy = await _cancellationPolicyRepository.FirstOrDefaultAsync(input.Id);

            GetCancellationPolicyForEditOutput output = new GetCancellationPolicyForEditOutput { CancellationPolicy = ObjectMapper.Map<CreateOrEditCancellationPolicyDto>(cancellationPolicy) };

            if (output.CancellationPolicy.CruiseServicesId != null)
            {
                CruiseService _lookupCruiseServices = await _lookup_cruiseServicesRepository.FirstOrDefaultAsync((int)output.CancellationPolicy.CruiseServicesId);
                output.CruiseServicesServiceName = _lookupCruiseServices.ServiceName.ToString();
            }

            if (output.CancellationPolicy.CancellationPreText != null)
            {
                MasterAmenities _lookupMasterAmenities = await _lookup_MasterAmenitiesRepository.FirstOrDefaultAsync((int)output.CancellationPolicy.CancellationPreText);
                output.CruiseMasterAmenitiesDisplayName = _lookupMasterAmenities.DisplayName.ToString();
            }

            if (output.CancellationPolicy.CancellationPostText != null)
            {
                MasterAmenities _lookupMasterAmenities = await _lookup_MasterAmenitiesRepository.FirstOrDefaultAsync((int)output.CancellationPolicy.CancellationPostText);
                output.CruiseMasterAmenitiesDisplayName2 = _lookupMasterAmenities.DisplayName.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCancellationPolicyDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CancellationPolicy_Create)]
        private async Task Create(CreateOrEditCancellationPolicyDto input)
        {
            CancellationPolicy cancellationPolicy = ObjectMapper.Map<CancellationPolicy>(input);


            if (AbpSession.TenantId != null)
            {
                cancellationPolicy.TenantId = AbpSession.TenantId;
            }


            await _cancellationPolicyRepository.InsertAsync(cancellationPolicy);
        }

        [AbpAuthorize(AppPermissions.Pages_CancellationPolicy_Edit)]
        private async Task Update(CreateOrEditCancellationPolicyDto input)
        {
            CancellationPolicy cancellationPolicy = await _cancellationPolicyRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, cancellationPolicy);
        }

        [AbpAuthorize(AppPermissions.Pages_CancellationPolicy_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _cancellationPolicyRepository.DeleteAsync(input.Id);
        }


        [AbpAuthorize(AppPermissions.Pages_CancellationPolicy)]
        public async Task<PagedResultDto<CancellationPolicyCruiseServicesLookupTableDto>> GetAllCruiseServicesForLookupTable(GetAllForLookupTableInput input)
        {
            //CurrentUnitOfWork.SetFilterParameter(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, null);

            IQueryable<CruiseService> query = _lookup_cruiseServicesRepository.GetAll()
                .Include(p => p.ServiceNaFk)
                .WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.ServiceName.ToString().Contains(input.Filter)
                );

            int totalCount = await query.CountAsync();

            List<CruiseService> cruiseServicesList = await query
                .PageBy(input)
                .ToListAsync();

            List<CancellationPolicyCruiseServicesLookupTableDto> lookupTableDtoList = new List<CancellationPolicyCruiseServicesLookupTableDto>();
            foreach (CruiseService cruiseServices in cruiseServicesList)
            {
                lookupTableDtoList.Add(new CancellationPolicyCruiseServicesLookupTableDto
                {
                    Id = cruiseServices.Id,
                    DisplayName = cruiseServices.ServiceNaFk.DisplayName
                });
            }

            return new PagedResultDto<CancellationPolicyCruiseServicesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CancellationPolicy)]
        public async Task<PagedResultDto<CancellationPolicyCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(GetAllForLookupTableInput input)
        {
            IQueryable<MasterAmenities> query = _lookup_MasterAmenitiesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.DisplayName.ToString().Contains(input.Filter)
                ).WhereIf(true, e => e.ParentId == 18);

            int totalCount = await query.CountAsync();

            List<MasterAmenities> MasterAmenitiesList = await query
                .PageBy(input)
                .ToListAsync();

            List<CancellationPolicyCruiseMasterAmenitiesLookupTableDto> lookupTableDtoList = new List<CancellationPolicyCruiseMasterAmenitiesLookupTableDto>();
            foreach (MasterAmenities MasterAmenities in MasterAmenitiesList)
            {
                lookupTableDtoList.Add(new CancellationPolicyCruiseMasterAmenitiesLookupTableDto
                {
                    Id = MasterAmenities.Id,
                    DisplayName = MasterAmenities.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<CancellationPolicyCruiseMasterAmenitiesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}
