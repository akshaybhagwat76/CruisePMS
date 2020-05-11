using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using CruisePMS.Authorization;
using CruisePMS.CruiseMasterAmenities;
using CruisePMS.CruiseServiceGroups.Dtos;
using CruisePMS.Common.Dto;

namespace CruisePMS.CruiseServiceGroups
{
    [AbpAuthorize(AppPermissions.Pages_CruiseServiceGroups)]
    public class CruiseServiceGroupsAppService : CruisePMSAppServiceBase, ICruiseServiceGroupsAppService
    {
        private readonly IRepository<CruiseServiceGroup> _cruiseServiceGroupsRepository;
        private readonly IRepository<MasterAmenities, int> _lookup_cruiseMasterAmenitiesRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CruiseServiceGroupsAppService(IRepository<CruiseServiceGroup> cruiseServiceGroupsRepository, IUnitOfWorkManager unitOfWorkManager, IRepository<MasterAmenities, int> lookup_cruiseMasterAmenitiesRepository)
        {
            _cruiseServiceGroupsRepository = cruiseServiceGroupsRepository;
            _lookup_cruiseMasterAmenitiesRepository = lookup_cruiseMasterAmenitiesRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<PagedResultDto<GetCruiseServiceGroupsForViewDto>> GetAll(GetAllCruiseServiceGroupsInput input)
        {

            IQueryable<CruiseServiceGroup> filteredCruiseServiceGroups = _cruiseServiceGroupsRepository.GetAll()
                        .Include(e => e.ServiceGroupNaFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruiseMasterAmenitiesDisplayNameFilter), e => e.ServiceGroupNaFk != null && e.ServiceGroupNaFk.DisplayName.ToLower() == input.CruiseMasterAmenitiesDisplayNameFilter.ToLower().Trim());

            IQueryable<CruiseServiceGroup> pagedAndFilteredCruiseServiceGroups = filteredCruiseServiceGroups
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            IQueryable<GetCruiseServiceGroupsForViewDto> cruiseServiceGroups = from o in pagedAndFilteredCruiseServiceGroups
                                                                               join o1 in _lookup_cruiseMasterAmenitiesRepository.GetAll() on o.ServiceGroupName equals o1.Id into j1
                                                                               from s1 in j1.DefaultIfEmpty()

                                                                               select new GetCruiseServiceGroupsForViewDto()
                                                                               {
                                                                                   CruiseServiceGroups = new CruiseServiceGroupsDto
                                                                                   {
                                                                                       IsMainService = o.IsMainService,
                                                                                       OnlyOneCanBeChoosen = o.OnlyOneCanBeChoosen,
                                                                                       Id = o.Id
                                                                                   },
                                                                                   CruiseMasterAmenitiesDisplayName = s1 == null ? "" : s1.DisplayName.ToString()
                                                                               };

            int totalCount = await filteredCruiseServiceGroups.CountAsync();

            return new PagedResultDto<GetCruiseServiceGroupsForViewDto>(
                totalCount,
                await cruiseServiceGroups.ToListAsync()
            );
        }




        public async Task<GetCruiseServiceGroupsForViewDto> GetCruiseServiceGroupsForView(int id)
        {
            CruiseServiceGroup cruiseServiceGroups = await _cruiseServiceGroupsRepository.GetAsync(id);

            GetCruiseServiceGroupsForViewDto output = new GetCruiseServiceGroupsForViewDto { CruiseServiceGroups = ObjectMapper.Map<CruiseServiceGroupsDto>(cruiseServiceGroups) };

            if (output.CruiseServiceGroups.ServiceGroupName != null)
            {
                MasterAmenities _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.CruiseServiceGroups.ServiceGroupName);
                output.CruiseMasterAmenitiesDisplayName = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseServiceGroups_Edit)]
        public async Task<GetCruiseServiceGroupsForEditOutput> GetCruiseServiceGroupsForEdit(EntityDto input)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            CruiseServiceGroup cruiseServiceGroups = await _cruiseServiceGroupsRepository.FirstOrDefaultAsync(input.Id);

            GetCruiseServiceGroupsForEditOutput output = new GetCruiseServiceGroupsForEditOutput { CruiseServiceGroups = ObjectMapper.Map<CreateOrEditCruiseServiceGroupsDto>(cruiseServiceGroups) };

            if (output.CruiseServiceGroups.ServiceGroupName != null)
            {
                MasterAmenities _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.CruiseServiceGroups.ServiceGroupName);
                output.CruiseMasterAmenitiesDisplayName = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCruiseServiceGroupsDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CruiseServiceGroups_Create)]
        private async Task Create(CreateOrEditCruiseServiceGroupsDto input)
        {
            CruiseServiceGroup cruiseServiceGroups = ObjectMapper.Map<CruiseServiceGroup>(input);


            if (AbpSession.TenantId != null)
            {
                cruiseServiceGroups.TenantId = AbpSession.TenantId;
            }


            await _cruiseServiceGroupsRepository.InsertAsync(cruiseServiceGroups);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseServiceGroups_Edit)]
        private async Task Update(CreateOrEditCruiseServiceGroupsDto input)
        {
            CruiseServiceGroup cruiseServiceGroups = await _cruiseServiceGroupsRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, cruiseServiceGroups);
        }

        [AbpAuthorize(AppPermissions.Pages_CruiseServiceGroups_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _cruiseServiceGroupsRepository.DeleteAsync(input.Id);
        }

   

        [AbpAuthorize(AppPermissions.Pages_CruiseServiceGroups)]
        public async Task<PagedResultDto<CruiseServiceGroupsCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(GetAllForLookupTableInput input)
        {
            var  query = _lookup_cruiseMasterAmenitiesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.DisplayName.ToString().Contains(input.Filter)
                ).Where(o => o.ParentId == 126);

            int totalCount = await query.CountAsync();

            List<MasterAmenities> cruiseMasterAmenitiesList = await query
                .PageBy(input)
                .ToListAsync();

            List<CruiseServiceGroupsCruiseMasterAmenitiesLookupTableDto> lookupTableDtoList = new List<CruiseServiceGroupsCruiseMasterAmenitiesLookupTableDto>();
            foreach (MasterAmenities cruiseMasterAmenities in cruiseMasterAmenitiesList)
            {
                lookupTableDtoList.Add(new CruiseServiceGroupsCruiseMasterAmenitiesLookupTableDto
                {
                    Id = cruiseMasterAmenities.Id,
                    DisplayName = cruiseMasterAmenities.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<CruiseServiceGroupsCruiseMasterAmenitiesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}
