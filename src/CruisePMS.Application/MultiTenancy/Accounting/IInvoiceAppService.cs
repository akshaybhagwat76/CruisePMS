using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using CruisePMS.MultiTenancy.Accounting.Dto;

namespace CruisePMS.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
