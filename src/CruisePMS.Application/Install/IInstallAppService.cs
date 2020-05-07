using System.Threading.Tasks;
using Abp.Application.Services;
using CruisePMS.Install.Dto;

namespace CruisePMS.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}