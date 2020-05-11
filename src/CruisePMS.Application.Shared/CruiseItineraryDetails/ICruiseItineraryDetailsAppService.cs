using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.Common.Dto;
using CruisePMS.CruiseItineraryDetails.Dtos;
using System.Threading.Tasks;
namespace CruisePMS.CruiseItineraryDetails
{
	public interface ICruiseItineraryDetailsAppService : IApplicationService
	{
		Task<PagedResultDto<GetCruiseItineraryDetailsForViewDto>> GetAll(GetAllCruiseItineraryDetailsInput input);

		Task<GetCruiseItineraryDetailsForViewDto> GetCruiseItineraryDetailsForView(int id);

		Task<GetCruiseItineraryDetailsForEditOutput> GetCruiseItineraryDetailsForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCruiseItineraryDetailsDto input);

		Task Delete(EntityDto input);

		Task<PagedResultDto<CruiseItineraryDetailsCruiseItinerariesLookupTableDto>> GetAllCruiseItinerariesForLookupTable(GetAllForLookupTableInput input);

	}
}
