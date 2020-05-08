using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CruisePMS.Contracts
{
    public interface IContractAppService : IApplicationService
    {
        Task<PagedResultDto<GetCruiseContractForViewDto>> GetAll(GetAllCruiseContractInput input);

        Task<GetCruiseContractForViewDto> GetCruiseContractForView(int id);

        Task<GetCruiseContractForEditOutput> GetCruiseContractForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCruiseContractDto input);

        Task Delete(EntityDto input);

        Task<CreateRecepientTimeRecordsDto> GetRecepientLoggedIn();
        Task<GetAllCruiseOpratorsTenants> GetAllCruiseOpratorsTenants();
        Task<GetAllCruiseOpratorsTenants> GetAllTravelAgentsContract();
        Task<GetAllCruiseOpratorsTenants> GetAllCruiseOpratorsTenantsForOperator();
        Task<GetAllCruiseOpratorsTenants> GetAllCruiseOpratorsContract();
    }
}
