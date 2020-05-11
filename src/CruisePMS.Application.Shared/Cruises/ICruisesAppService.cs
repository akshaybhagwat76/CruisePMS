using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.Common.Dto;
using CruisePMS.Cruises.Dtos;
using CruisePMS.MultiTenancy.Dto;

namespace CruisePMS.Cruises
{
	public interface ICruisesAppService : IApplicationService
	{
		Task<PagedResultDto<GetCruisesForViewDto>> GetAll(GetAllCruisesInput input);

		Task<GetCruisesForViewDto> GetCruisesForView(int id);

		Task<GetCruisesForEditOutput> GetCruisesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCruisesDto input);

		Task Delete(EntityDto input);

		Task<PagedResultDto<CruisesCruiseShipsLookupTableDto>> GetAllCruiseShipsForLookupTable(GetAllForLookupTableInput input);

		Task<PagedResultDto<CruisesCruiseThemesLookupTableDto>> GetAllCruiseThemesForLookupTable(GetAllForLookupTableInput input);

		Task<PagedResultDto<CruisesCruiseServicesLookupTableDto>> GetAllCruiseServicesForLookupTable(GetAllForLookupTableInput input);

		Task<PagedResultDto<CruisesCruiseItinerariesLookupTableDto>> GetAllCruiseItinerariesForLookupTable(GetAllForLookupTableInput input);

		Task<PagedResultDto<GetCruisesForViewDto>> GetAllShipes();
		Task<PagedResultDto<TenantListDto>> GetAllTenant();
	}
}
