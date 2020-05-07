

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CruisePMS.CruiseMasterAmenities.Exporting;
using CruisePMS.CruiseMasterAmenities.Dtos;
using CruisePMS.Dto;
using Abp.Application.Services.Dto;
using CruisePMS.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using CruisePMS.Configuration;
using Abp.Runtime.Session;
using Abp.Domain.Uow;
using Microsoft.Data.SqlClient;
using Abp.EntityFrameworkCore.Uow;
using CruisePMS.EntityFrameworkCore;

namespace CruisePMS.CruiseMasterAmenities
{
    [AbpAuthorize(AppPermissions.Pages_MasterAmenitieses)]
    public class MasterAmenitiesesAppService : CruisePMSAppServiceBase, IMasterAmenitiesesAppService
    {
        private readonly IRepository<MasterAmenities> _masterAmenitiesRepository;
        private readonly IMasterAmenitiesesExcelExporter _masterAmenitiesesExcelExporter;


        public MasterAmenitiesesAppService(IRepository<MasterAmenities> masterAmenitiesRepository, IMasterAmenitiesesExcelExporter masterAmenitiesesExcelExporter)
        {
            _masterAmenitiesRepository = masterAmenitiesRepository;
            _masterAmenitiesesExcelExporter = masterAmenitiesesExcelExporter;

        }

        public async Task<PagedResultDto<GetMasterAmenitiesForViewDto>> GetAll(GetAllMasterAmenitiesesInput input)
        {


            string defaultCurrentLanguage = await getCurrentLanguageAsync();
            if (string.IsNullOrWhiteSpace(await getCurrentLanguageAsync()))
            { defaultCurrentLanguage = "EN"; }


            var filteredMasterAmenitieses = _masterAmenitiesRepository.GetAll()
                        .WhereIf(input.ParentId.HasValue, x => x.ParentId == Convert.ToInt32(input.ParentId) && x.Lang.ToUpper() == defaultCurrentLanguage.ToUpper())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.DisplayName.Contains(input.Filter) || e.Lang.Contains(input.Filter) || e.DisplayName2.Contains(input.Filter) || e.SourceTable.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DisplayNameFilter), e => e.DisplayName == input.DisplayNameFilter);

            var pagedAndFilteredMasterAmenitieses = filteredMasterAmenitieses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var masterAmenitieses = from o in pagedAndFilteredMasterAmenitieses
                                    select new GetMasterAmenitiesForViewDto()
                                    {
                                        MasterAmenities = new MasterAmenitiesDto
                                        {
                                            DisplayName = o.DisplayName,
                                            Id = o.Id
                                        }
                                    };

            var totalCount = await filteredMasterAmenitieses.CountAsync();

            return new PagedResultDto<GetMasterAmenitiesForViewDto>(
                totalCount,
                await masterAmenitieses.ToListAsync()
            );
        }

        public async Task<GetMasterAmenitiesForViewDto> GetMasterAmenitiesForView(int id)
        {
            var masterAmenities = await _masterAmenitiesRepository.GetAsync(id);

            var output = new GetMasterAmenitiesForViewDto { MasterAmenities = ObjectMapper.Map<MasterAmenitiesDto>(masterAmenities) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MasterAmenitieses_Edit)]
        public async Task<GetMasterAmenitiesForEditOutput> GetMasterAmenitiesForEdit(EntityDto input)
        {
            var masterAmenities = await _masterAmenitiesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMasterAmenitiesForEditOutput { MasterAmenities = ObjectMapper.Map<CreateOrEditMasterAmenitiesDto>(masterAmenities) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMasterAmenitiesDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MasterAmenitieses_Create)]
        protected virtual async Task Create(CreateOrEditMasterAmenitiesDto input)
        {
            string defaultCurrentLanguage = await getCurrentLanguageAsync();
            if (string.IsNullOrWhiteSpace(await getCurrentLanguageAsync()))
            { defaultCurrentLanguage = "EN"; }
            var masterAmenities = ObjectMapper.Map<MasterAmenities>(input);
            var amenities = (_masterAmenitiesRepository.GetAll()).OrderByDescending(x => x.Id);
            var amenitiesOrderColumn = await amenities.Where(x => x.OrderColumn != null).FirstOrDefaultAsync();

            masterAmenities.NewId = amenities.FirstOrDefault() == null ? 1 : amenities.FirstOrDefault().NewId + 1;
            masterAmenities.OrderColumn = amenitiesOrderColumn == null ? 1 : amenitiesOrderColumn.OrderColumn + 1;
            masterAmenities.Lang = defaultCurrentLanguage;
            await _masterAmenitiesRepository.InsertAsync(masterAmenities);
        }

        [AbpAuthorize(AppPermissions.Pages_MasterAmenitieses_Edit)]
        protected virtual async Task Update(CreateOrEditMasterAmenitiesDto input)
        {
            string defaultCurrentLanguage = await getCurrentLanguageAsync();
            if (string.IsNullOrWhiteSpace(await getCurrentLanguageAsync()))
            { defaultCurrentLanguage = "EN"; }

            var masterAmenities = await _masterAmenitiesRepository.FirstOrDefaultAsync((int)input.Id);
            masterAmenities.Lang = defaultCurrentLanguage;
            masterAmenities.DisplayName = input.DisplayName;

            ObjectMapper.Map(input, masterAmenities);
        }

        [AbpAuthorize(AppPermissions.Pages_MasterAmenitieses_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _masterAmenitiesRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMasterAmenitiesesToExcel(GetAllMasterAmenitiesesForExcelInput input)
        {

            var filteredMasterAmenitieses = _masterAmenitiesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.DisplayName.Contains(input.Filter) || e.Lang.Contains(input.Filter) || e.DisplayName2.Contains(input.Filter) || e.SourceTable.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DisplayNameFilter), e => e.DisplayName == input.DisplayNameFilter);

            var query = (from o in filteredMasterAmenitieses
                         select new GetMasterAmenitiesForViewDto()
                         {
                             MasterAmenities = new MasterAmenitiesDto
                             {
                                 DisplayName = o.DisplayName,
                                 Id = o.Id
                             }
                         });


            var masterAmenitiesListDtos = await query.ToListAsync();

            return _masterAmenitiesesExcelExporter.ExportToFile(masterAmenitiesListDtos);
        }


        public async Task ReOrderRow(RowReorder input)
        {
            string defaultCurrentLanguage = await getCurrentLanguageAsync();
            if (string.IsNullOrWhiteSpace(await getCurrentLanguageAsync()))
            { defaultCurrentLanguage = "EN"; }

            var cruiseMasterAmenities = await _masterAmenitiesRepository.GetAll().Where(x => x.NewId == (int)input.DropIndexId).FirstOrDefaultAsync();
            cruiseMasterAmenities.Lang = defaultCurrentLanguage;
            cruiseMasterAmenities.OrderColumn = input.DropIndex;
            await _masterAmenitiesRepository.UpdateAsync(cruiseMasterAmenities);
        }



        public async Task ReOrderRowByIcon(ReorderRowsByIcon input)
        {
            string defaultCurrentLanguage = await SettingManager.GetSettingValueForUserAsync(AppSettings.DefaultCurrentLanguage, AbpSession.ToUserIdentifier());
            if (string.IsNullOrWhiteSpace(defaultCurrentLanguage))
            { defaultCurrentLanguage = "EN"; }

            await ReorderMasterAmenities(input);
        }

        private async Task<string> getCurrentLanguageAsync()
        {
            string defaultCurrentLanguage = await SettingManager.GetSettingValueForUserAsync(AppSettings.DefaultCurrentLanguage, AbpSession.ToUserIdentifier());
            if (string.IsNullOrWhiteSpace(defaultCurrentLanguage))
            { defaultCurrentLanguage = "EN"; }

            return defaultCurrentLanguage;
        }


        private async Task<long> ReorderMasterAmenities(ReorderRowsByIcon input)
        {
            long affectedRows = 0;
            try
            {
                UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                List<SqlParameter> pc = new List<SqlParameter>
                {
                    new SqlParameter("PresentRowNewId", input.PresentRowNewId),
                    new SqlParameter("NewRowOrderValue", input.NewRowOrderValue),
                    new SqlParameter("Parentid ", input.ParentId)
                };

                var ID = await CurrentUnitOfWork.GetDbContext<CruisePMSDbContext>().Database.ExecuteSqlRawAsync("ReorderMasterAmenities @PresentRowNewId, @NewRowOrderValue,@Parentid", pc.ToArray());
                if (ID > 0 || ID < 0 || ID == 0)
                {
                }
                else
                {
                    affectedRows = 0;
                };

                await CurrentUnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
            return affectedRows;
        }
    }
}