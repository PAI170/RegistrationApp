namespace RegistrationAPI.Filters
{
    public class EmployeeListFilter
    {
        public string? DocumentId { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDateFrom { get; set; }
        public DateTime? BirthDateTo { get; set; }
        public DateTime? JoinedDateFrom { get; set; }
        public DateTime? JoinedDateTo { get; set; }
        public int? DocumentTypeId { get; set; }
        public int? CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
    }
}
