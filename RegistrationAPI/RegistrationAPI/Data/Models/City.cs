using System.Collections.Generic;

namespace RegistrationAPI.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProvinceId { get; set; }

        // Navigation properties
        public virtual Province Province { get; set; }
        public virtual ICollection<District> Districts { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}