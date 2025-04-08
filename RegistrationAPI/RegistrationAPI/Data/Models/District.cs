using System.Collections.Generic;

namespace RegistrationAPI.Models
{
    public class District
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }

        // Navigation properties
        public virtual City City { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}