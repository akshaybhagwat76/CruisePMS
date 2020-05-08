using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.Common.Dto;
using CruisePMS.CruiseDepartures.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CruisePMS.CruiseDepartures
{
    public interface ICruiseDeparturesAppService:IApplicationService
    {
        Task<PagedResultDto<GetCruiseDeparturesForViewDto>> GetAll(GetAllCruiseDeparturesInput input);

        Task<GetCruiseDeparturesForViewDto> GetCruiseDeparturesForView(int id);

        Task<GetCruiseDeparturesForEditOutput> GetCruiseDeparturesForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCruiseDeparturesDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<CruiseDeparturesCruisesLookupTableDto>> GetAllCruisesForLookupTable(GetAllForLookupTableInput input);

        Task CreateFromCruiseDefaultSeasons(List<CreateDeparturesDto> collection);

        InsertdRecordsByCruiseId GetRecordsOfDepartureByCruiseId(int cruiseId);

        Task CreateEditedFromCruiseDefaultSeasons(List<CreateDeparturesDto> collection);
    }
}
