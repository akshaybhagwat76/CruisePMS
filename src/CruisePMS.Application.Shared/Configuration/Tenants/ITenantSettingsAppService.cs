using System.Threading.Tasks;
using Abp.Application.Services;
using CruisePMS.Configuration.Tenants.Dto;

namespace CruisePMS.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
