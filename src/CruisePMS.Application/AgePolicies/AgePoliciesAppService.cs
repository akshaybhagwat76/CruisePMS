using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using CruisePMS.AgePolicies.Dtos;
using CruisePMS.Authorization;
using CruisePMS.CruiseMasterAmenities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using CruisePMS.CruiseMasterAmenities.Dtos;

namespace CruisePMS.AgePolicies
{
    [AbpAuthorize(AppPermissions.Pages_AgePolicies)]
    public class AgePoliciesAppService : CruisePMSAppServiceBase, IAgePoliciesAppService
    {
        private readonly IRepository<AgePolicy, long> _agePoliciesRepository;
        private readonly IRepository<MasterAmenities, int> _lookup_cruiseMasterAmenitiesRepository;

        
        public AgePoliciesAppService(IRepository<AgePolicy, long> agePoliciesRepository, IRepository<MasterAmenities, int> lookup_cruiseMasterAmenitiesRepository)
        {
            _agePoliciesRepository = agePoliciesRepository;
            _lookup_cruiseMasterAmenitiesRepository = lookup_cruiseMasterAmenitiesRepository;

        }

        public async Task<PagedResultDto<GetAgePoliciesForViewDto>> GetAll(GetAllAgePoliciesInput input)
        {

            var filteredAgePolicies = _agePoliciesRepository.GetAll()
                        .Include(e => e.GuestTyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruiseMasterAmenitiesDisplayNameFilter), e => e.GuestTyFk != null && e.GuestTyFk.DisplayName.ToLower() == input.CruiseMasterAmenitiesDisplayNameFilter.ToLower().Trim());

            var pagedAndFilteredAgePolicies = filteredAgePolicies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var agePolicies = from o in pagedAndFilteredAgePolicies
                              join o1 in _lookup_cruiseMasterAmenitiesRepository.GetAll() on o.GuestType equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              select new GetAgePoliciesForViewDto()
                              {
                                  AgePolicies = new AgePoliciesDto
                                  {
                                      AgeFrom = o.AgeFrom,
                                      AgeTo = o.AgeTo,
                                      Id = o.Id
                                  },
                                  CruiseMasterAmenitiesDisplayName = s1 == null ? "" : s1.DisplayName.ToString()
                              };

            var totalCount = await filteredAgePolicies.CountAsync();

            return new PagedResultDto<GetAgePoliciesForViewDto>(
                totalCount,
                await agePolicies.ToListAsync()
            );
        }

        public async Task<GetAgePoliciesForViewDto> GetAgePoliciesForView(long id)
        {
            var agePolicies = await _agePoliciesRepository.GetAsync(id);

            var output = new GetAgePoliciesForViewDto { AgePolicies = ObjectMapper.Map<AgePoliciesDto>(agePolicies) };

            if (output.AgePolicies.GuestType != null)
            {
                var _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.AgePolicies.GuestType);
                output.CruiseMasterAmenitiesDisplayName = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AgePolicies_Edit)]
        public async Task<GetAgePoliciesForEditOutput> GetAgePoliciesForEdit(EntityDto<long> input)
        {
            var agePolicies = await _agePoliciesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAgePoliciesForEditOutput { AgePolicies = ObjectMapper.Map<CreateOrEditAgePoliciesDto>(agePolicies) };

            if (output.AgePolicies.GuestType != null)
            {
                var _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.AgePolicies.GuestType);
                output.CruiseMasterAmenitiesDisplayName = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAgePoliciesDto input)
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

        [AbpAuthorize(AppPermissions.Pages_AgePolicies_Create)]
        private async Task Create(CreateOrEditAgePoliciesDto input)
        {
            var agePolicies = ObjectMapper.Map<AgePolicy>(input);


            if (AbpSession.TenantId != null)
            {
                agePolicies.TenantId = (int?)AbpSession.TenantId;
            }


            await _agePoliciesRepository.InsertAsync(agePolicies);
        }

        [AbpAuthorize(AppPermissions.Pages_AgePolicies_Edit)]
        private async Task Update(CreateOrEditAgePoliciesDto input)
        {
            var agePolicies = await _agePoliciesRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, agePolicies);
        }

        [AbpAuthorize(AppPermissions.Pages_AgePolicies_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _agePoliciesRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_AgePolicies)]
        public async Task<PagedResultDto<AgePoliciesCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_cruiseMasterAmenitiesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DisplayName.ToString().Contains(input.Filter)
               ).Where(x => x.ParentId == 67);

            var totalCount = await query.CountAsync();

            var cruiseMasterAmenitiesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<AgePoliciesCruiseMasterAmenitiesLookupTableDto>();
            foreach (var cruiseMasterAmenities in cruiseMasterAmenitiesList)
            {
                lookupTableDtoList.Add(new AgePoliciesCruiseMasterAmenitiesLookupTableDto
                {
                    Id = cruiseMasterAmenities.Id,
                    DisplayName = cruiseMasterAmenities.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<AgePoliciesCruiseMasterAmenitiesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}
