using Microsoft.Extensions.DependencyInjection;
using Smartup.M2M.Education.Module.Contracts;


namespace Smartup.M2M.Education.Infrastructure.Providers.Extensions
{
    public static class EducationProviderServiceExtensions
    {
        public static IServiceCollection AddStudentCourseProvider(this IServiceCollection services)
        {
            services.AddScoped<IStudentCourseProvider, StudentCourseProvider>();
            return services;
        }
    }
}
