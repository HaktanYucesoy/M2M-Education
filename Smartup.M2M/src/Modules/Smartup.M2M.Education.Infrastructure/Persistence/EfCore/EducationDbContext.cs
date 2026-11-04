using Microsoft.EntityFrameworkCore;
using Smartup.M2M.Education.Infrastructure.Persistence.EfCore.Configurations;
using Smartup.M2M.Education.Module.Models;

namespace Smartup.M2M.Education.Infrastructure.Persistence.EfCore
{
    public class EducationDbContext : DbContext
    {
        public EducationDbContext(DbContextOptions<EducationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema("edu");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentConfiguration).Assembly);
        }
    }
}
