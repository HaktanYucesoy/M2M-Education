
namespace Smartup.M2M.Education.Module.Models
{
    public class Enrollment
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
