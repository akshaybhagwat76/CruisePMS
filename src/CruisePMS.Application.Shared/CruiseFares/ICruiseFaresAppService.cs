using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.Common.Dto;
using CruisePMS.CruiseDepartures.Dtos;
using CruisePMS.CruiseFares.Dtos;
using System.Threading.Tasks;
namespace CruisePMS.CruiseFares
{
    public interface ICruiseFaresAppService : IApplicationService
    {
        Task<PagedResultDto<GetCruiseFaresForViewDto>> GetAll(GetAllCruiseFaresInput input);
        Task<PagedResultDto<GetCruiseFaresForViewDto>> GetAllCharterfaresByShip(GetAllCruiseFaresInput input);


        Task<GetCruiseFaresForViewDto> GetCruiseFaresForView(int id);

        Task<GetCruiseFaresForEditOutput> GetCruiseFaresForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCruiseFaresDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<CruiseFaresCruisesLookupTableDto>> GetAllCruisesForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<CruiseFaresCruiseMasterAmenitiesLookupTableDto>> GetAllCruiseMasterAmenitiesForLookupTable(GetAllForLookupTableInput input);
        InsertdRecordsByCruiseId GetRecordsOfCruiseDeparturesDatesDeparture(int cruiseId);
    }
}
