using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.Common.Dto;
using CruisePMS.CruiseServiceGroups.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CruisePMS.CruiseServiceGroups
{
	public interface ICruiseServiceGroupsAppService : IApplicationService
	{
		Task<PagedResultDto<GetCruiseServiceGroupsForViewDto>> GetAll(GetAllCruiseServiceGroupsInput input);

		Task<GetCruiseServiceGroupsForViewDto> GetCruiseServiceGroupsForView(int id);

		Task<GetCruiseServiceGroupsForEditOutput> GetCruiseServiceGroupsForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCruiseServiceGroupsDto input);

		Task Delete(EntityDto input);


		Task<PagedResultDto<CruiseServiceGroupsCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(GetAllForLookupTableInput input);

	}
}
