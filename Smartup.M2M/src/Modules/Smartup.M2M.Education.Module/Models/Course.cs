using Smartup.M2M.BuildingBlocks.Domains;


namespace Smartup.M2M.Education.Module.Models
{
    public class Course : BaseEntity
    {
        public Course() { }   
        public string Name { get; set; }

        public string Code { get; set; }

        public string Detail { get; set; }
    }
}
