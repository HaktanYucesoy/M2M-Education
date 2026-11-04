using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Smartup.M2M.Education.Module.Models;


namespace Smartup.M2M.Education.Infrastructure.Persistence.EfCore.Extensions
{
    public static class DatabaseSeederExtension
    {
        public static async Task ApplyMigrationsAndSeedAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EducationDbContext>();

            await context.Database.MigrateAsync();

            
            if (await context.Students.AnyAsync())
                return;

            var courseFaker = new Faker<Course>("tr")
                .RuleFor(c => c.Code, f => f.Commerce.Ean8())
                .RuleFor(c => c.Name, f => f.Company.CatchPhrase())
                .RuleFor(c => c.Detail, f => f.Lorem.Sentence());

            var courses = courseFaker.Generate(10);
            await context.Courses.AddRangeAsync(courses);

            var studentFaker = new Faker<Student>("tr")
                .CustomInstantiator(f => new Student(f.Name.FirstName(), f.Name.LastName()));

            var students = studentFaker.Generate(200);
            await context.Students.AddRangeAsync(students);

            await context.SaveChangesAsync();

            
            var random = new Random();
            var enrollments = new List<Enrollment>();

            foreach (var student in students)
            {
                
                var randomCourses = courses.OrderBy(x => random.Next()).Take(random.Next(1, 4)).ToList();

                foreach (var course in randomCourses)
                {
                    enrollments.Add(new Enrollment
                    {
                        StudentId = student.Id,
                        CourseId = course.Id
                    });
                }
            }

            await context.Enrollments.AddRangeAsync(enrollments);
            await context.SaveChangesAsync();
        }
    }
}
