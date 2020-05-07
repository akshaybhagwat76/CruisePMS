using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace CruisePMS.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
