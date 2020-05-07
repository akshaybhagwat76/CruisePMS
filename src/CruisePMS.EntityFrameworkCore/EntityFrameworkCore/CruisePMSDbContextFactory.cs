using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using CruisePMS.Configuration;
using CruisePMS.Web;

namespace CruisePMS.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class CruisePMSDbContextFactory : IDesignTimeDbContextFactory<CruisePMSDbContext>
    {
        public CruisePMSDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CruisePMSDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            CruisePMSDbContextConfigurer.Configure(builder, configuration.GetConnectionString(CruisePMSConsts.ConnectionStringName));

            return new CruisePMSDbContext(builder.Options);
        }
    }
}