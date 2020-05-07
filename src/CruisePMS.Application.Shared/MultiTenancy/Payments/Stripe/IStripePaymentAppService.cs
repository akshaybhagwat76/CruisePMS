using System.Threading.Tasks;
using Abp.Application.Services;
using CruisePMS.MultiTenancy.Payments.Dto;
using CruisePMS.MultiTenancy.Payments.Stripe.Dto;

namespace CruisePMS.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}