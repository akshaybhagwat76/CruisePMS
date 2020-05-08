using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using CruisePMS.Authorization;
using CruisePMS.Authorization.Users;
using CruisePMS.Common;
using CruisePMS.Configuration;
using System.Linq.Dynamic.Core;
using CruisePMS.Contracts.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruisePMS.Contracts
{
    [AbpAuthorize(AppPermissions.Pages_CruiseContract)]
    public class ContractAppService : CruisePMSAppServiceBase, IContractAppService
    {
        private readonly IRepository<Contract> _cruiseContractRepository;
        private readonly UserManager _userManager;
        private readonly IRepository<ReadSettings, long> _readSettingsRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;


        public ContractAppService(UserManager userManager, IRepository<ReadSettings, long> readSettingsRepository, IRepository<Contract> cruiseContractRepository, IUnitOfWorkManager unitOfWork)
        {
            _cruiseContractRepository = cruiseContractRepository;
            _readSettingsRepository = readSettingsRepository;
            _userManager = userManager;
            _unitOfWorkManager = unitOfWork;

        }

        public async Task<PagedResultDto<GetCruiseContractForViewDto>> GetAll(GetAllCruiseContractInput input)
        {
            int workingYear = DateTime.Now.Year;
            bool showNextYear = false;
            bool showHistoryData = false;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {

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


                var filteredCruiseContract = _cruiseContractRepository.GetAll()
                        .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == false, e => (Convert.ToInt32(e.Season) == workingYear || Convert.ToInt32(e.Season) == workingYear + 1))
                        .WhereIf(workingYear > 0 && showNextYear == true && showHistoryData == true, e => (Convert.ToInt32(e.Season) == workingYear || Convert.ToInt32(e.Season) == workingYear + 1 || Convert.ToInt32(e.Season) <= workingYear))
                        .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == true, e => (Convert.ToInt32(e.Season) == workingYear || Convert.ToInt32(e.Season) <= workingYear))
                        .WhereIf(workingYear > 0 && showNextYear == false && showHistoryData == false, e => Convert.ToInt32(e.Season) == workingYear)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Season.Contains(input.Filter))
                        .Where(x => x.TenantsRecipient == AbpSession.TenantId
                                                                          || x.TenantSupplierId == AbpSession.TenantId
                                                                          || x.TenantId == AbpSession.TenantId);
                var pagedAndFilteredCruiseContract = filteredCruiseContract
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                IQueryable<GetCruiseContractForViewDto> cruiseContract = (from o in pagedAndFilteredCruiseContract.Where(x => x.TenantsRecipient == AbpSession.TenantId
                                                                          || x.TenantSupplierId == AbpSession.TenantId
                                                                          || x.TenantId == AbpSession.TenantId)
                                                                          select new GetCruiseContractForViewDto()
                                                                          {
                                                                              CruiseContract = new CruiseContractDto
                                                                              {
                                                                                  ContractDate = o.ContractDate,
                                                                                  ContractDisable = o.ContractDisable,
                                                                                  UserId = o.UserId,
                                                                                  TenantsRecipient = o.TenantsRecipient,
                                                                                  ContractDcoId = o.ContractDcoId,
                                                                                  Season = o.Season,
                                                                                  contractCreatedate = o.contractCreatedate,
                                                                                  TenantSupplierId = o.TenantSupplierId,
                                                                                  SuplierUserId = o.SuplierUserId,
                                                                                  ConfirmDate = o.ConfirmDate,
                                                                                  ContractEnable = o.ContractEnable,
                                                                                  Id = o.Id,
                                                                              }
                                                                          });

                if (input.TravelAgentsTenantId > 0)
                {
                    cruiseContract = cruiseContract.Where(x => x.CruiseContract.TenantsRecipient == input.TravelAgentsTenantId);
                }
                if (input.CruiseOpratorTenantId > 0)
                {
                    cruiseContract = cruiseContract.Where(x => x.CruiseContract.TenantSupplierId == input.CruiseOpratorTenantId);
                }

                List<GetCruiseContractForViewDto> cruiseContracts = new List<GetCruiseContractForViewDto>();
                foreach (var item in cruiseContract)
                {
                    GetCruiseContractForViewDto getCruiseContractForViewDto = new GetCruiseContractForViewDto();
                    CruiseContractDto cruiseContract1 = new CruiseContractDto();
                    if (!string.IsNullOrEmpty(Convert.ToString(item.CruiseContract.TenantsRecipient)))
                    {
                        if (item.CruiseContract.TenantsRecipient > 0)
                        {
                            var RecipientTT = await TenantManager.GetByIdAsync((int)item.CruiseContract.TenantsRecipient);
                            cruiseContract1.TenantRecipient = RecipientTT.Name;
                        }
                        else
                        {
                            cruiseContract1.TenantRecipient = string.Empty;
                        }
                    }
                    else
                    {
                        cruiseContract1.TenantRecipient = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(item.CruiseContract.TenantSupplierId)))
                    {
                        if (item.CruiseContract.TenantSupplierId > 0)
                        {
                            var SupplierTT = await TenantManager.GetByIdAsync((int)item.CruiseContract.TenantSupplierId);
                            cruiseContract1.TenantSupplier = Convert.ToString(SupplierTT.Name);
                        }
                        else
                        {
                            cruiseContract1.TenantSupplier = string.Empty;
                        }
                    }
                    else
                    {
                        cruiseContract1.TenantSupplier = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(item.CruiseContract.UserId)))
                    {
                        if (item.CruiseContract.UserId > 0)
                        {
                            var RecipientUser = await _userManager.FindByIdAsync(Convert.ToString(item.CruiseContract.UserId));

                            cruiseContract1.RecipientUserName = Convert.ToString(RecipientUser.FullName);
                        }
                        else
                        {
                            cruiseContract1.RecipientUserName = string.Empty;
                        }
                    }
                    else
                    { cruiseContract1.RecipientUserName = string.Empty; }
                    if (!string.IsNullOrEmpty(Convert.ToString(item.CruiseContract.SuplierUserId)))
                    {
                        if (item.CruiseContract.SuplierUserId > 0)
                        {
                            var suplierName = await _userManager.FindByIdAsync(Convert.ToString(item.CruiseContract.SuplierUserId));

                            cruiseContract1.SuplierUserName = Convert.ToString(suplierName.FullName);
                        }
                        else { cruiseContract1.SuplierUserName = string.Empty; }
                    }
                    else { cruiseContract1.SuplierUserName = string.Empty; }



                    cruiseContract1.ContractDate = item.CruiseContract.ContractDate;
                    cruiseContract1.ContractDisable = item.CruiseContract.ContractDisable;
                    cruiseContract1.UserId = item.CruiseContract.UserId;
                    cruiseContract1.TenantsRecipient = item.CruiseContract.TenantsRecipient;
                    cruiseContract1.ContractDcoId = item.CruiseContract.ContractDcoId;
                    cruiseContract1.Season = item.CruiseContract.Season;
                    cruiseContract1.contractCreatedate = item.CruiseContract.contractCreatedate;
                    cruiseContract1.TenantSupplierId = item.CruiseContract.TenantSupplierId;
                    cruiseContract1.SuplierUserId = item.CruiseContract.SuplierUserId;
                    cruiseContract1.ConfirmDate = item.CruiseContract.ConfirmDate;
                    cruiseContract1.ContractEnable = item.CruiseContract.ContractEnable;
                    cruiseContract1.Id = item.CruiseContract.Id;
                    getCruiseContractForViewDto.CruiseContract = cruiseContract1;
                    cruiseContracts.Add(getCruiseContractForViewDto);
                }



                var totalCount = await filteredCruiseContract.CountAsync();

                return new PagedResultDto<GetCruiseContractForViewDto>(
                    totalCount,
                    cruiseContracts
                );
            }
        }

        //Bind Tenant Travel Recepient who is logged in 
        public async Task<CreateRecepientTimeRecordsDto> GetRecepientLoggedIn()
        {
            CreateRecepientTimeRecordsDto createRecepientTimeRecords = new CreateRecepientTimeRecordsDto();
            if (AbpSession.TenantId != null)
            {
                var tenant = await TenantManager.GetByIdAsync((int)AbpSession.TenantId);
                var user = await _userManager.FindByIdAsync(Convert.ToString(AbpSession.UserId));
                createRecepientTimeRecords.TenantId = (long)AbpSession.TenantId;
                createRecepientTimeRecords.TenantName = tenant.Name;
                createRecepientTimeRecords.CurrentLoggedInUserName = user.FullName;
                createRecepientTimeRecords.CurrentLoggedInUserId = (long)AbpSession.UserId;


            }

            return createRecepientTimeRecords;
        }
        // fill the Suplier

        public async Task<GetAllCruiseOpratorsTenants> GetAllCruiseOpratorsTenants()
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var workingYear = DateTime.Now.Year;
                var AgentUserSettings = await SettingManager.GetAllSettingValuesForUserAsync(AbpSession.ToUserIdentifier());
                foreach (var Setting in AgentUserSettings.Where(x => x.Value == "App.UserManagement.WorkingYear"))
                {
                    if (Setting.Name == "App.UserManagement.WorkingYear")
                    {
                        workingYear = Convert.ToInt32(Setting.Value);
                    }
                }
                IQueryable<Contract> filteredCruiseContract = _cruiseContractRepository.GetAll()
                    .Where(x => x.Season == Convert.ToString(workingYear) && x.ContractDisable == true && x.ContractEnable == true);


                GetAllCruiseOpratorsTenants getAllCruiseOpratorsTenants = new GetAllCruiseOpratorsTenants();
                var filteredSettings = _readSettingsRepository.GetAll().Where(x => x.Name == "App.TenantManagement.IsCruise");
                List<CruiseOpratorsTenant> cruiseOpratorsList = new List<CruiseOpratorsTenant>();
                foreach (var setting in filteredSettings)
                {
                    if (Convert.ToBoolean(setting.Value))
                    {
                        var findValue = cruiseOpratorsList.Where(x => x.TenantId == setting.TenantId);
                        if (findValue.Count() == 0)
                        {
                            CruiseOpratorsTenant cruiseOpratorsTenant = new CruiseOpratorsTenant();
                            cruiseOpratorsTenant.TenantId = setting.TenantId;
                            cruiseOpratorsTenant.IsCruiseOprator = Convert.ToBoolean(setting.Value);
                            cruiseOpratorsTenant.TenantName = Convert.ToString(setting.TenancyName);
                            cruiseOpratorsList.Add(cruiseOpratorsTenant);
                        }
                    }
                }


                getAllCruiseOpratorsTenants.cruiseOpratorsTenant = new List<CruiseOpratorsTenant>();
                getAllCruiseOpratorsTenants.cruiseOpratorsTenant = cruiseOpratorsList;

                return getAllCruiseOpratorsTenants;
            }
        }


        public IEnumerable<T> Distinct<T>(IEnumerable<T> source)
        {
            List<T> uniques = new List<T>();
            foreach (T item in source)
            {
                if (!uniques.Contains(item)) uniques.Add(item);
            }
            return uniques;
        }

        public async Task<GetAllCruiseOpratorsTenants> GetAllCruiseOpratorsTenantsForOperator()
        {
            GetAllCruiseOpratorsTenants getAllCruiseOpratorsTenants = new GetAllCruiseOpratorsTenants();
            var filteredSettings = _readSettingsRepository.GetAll().Where(x => x.Name == "App.TenantManagement.IsCruise");
            List<CruiseOpratorsTenant> cruiseOpratorsList = new List<CruiseOpratorsTenant>();
            foreach (var setting in filteredSettings)
            {
                if (Convert.ToBoolean(setting.Value))
                {
                    CruiseOpratorsTenant cruiseOpratorsTenant = new CruiseOpratorsTenant();
                    cruiseOpratorsTenant.TenantId = setting.TenantId;
                    cruiseOpratorsTenant.IsCruiseOprator = Convert.ToBoolean(setting.Value);
                    cruiseOpratorsTenant.TenantName = Convert.ToString(setting.TenancyName);
                    cruiseOpratorsList.Add(cruiseOpratorsTenant);
                }

            }
            getAllCruiseOpratorsTenants.cruiseOpratorsTenant = new List<CruiseOpratorsTenant>();
            getAllCruiseOpratorsTenants.cruiseOpratorsTenant = cruiseOpratorsList;

            return getAllCruiseOpratorsTenants;
        }


        public async Task<GetAllCruiseOpratorsTenants> GetAllCruiseOpratorsContract()
        {
            GetAllCruiseOpratorsTenants getAllCruiseOpratorsTenants = new GetAllCruiseOpratorsTenants();
            var filteredSettings = _readSettingsRepository.GetAll().Where(x => x.Name == "App.TenantManagement.IsCruise");
            List<CruiseOpratorsTenant> cruiseOpratorsList = new List<CruiseOpratorsTenant>();
            foreach (var setting in filteredSettings)
            {
                if (Convert.ToBoolean(setting.Value))
                {
                    CruiseOpratorsTenant cruiseOpratorsTenant = new CruiseOpratorsTenant();
                    cruiseOpratorsTenant.TenantId = setting.TenantId;
                    cruiseOpratorsTenant.IsCruiseOprator = Convert.ToBoolean(setting.Value);
                    cruiseOpratorsTenant.TenantName = Convert.ToString(setting.TenancyName);
                    cruiseOpratorsList.Add(cruiseOpratorsTenant);
                }

            }
            getAllCruiseOpratorsTenants.cruiseOpratorsTenant = new List<CruiseOpratorsTenant>();
            getAllCruiseOpratorsTenants.cruiseOpratorsTenant = cruiseOpratorsList;

            return getAllCruiseOpratorsTenants;
        }
        public async Task<GetAllCruiseOpratorsTenants> GetAllTravelAgentsContract()
        {
            GetAllCruiseOpratorsTenants getAllCruiseOpratorsTenants = new GetAllCruiseOpratorsTenants();
            var filteredSettings = _readSettingsRepository.GetAll().Where(x => x.Name == "App.TenantManagement.IsTravelOperator");
            List<CruiseOpratorsTenant> cruiseOpratorsList = new List<CruiseOpratorsTenant>();
            foreach (var setting in filteredSettings)
            {
                if (Convert.ToBoolean(setting.Value))
                {
                    CruiseOpratorsTenant cruiseOpratorsTenant = new CruiseOpratorsTenant();
                    cruiseOpratorsTenant.TenantId = setting.TenantId;
                    cruiseOpratorsTenant.IsCruiseOprator = Convert.ToBoolean(setting.Value);
                    cruiseOpratorsTenant.TenantName = Convert.ToString(setting.TenancyName);
                    cruiseOpratorsList.Add(cruiseOpratorsTenant);
                }

            }
            getAllCruiseOpratorsTenants.cruiseOpratorsTenant = new List<CruiseOpratorsTenant>();
            getAllCruiseOpratorsTenants.cruiseOpratorsTenant = cruiseOpratorsList;

            return getAllCruiseOpratorsTenants;
        }


        public async Task<GetCruiseContractForViewDto> GetCruiseContractForView(int id)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var cruiseContract = await _cruiseContractRepository.GetAsync(id);

                var output = new GetCruiseContractForViewDto { CruiseContract = ObjectMapper.Map<CruiseContractDto>(cruiseContract) };

                return output;
            }
        }
        // .Ignore(record => record.Field)
        //.Ignore(record => record.AnotherField)
        //.Ignore(record => record.Etc);

        [AbpAuthorize(AppPermissions.Pages_CruiseContract_Edit)]
        public async Task<GetCruiseContractForEditOutput> GetCruiseContractForEdit(EntityDto input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var cruiseContract = await _cruiseContractRepository.FirstOrDefaultAsync(input.Id);

                var output = new GetCruiseContractForEditOutput { CruiseContract = ObjectMapper.Map<CreateOrEditCruiseContractDto>(cruiseContract) };
                var CurrentTenant = await TenantManager.GetByIdAsync((int)AbpSession.TenantId);
                var Currentuser = await _userManager.FindByIdAsync(Convert.ToString(AbpSession.UserId));

                if (output.CruiseContract.SuplierUserId == 0 && output.CruiseContract.UserId > 0)
                {
                    output.CruiseContract.supplierName = Currentuser.FullName;
                    //output.CruiseContract.TenantSupplierId = CurrentTenant.Id;
                    output.CruiseContract.SuplierUserId = Currentuser.Id;
                }
                if (output.CruiseContract.SuplierUserId > 0 && output.CruiseContract.UserId > 0)
                {
                    CurrentTenant = await TenantManager.GetByIdAsync((int)output.CruiseContract.TenantSupplierId);
                    Currentuser = await _userManager.FindByIdAsync(Convert.ToString(output.CruiseContract.SuplierUserId));

                    output.CruiseContract.supplierName = Currentuser.FullName;
                    output.CruiseContract.TenantSupplierId = CurrentTenant.Id;
                    output.CruiseContract.SuplierUserId = Currentuser.Id;
                }

                if (output.CruiseContract.TenantsRecipient > 0)
                {
                    var AgentTenant = await TenantManager.GetByIdAsync((int)output.CruiseContract.TenantsRecipient);
                    var AgenttUser = await _userManager.FindByIdAsync(Convert.ToString(output.CruiseContract.UserId));

                    output.CruiseContract.AgentTenantName = AgentTenant.Name;
                    output.CruiseContract.AgentTenantId = AgentTenant.Id;

                    output.CruiseContract.AgentName = AgenttUser.FullName;
                    output.CruiseContract.AgentId = AgenttUser.Id;
                }


                return output;
            }
        }

        public async Task CreateOrEdit(CreateOrEditCruiseContractDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CruiseContract_Create)]
        protected virtual async Task Create(CreateOrEditCruiseContractDto input)
        {
            var cruiseContract = ObjectMapper.Map<Contract>(input);


            if (AbpSession.TenantId != null)
            {
                cruiseContract.contractCreatedate = DateTime.Now;
                cruiseContract.ContractDate = DateTime.Now;

            }


            await _cruiseContractRepository.InsertAsync(cruiseContract);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseContract_Edit)]
        protected virtual async Task Update(CreateOrEditCruiseContractDto input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var cruiseContract = await _cruiseContractRepository.FirstOrDefaultAsync((int)input.Id);
                ObjectMapper.Map(input, cruiseContract);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseContract_Delete)]
        public async Task Delete(EntityDto input)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                await _cruiseContractRepository.DeleteAsync(input.Id);
            }
        }
    }
}
