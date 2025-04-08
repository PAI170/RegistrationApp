using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationAPI.DataTransferObject;
using RegistrationAPI.Filters;
using RegistrationAPI.Models;
using RegistrationAPI.Services;
using RegistrationAPI.Utils;
using RegistrationAPI.Validators;

namespace RegistrationAPI.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeValidator _employeeValidator;

        public EmployeesController(IMapper mapper, IEmployeeService employeeService, IEmployeeValidator employeeValidator)
        {
            _mapper = mapper;
            _employeeService = employeeService;
            _employeeValidator = employeeValidator;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> ListEmployees([FromQuery] FilterEmployeeDTO data)
        {
            EmployeeListFilter filter = _mapper.Map<FilterEmployeeDTO, EmployeeListFilter>(data);
            List<Employee> list = await _employeeService.ListEmployees(filter)
                                    .OrderBy(e => e.Name)
                                    .ThenBy(e => e.LastName)
                                    .ToListAsync();

            APIResponse response = new()
            {
                StatusCode = HttpStatusCode.OK,
                Data = list.Select(e => _mapper.Map<Employee, GetEmployeeDTO>(e))
            };

            return response;
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<APIResponse>> SearchEmployees([FromQuery] string[] s)
        {
            List<Employee> list = await _employeeService.SearchEmployees(s).ToListAsync();

            APIResponse response = new()
            {
                StatusCode = HttpStatusCode.OK,
                Data = list.Select(e => _mapper.Map<Employee, GetEmployeeDTO>(e))
            };

            return response;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<APIResponse>> FindEmployee(int id)
        {
            Employee? employee = await _employeeService.FindEmployee(id);
            if (employee == null)
            {
                return HttpErrors.NotFound("Empleado no existe en el sistema");
            }

            APIResponse response = new()
            {
                StatusCode = HttpStatusCode.OK,
                Data = _mapper.Map<Employee, GetEmployeeDTO>(employee)
            };

            return response;
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> InsertEmployee(InsertUpdateEmployeeDTO data)
        {
            APIResponse response = new();
            response.Success = _employeeValidator.ValidateInsert(data, response.Messages);

            if (!response.Success)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            Employee employee = _mapper.Map<InsertUpdateEmployeeDTO, Employee>(data);
            employee.JoinedDate = DateTime.Now; // Set joined date to now

            await _employeeService.InsertEmployee(employee);

            response.StatusCode = HttpStatusCode.Created;
            response.Data = _mapper.Map<Employee, GetEmployeeDTO>(employee);
            response.Messages.Add("Empleado ha sido insertado correctamente");

            return response;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<APIResponse>> UpdateEmployee(int id, InsertUpdateEmployeeDTO data)
        {
            Employee? employee = await _employeeService.FindEmployee(id);
            if (employee == null)
            {
                return HttpErrors.NotFound("Empleado no existe en el sistema");
            }

            APIResponse response = new();
            response.Success = _employeeValidator.ValidateUpdate(id, data, response.Messages);

            if (!response.Success)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            _mapper.Map(data, employee);
            await _employeeService.UpdateEmployee(employee);

            response.StatusCode = HttpStatusCode.OK;
            response.Data = _mapper.Map<Employee, GetEmployeeDTO>(employee);
            response.Messages.Add("Empleado ha sido actualizado correctamente");

            return response;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<APIResponse>> DeleteEmployee(int id)
        {
            Employee? employee = await _employeeService.FindEmployee(id);
            if (employee == null)
            {
                return HttpErrors.NotFound("Empleado no existe en el sistema");
            }

            APIResponse response = new();
            response.Success = _employeeValidator.ValidateDelete(id, response.Messages);

            if (!response.Success)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            await _employeeService.DeleteEmployee(employee);

            response.StatusCode = HttpStatusCode.OK;
            response.Data = _mapper.Map<Employee, GetEmployeeDTO>(employee);
            response.Messages.Add("Empleado ha sido eliminado correctamente");

            return response;
        }
    }
}