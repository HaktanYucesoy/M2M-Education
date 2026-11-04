

namespace Smartup.M2M.Education.Module.DTOs
{
    public record StudentCourseDto
    {
        public string StudentFullName { get; init; } = string.Empty;

        public List<CourseDetailDto> Courses { get; set; } = new();

    }



    public record CourseDetailDto
    {
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;

    }
}

