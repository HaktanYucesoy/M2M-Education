using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smartup.M2M.Education.Module.Models;

namespace Smartup.M2M.Education.Infrastructure.Persistence.EfCore.Configurations
{
    public class StudentConfiguration:IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(s => s.LastName).IsRequired().HasMaxLength(100);
            builder.Property(s => s.FullName).IsRequired();
            builder.HasIndex(s => s.FullName);
        }
    
    }
}
