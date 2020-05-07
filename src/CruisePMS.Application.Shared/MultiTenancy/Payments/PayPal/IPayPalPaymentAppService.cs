using System.Threading.Tasks;
using Abp.Application.Services;
using CruisePMS.MultiTenancy.Payments.PayPal.Dto;

namespace CruisePMS.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
