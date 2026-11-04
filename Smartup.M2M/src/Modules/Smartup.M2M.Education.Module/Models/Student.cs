

using Smartup.M2M.BuildingBlocks.Domains;

namespace Smartup.M2M.Education.Module.Models
{
    public class Student : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; private set; }
        public Student(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            FullName = $"{firstName} {lastName}";
        }

        
        public Student() { }
    }
}
