using Microsoft.EntityFrameworkCore;
using Smartup.M2M.BuildingBlocks.Contracts.Pagination;
using Smartup.M2M.BuildingBlocks.Results;
using Smartup.M2M.Education.Infrastructure.Persistence.EfCore;
using Smartup.M2M.Education.Module.Contracts;
using Smartup.M2M.Education.Module.DTOs;
using Smartup.M2M.Infrastructure.Persistence.EfCore.Extensions;


namespace Smartup.M2M.Education.Infrastructure.Providers
{
    public class StudentCourseProvider : IStudentCourseProvider
    {
        private readonly EducationDbContext _dbContext;

        public StudentCourseProvider(EducationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Result<PaginatedResponse<StudentCourseDto>>> GetStudentCoursesAsync(
                PageRequest? pageRequest,
                string? criteria,
                CancellationToken cancellationToken = default)
        {
            // Senin yazdığın orijinal JOIN mantığı tamamen korundu
            var query = from enrollment in _dbContext.Enrollments.AsNoTracking()
                        join student in _dbContext.Students.AsNoTracking()
                            on enrollment.StudentId equals student.Id
                        join course in _dbContext.Courses.AsNoTracking()
                            on enrollment.CourseId equals course.Id
                        select new { enrollment, student, course };

            if (!string.IsNullOrWhiteSpace(criteria))
            {
                query = query.Where(x =>
                    x.student.FullName.StartsWith(criteria) ||
                    x.course.Name.StartsWith(criteria));
            }

           
            var projectedQuery = query
                .GroupBy(x => new { x.student.Id, x.student.FullName })
                .Select(g => new StudentCourseDto
                {
                    StudentFullName = g.Key.FullName,
                    Courses = g.Select(x => new CourseDetailDto
                    {
                        Code = x.course.Code,
                        Name = x.course.Name,
                        Description = x.course.Detail
                    }).ToList()
                });

            if (pageRequest == null || pageRequest.PageSize <= 0)
            {
                var allData = await projectedQuery.ToListAsync(cancellationToken);

                var unpaginatedResponse = new PaginatedResponse<StudentCourseDto>(
                    items: allData,
                    totalCount: allData.Count,
                    pageIndex: 1,
                    pageSize: allData.Count > 0 ? allData.Count : 1);

                return Result.Success(unpaginatedResponse);
            }

            var paginatedData = await projectedQuery.ToPaginatedResponseAsync(
                pageRequest.PageNumber,
                pageRequest.PageSize,
                cancellationToken);

            return Result.Success(paginatedData);
        }
    }
}
