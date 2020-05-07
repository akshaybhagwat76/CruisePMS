using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using CruisePMS.Authorization;
using CruisePMS.Configuration;
using CruisePMS.CruiseTenantTypes;
using CruisePMS.CruiseTenantTypes.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;


using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Runtime.Session;

namespace CruisePMS.MultiTenancy
{
    [AbpAuthorize(AppPermissions.Pages_TenantTypes)]

    public  class TenantTypesAppService: CruisePMSAppServiceBase, ITenantTypesAppService
    {
        private readonly IRepository<TenantTypes> _tenantTypesRepository;

        public TenantTypesAppService(IRepository<TenantTypes> tenantTypesRepository)
        {
            _tenantTypesRepository = tenantTypesRepository;
        }

        public async Task<PagedResultDto<GetTenantTypesForViewDto>> GetAll(GetAllTenantTypesInput input)
        {


            string defaultCurrentLanguage = await getCurrentLanguageAsync();
            if (string.IsNullOrWhiteSpace(await getCurrentLanguageAsync()))
            { defaultCurrentLanguage = "EN"; }


            var filteredTenantTypes = _tenantTypesRepository.GetAll()
                        .WhereIf(!string.IsNullOrEmpty(input.TenantTypeName), x => x.TenantTypeName == input.TenantTypeName);

            var pagedAndFilteredTenantTypes = filteredTenantTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var tenantTypes = from o in pagedAndFilteredTenantTypes
                              select new GetTenantTypesForViewDto()
                                    {
                                        TenantTypes = new TenantTypesDto
                                        {
                                            TenantTypeName = o.TenantTypeName,
                                            Id = o.Id
                                        }
                                    };

            var totalCount = await filteredTenantTypes.CountAsync();

            return new PagedResultDto<GetTenantTypesForViewDto>(
                totalCount,
                await tenantTypes.ToListAsync()
            );
        }
        public async Task<GetTenantTypesForViewDto> GetTenantTypesForView(int id)
        {
            var tenantTypes = await _tenantTypesRepository.GetAsync(id);

            var output = new GetTenantTypesForViewDto { TenantTypes = ObjectMapper.Map<TenantTypesDto>(tenantTypes) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TenantTypes_Edit)]
        public async Task<GetTenantTypesForEditOutput> GetTenantTypesForEdit(EntityDto input)
        {
            var tenantTypes = await _tenantTypesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTenantTypesForEditOutput { TenantTypes = ObjectMapper.Map<CreateOrEditTenantTypesDto>(tenantTypes) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTenantTypesDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TenantTypes_Create)]
        protected virtual async Task Create(CreateOrEditTenantTypesDto input)
        {
            string defaultCurrentLanguage = await getCurrentLanguageAsync();
            if (string.IsNullOrWhiteSpace(await getCurrentLanguageAsync()))
            { defaultCurrentLanguage = "EN"; }
            var tenantTypes = ObjectMapper.Map<TenantTypes>(input);

            await _tenantTypesRepository.InsertAsync(tenantTypes);
        }

        [AbpAuthorize(AppPermissions.Pages_TenantTypes_Edit)]
        protected virtual async Task Update(CreateOrEditTenantTypesDto input)
        {
            string defaultCurrentLanguage = await getCurrentLanguageAsync();
            if (string.IsNullOrWhiteSpace(await getCurrentLanguageAsync()))
            { defaultCurrentLanguage = "EN"; }

            var tenantTypes = await _tenantTypesRepository.FirstOrDefaultAsync((int)input.Id);

            ObjectMapper.Map(input, tenantTypes);
        }

        [AbpAuthorize(AppPermissions.Pages_TenantTypes_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _tenantTypesRepository.DeleteAsync(input.Id);
        }


        private async Task<string> getCurrentLanguageAsync()
        {
            string defaultCurrentLanguage = await SettingManager.GetSettingValueForUserAsync(AppSettings.DefaultCurrentLanguage, AbpSession.ToUserIdentifier());
            if (string.IsNullOrWhiteSpace(defaultCurrentLanguage))
            { defaultCurrentLanguage = "EN"; }

            return defaultCurrentLanguage;
        }
    }
}
