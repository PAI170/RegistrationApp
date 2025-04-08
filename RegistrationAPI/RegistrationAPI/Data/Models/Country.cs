using System.Collections.Generic;

namespace RegistrationAPI.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }

        // Navigation properties
        public virtual ICollection<Province> Provinces { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}