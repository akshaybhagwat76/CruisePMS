using System.Threading.Tasks;
using CruisePMS.Authorization.Users;

namespace CruisePMS.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
