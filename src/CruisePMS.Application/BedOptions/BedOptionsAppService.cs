using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using CruisePMS.Authorization;
using CruisePMS.BedOptions.Dtos;
using CruisePMS.CruiseMasterAmenities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using CruisePMS.CruiseMasterAmenities.Dtos;

namespace CruisePMS.BedOptions
{
    [AbpAuthorize(AppPermissions.Pages_BedOptions)]
    public class BedOptionsAppService : CruisePMSAppServiceBase, IBedOptionsAppService
    {
        private readonly IRepository<BedOption> _bedOptionsRepository;
        private readonly IRepository<MasterAmenities, int> _lookup_cruiseMasterAmenitiesRepository;

        public BedOptionsAppService(IRepository<BedOption> bedOptionsRepository, IRepository<MasterAmenities, int> lookup_cruiseMasterAmenitiesRepository)
        {
            _bedOptionsRepository = bedOptionsRepository;
            _lookup_cruiseMasterAmenitiesRepository = lookup_cruiseMasterAmenitiesRepository;
        }

        public async Task<PagedResultDto<GetBedOptionsForViewDto>> GetAll(GetAllBedOptionsInput input)
        {
            IQueryable<BedOption> filteredBedOptions = _bedOptionsRepository.GetAll()
                        .Include(e => e.BedOptionNaFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinBedCapacityFilter != null, e => e.BedCapacity >= input.MinBedCapacityFilter)
                        .WhereIf(input.MaxBedCapacityFilter != null, e => e.BedCapacity <= input.MaxBedCapacityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CruiseMasterAmenitiesDisplayNameFilter), e => e.BedOptionNaFk != null && e.BedOptionNaFk.DisplayName.ToLower() == input.CruiseMasterAmenitiesDisplayNameFilter.ToLower().Trim());

            IQueryable<BedOption> pagedAndFilteredBedOptions = filteredBedOptions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            IQueryable<GetBedOptionsForViewDto> bedOptions = from o in pagedAndFilteredBedOptions
                                                             join o1 in _lookup_cruiseMasterAmenitiesRepository.GetAll() on o.BedOptionName equals o1.Id into j1
                                                             from s1 in j1.DefaultIfEmpty()

                                                             select new GetBedOptionsForViewDto()
                                                             {
                                                                 BedOptions = new BedOptionsDto
                                                                 {
                                                                     BedCapacity = o.BedCapacity,
                                                                     Id = o.Id
                                                                 },
                                                                 CruiseMasterAmenitiesDisplayName = s1 == null ? "" : s1.DisplayName.ToString()
                                                             };

            int totalCount = await filteredBedOptions.CountAsync();

            return new PagedResultDto<GetBedOptionsForViewDto>(
                totalCount,
                await bedOptions.ToListAsync()
            );
        }

        public async Task<GetBedOptionsForViewDto> GetBedOptionsForView(int id)
        {
            BedOption bedOptions = await _bedOptionsRepository.GetAsync(id);

            GetBedOptionsForViewDto output = new GetBedOptionsForViewDto { BedOptions = ObjectMapper.Map<BedOptionsDto>(bedOptions) };

            if (output.BedOptions.BedOptionName != null)
            {
                MasterAmenities _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.BedOptions.BedOptionName);
                output.CruiseMasterAmenitiesDisplayName = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_BedOptions_Edit)]
        public async Task<GetBedOptionsForEditOutput> GetBedOptionsForEdit(EntityDto input)
        {
            BedOption bedOptions = await _bedOptionsRepository.FirstOrDefaultAsync(input.Id);

            GetBedOptionsForEditOutput output = new GetBedOptionsForEditOutput { BedOptions = ObjectMapper.Map<CreateOrEditBedOptionsDto>(bedOptions) };

            if (output.BedOptions.BedOptionName != null)
            {
                MasterAmenities _lookupCruiseMasterAmenities = await _lookup_cruiseMasterAmenitiesRepository.FirstOrDefaultAsync((int)output.BedOptions.BedOptionName);
                output.CruiseMasterAmenitiesDisplayName = _lookupCruiseMasterAmenities.DisplayName.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBedOptionsDto input)
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

        [AbpAuthorize(AppPermissions.Pages_BedOptions_Create)]
        private async Task Create(CreateOrEditBedOptionsDto input)
        {
            BedOption bedOptions = ObjectMapper.Map<BedOption>(input);
            await _bedOptionsRepository.InsertAsync(bedOptions);
        }

        [AbpAuthorize(AppPermissions.Pages_BedOptions_Edit)]
        private async Task Update(CreateOrEditBedOptionsDto input)
        {
            BedOption bedOptions = await _bedOptionsRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, bedOptions);
        }

        [AbpAuthorize(AppPermissions.Pages_BedOptions_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _bedOptionsRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_BedOptions)]
        public async Task<PagedResultDto<BedOptionsCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(GetAllForLookupTableInput input)
        {
            IQueryable<MasterAmenities> query = _lookup_cruiseMasterAmenitiesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.DisplayName.ToString().Contains(input.Filter)
                ).Where(o => o.ParentId == 110);

            int totalCount = await query.CountAsync();

            List<MasterAmenities> cruiseMasterAmenitiesList = await query
                .PageBy(input)
                .ToListAsync();

            List<BedOptionsCruiseMasterAmenitiesLookupTableDto> lookupTableDtoList = new List<BedOptionsCruiseMasterAmenitiesLookupTableDto>();
            foreach (MasterAmenities cruiseMasterAmenities in cruiseMasterAmenitiesList)
            {
                lookupTableDtoList.Add(new BedOptionsCruiseMasterAmenitiesLookupTableDto
                {
                    Id = cruiseMasterAmenities.Id,
                    DisplayName = cruiseMasterAmenities.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<BedOptionsCruiseMasterAmenitiesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}
