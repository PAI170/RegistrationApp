using Microsoft.EntityFrameworkCore;
using RegistrationAPI.Data;
using RegistrationAPI.ExtensionMethods;
using RegistrationAPI.Filters;
using RegistrationAPI.Models;
using System.Linq.Expressions;

namespace RegistrationAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeesDbContext _database;

        public EmployeeService(EmployeesDbContext database)
        {
            _database = database;
        }

        public IQueryable<Employee> ListEmployees(EmployeeListFilter? filter = null)
        {
            filter ??= new EmployeeListFilter();

            return _database
                .Employees
                .Include(e => e.DocumentType)
                .Include(e => e.Country)
                .Include(e => e.Province)
                .Include(e => e.City)
                .Include(e => e.District)
                .Include(e => e.EmployeeImages.Where(i => i.IsProfilePicture))
                .Where(e => (string.IsNullOrWhiteSpace(filter.DocumentId) || e.DocumentId.Contains(filter.DocumentId))
                    && (string.IsNullOrWhiteSpace(filter.Name) || e.Name.Contains(filter.Name))
                    && (string.IsNullOrWhiteSpace(filter.LastName) || e.LastName.Contains(filter.LastName))
                    && (!filter.BirthDateFrom.HasValue || e.BirthDay >= filter.BirthDateFrom)
                    && (!filter.BirthDateTo.HasValue || e.BirthDay <= filter.BirthDateTo)
                    && (!filter.JoinedDateFrom.HasValue || e.JoinedDate >= filter.JoinedDateFrom)
                    && (!filter.JoinedDateTo.HasValue || e.JoinedDate <= filter.JoinedDateTo)
                    && (!filter.DocumentTypeId.HasValue || e.DocumentTypeId == filter.DocumentTypeId)
                    && (!filter.CountryId.HasValue || e.CountryId == filter.CountryId)
                    && (!filter.ProvinceId.HasValue || e.ProvinceId == filter.ProvinceId)
                    && (!filter.CityId.HasValue || e.CityId == filter.CityId)
                    && (!filter.DistrictId.HasValue || e.DistrictId == filter.DistrictId));
        }

        public IQueryable<Employee> SearchEmployees(string[] values)
        {
            return _database
                .Employees
                .Search(values,
                    // Lambda that receives a string value and returns a boolean lambda expression that receives an employee
                    (value) => (employee) => employee.DocumentId.Contains(value)
                        || employee.Name.Contains(value)
                        || employee.LastName.Contains(value)
                        || employee.Email.Contains(value),
                    // List of lambda expressions for ordering
                    orderBys: new List<Expression<Func<Employee, object>>>
                    {
                        employee => employee.DocumentId,
                        employee => employee.Name,
                        employee => employee.LastName
                    }
                );
        }

        public async Task<Employee?> FindEmployee(int id)
        {
            return await _database
                .Employees
                .Include(e => e.DocumentType)
                .Include(e => e.Country)
                .Include(e => e.Province)
                .Include(e => e.City)
                .Include(e => e.District)
                .Include(e => e.EmployeeImages)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task InsertEmployee(Employee entity)
        {
            _database.Employees.Add(entity);
            await _database.SaveChangesAsync();

            // Load related entities
            await _database.Entry(entity).Reference(e => e.DocumentType).LoadAsync();
            await _database.Entry(entity).Reference(e => e.Country).LoadAsync();
            await _database.Entry(entity).Reference(e => e.Province).LoadAsync();
            await _database.Entry(entity).Reference(e => e.City).LoadAsync();
            await _database.Entry(entity).Reference(e => e.District).LoadAsync();
        }

        public async Task UpdateEmployee(Employee entity)
        {
            _database.Employees.Update(entity);
            await _database.SaveChangesAsync();

            // Load related entities
            await _database.Entry(entity).Reference(e => e.DocumentType).LoadAsync();
            await _database.Entry(entity).Reference(e => e.Country).LoadAsync();
            await _database.Entry(entity).Reference(e => e.Province).LoadAsync();
            await _database.Entry(entity).Reference(e => e.City).LoadAsync();
            await _database.Entry(entity).Reference(e => e.District).LoadAsync();
        }

        public async Task DeleteEmployee(Employee entity)
        {
            _database.Employees.Remove(entity);
            await _database.SaveChangesAsync();
        }
    }
}
