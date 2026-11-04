using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Smartup.M2M.Education.Infrastructure.Persistence.EfCore.Extensions
{
    public static class EducationDatabaseServiceExtensions
    {
        public static IServiceCollection AddEduDbServices(this IServiceCollection services,string connectionString)
        {
            services.AddDbContext<EducationDbContext>(options =>options.UseSqlite(connectionString));
            return services;
        }
    }
}
