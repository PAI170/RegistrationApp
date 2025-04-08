using System.Collections.Generic;

namespace RegistrationAPI.Models
{
    public class Province
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }

        // Navigation properties
        public virtual Country Country { get; set; }
        public virtual ICollection<City> Cities { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}