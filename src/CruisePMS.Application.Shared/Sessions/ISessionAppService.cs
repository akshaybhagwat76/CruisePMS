using System.Threading.Tasks;
using Abp.Application.Services;
using CruisePMS.Sessions.Dto;

namespace CruisePMS.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
