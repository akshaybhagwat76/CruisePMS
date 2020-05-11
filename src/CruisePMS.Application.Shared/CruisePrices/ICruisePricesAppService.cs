using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.Common.Dto;
using CruisePMS.CruisePrices.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace CruisePMS.CruisePrices
{
    public interface ICruisePricesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCruisePricesForViewDto>> GetAll(GetAllCruisePricesInput input);

        Task<GetCruisePricesForViewDto> GetCruisePricesForView(int id);

        Task<GetCruisePricesForEditOutput> GetCruisePricesForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCruisePricesDto input);

        Task Delete(EntityDto input);


        Task<PagedResultDto<CruisePricesCruiseFaresLookupTableDto>> GetAllCruiseFaresForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<CruisePricesCruiseDeparturesLookupTableDto>> GetAllCruiseDeparturesForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<CruisePricesCruiseShipCabinsLookupTableDto>> GetAllCruiseShipCabinsForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<CruisePricesCruiseServicesLookupTableDto>> GetAllCruiseServicesForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<CruisePricesCruiseShipsLookupTableDto>> GetAllCruiseShipsForLookupTable(GetAllForLookupTableInput input);

        PriceScreenRecordDto GetCreatePriceScreenRecord(int fareId);

        PriceScreenRecordDto GetCreateCharterPriceScreenRecord(int fareId);
        Task SavePriceScreenRecord(List<SavePriceScreenRecordDto> items);

        Task SaveCharterPriceScreenRecord(List<SavePriceScreenRecordDto> items);

        CruisePriceModalsScreen CruisePriceModalsScreen(GetCruisePriceModalsScreen getCruisePriceModalsScreen);
        Task SavePriceModalScreenRecord(List<SavePriceScreenRecordDto> items);
        Task DeleteCruisePriceById(EntityDto input);
        Task DeleteAllCruisePriceById(EntityDto input);
    }
}
