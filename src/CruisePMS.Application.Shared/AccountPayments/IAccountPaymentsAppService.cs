using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.AccountPayments.Dtos;
using System.Threading.Tasks;

namespace CruisePMS.AccountPayments
{
    public interface IAccountPaymentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetAccountPaymentsForViewDto>> GetAll(GetAllAccountPaymentsInput input);

        Task<GetAccountPaymentsForViewDto> GetAccountPaymentsForView(int id);

        Task<GetAccountPaymentsForEditOutput> GetAccountPaymentsForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAccountPaymentsDto input);

        Task Delete(EntityDto input);

    }
}
