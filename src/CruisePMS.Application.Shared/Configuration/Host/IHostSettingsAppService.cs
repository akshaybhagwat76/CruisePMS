using System.Threading.Tasks;
using Abp.Application.Services;
using CruisePMS.Configuration.Host.Dto;

namespace CruisePMS.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
