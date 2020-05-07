using Abp.Application.Services;
using CruisePMS.Dto;
using CruisePMS.Logging.Dto;

namespace CruisePMS.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
