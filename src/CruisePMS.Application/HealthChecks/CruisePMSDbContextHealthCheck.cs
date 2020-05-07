using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using CruisePMS.EntityFrameworkCore;

namespace CruisePMS.HealthChecks
{
    public class CruisePMSDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public CruisePMSDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("CruisePMSDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("CruisePMSDbContext could not connect to database"));
        }
    }
}
