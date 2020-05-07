using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace CruisePMS.EntityFrameworkCore
{
    public static class CruisePMSDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<CruisePMSDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<CruisePMSDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}