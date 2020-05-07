using System.Threading.Tasks;
using Abp.Application.Services;

namespace CruisePMS.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
