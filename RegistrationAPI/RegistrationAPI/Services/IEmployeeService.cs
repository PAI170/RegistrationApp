using RegistrationAPI.Filters;
using RegistrationAPI.Models;

namespace RegistrationAPI.Services
{
    public interface IEmployeeService
    {
        IQueryable<Employee> ListEmployees(EmployeeListFilter? filter = null);
        IQueryable<Employee> SearchEmployees(string[] values);
        Task<Employee?> FindEmployee(int id);
        Task InsertEmployee(Employee entity);
        Task UpdateEmployee(Employee entity);
        Task DeleteEmployee(Employee entity);
    }
}
