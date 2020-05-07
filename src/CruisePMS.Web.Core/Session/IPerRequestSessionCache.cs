using System.Threading.Tasks;
using CruisePMS.Sessions.Dto;

namespace CruisePMS.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
