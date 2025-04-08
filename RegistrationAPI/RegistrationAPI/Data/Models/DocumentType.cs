using System.Collections.Generic;

namespace RegistrationAPI.Models
{
    public class DocumentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation property
        public virtual ICollection<Employee> Employees { get; set; }
    }
}