using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace Smartup.M2M.Education.Infrastructure.Persistence.EfCore
{
    public class EducationDbContextFactory : IDesignTimeDbContextFactory<EducationDbContext>
    {
        public EducationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EducationDbContext>();
            optionsBuilder.UseSqlite("Data Source=SmartupM2M.db");
            return new EducationDbContext(optionsBuilder.Options);
        }
    }
}
