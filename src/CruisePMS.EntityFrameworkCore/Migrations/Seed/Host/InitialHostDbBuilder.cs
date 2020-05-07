using CruisePMS.EntityFrameworkCore;

namespace CruisePMS.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly CruisePMSDbContext _context;

        public InitialHostDbBuilder(CruisePMSDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
