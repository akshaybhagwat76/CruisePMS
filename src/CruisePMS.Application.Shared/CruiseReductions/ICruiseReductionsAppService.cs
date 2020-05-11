using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.Common.Dto;
using CruisePMS.CruiseReductions.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CruisePMS.CruiseReductions
{
    public interface ICruiseReductionsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCruiseReductionsForViewDto>> GetAll(GetAllCruiseReductionsInput input);

        Task<GetCruiseReductionsForViewDto> GetCruiseReductionsForView(int id);

        Task<GetCruiseReductionsForEditOutput> GetCruiseReductionsForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCruiseReductionsDto input);

        Task Delete(EntityDto input);

        GetReductionsScreenDto GetAllReductions();

        Task<PagedResultDto<CruiseReductionsCruiseServicesLookupTableDto>> GetAllCruiseServicesForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<CruiseReductionsAgePoliciesLookupTableDto>> GetAllAgePoliciesForLookupTable(GetAllForLookupTableInput input);
        Task SaveReductions(List<ReductionsScreen> reductionsScreen);
    }
}
