using Smartup.M2M.BuildingBlocks.Contracts.Pagination;
using Smartup.M2M.BuildingBlocks.Results;
using Smartup.M2M.Education.Module.DTOs;

namespace Smartup.M2M.Education.Module.Contracts
{
    public interface IStudentCourseProvider
    {
        Task<Result<PaginatedResponse<StudentCourseDto>>> GetStudentCoursesAsync(
            PageRequest? pageRequest,
            string? criteria,
            CancellationToken cancellationToken = default);
    }
}
