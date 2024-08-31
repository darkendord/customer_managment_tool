using Initial_API.Data;
using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository) 
        { 
            _employeeRepository = employeeRepository;
        }

        [HttpGet("GetEmployees")]
        public IEnumerable<EmployeeModel> GetEmployees()
        {
            IEnumerable<EmployeeModel> employees = _employeeRepository.GetEmployees();
            return employees;
        }

        [HttpGet("GetSingleEmployee/{employeeNumber}")]
        public EmployeeModel GetSingleEmployee(int employeeNumber)
        {
            EmployeeModel employee = _employeeRepository.GetSingleEmployee(employeeNumber);

            if (employee != null)
            {
                return employee;
            }
            throw new Exception("Failed to Get Employee");
        }

        [HttpPut("EditEmployee")]
        public IActionResult EditEmployee(EmployeeModel employee)
        {
            EmployeeModel employeeOnDb = _employeeRepository.EditEmployee(employee);
            if(_employeeRepository.SaveChanges())
            {
                return Ok(employeeOnDb);
            }
            throw new Exception("Failed to Update or edit Employee or was not found");
        }

        [HttpPost("PostEmployee")]
        public IActionResult AddEmployee(EmployeeModel employee)
        {
            EmployeeModel employeeToDb = _employeeRepository.AddEmployee(employee);

                if (_employeeRepository.SaveChanges())
                {
                    return Ok(employeeToDb);
                }
            throw new Exception("Failed to Add Employee");
        }

        [HttpDelete("DeleteEmployee/{employeeNumber}")]
        public IActionResult DeleteEmployee(int employeeNumber)
        {
            EmployeeModel employeeOnDb = (EmployeeModel)_employeeRepository.DeleteEmployee(employeeNumber);

            if (employeeOnDb != null)
            {
                _employeeRepository.RemoveEntity<EmployeeModel>(employeeOnDb);
                if (_employeeRepository.SaveChanges())
                {
                    return Ok(employeeOnDb);
                }
            }
            throw new Exception("Failed to Update or delete Employee");

        }
    }
}
