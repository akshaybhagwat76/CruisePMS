using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CruisePMS.Caching.Dto;

namespace CruisePMS.Caching
{
    public interface ICachingAppService : IApplicationService
    {
        ListResultDto<CacheDto> GetAllCaches();

        Task ClearCache(EntityDto<string> input);

        Task ClearAllCaches();
    }
}
