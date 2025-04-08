using System;
using System.Collections.Generic;

namespace RegistrationAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public string Email { get; set; }
        public DateTime JoinedDate { get; set; }
        public decimal CostPerHour { get; set; }
        public string IBAN { get; set; }
        public int CountryId { get; set; }
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public string StreetAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string PhonePrefix { get; set; }

        // Navigation properties
        public virtual DocumentType DocumentType { get; set; }
        public virtual Country Country { get; set; }
        public virtual Province Province { get; set; }
        public virtual City City { get; set; }
        public virtual District District { get; set; }
        public virtual ICollection<EmployeeImage> EmployeeImages { get; set; }
    }
}