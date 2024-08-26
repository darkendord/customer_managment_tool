using Initial_API.Data;
using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        DataContextEF _entityFramework;
        public EmployeeController(IConfiguration config) 
        { 
            _entityFramework = new DataContextEF(config);
        }

        [HttpGet("GetEmployees")]
        public IEnumerable<EmployeeModel> GetEmployees()
        {
            IEnumerable<EmployeeModel> employees = _entityFramework.Employees.ToList<EmployeeModel>();
            return employees;
        }

        [HttpGet("GetSingleEmployee/{employeeNumber}")]
        public EmployeeModel GetSingleEmployee(int employeeNumber)
        {
            EmployeeModel? employee = _entityFramework.Employees.Where(emp => emp.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employee != null)
            {
                return employee;
            }
            throw new Exception("Failed to Get Employee");
        }

        [HttpPut("EditEmployee")]
        public IActionResult EditEmployee(EmployeeModel employee)
        {
                EmployeeModel? employeeOnDb = _entityFramework.Employees.Where(emp => emp.EmployeeNumber == employee.EmployeeNumber).FirstOrDefault();

            if (employeeOnDb != null) 
            {
                employeeOnDb.EmployeeLastName = employee.EmployeeLastName ?? employeeOnDb.EmployeeLastName;
                employeeOnDb.EmployeeName = employee.EmployeeName ?? employeeOnDb.EmployeeName;
                employeeOnDb.username = employee.username ?? employeeOnDb.username;
                employeeOnDb.IsActive = employee.IsActive || employeeOnDb.IsActive;
                if(_entityFramework.SaveChanges() > 0)
                {
                    return Ok(employeeOnDb);
                }
            }
            throw new Exception("Failed to Update or edit Employee or was not found");
        }

        [HttpPost("PostEmployee")]
        public IActionResult AddEmployee(EmployeeModel employee)
        {
            EmployeeModel employeeToDb = new EmployeeModel();

            
                employeeToDb.EmployeeLastName = employee.EmployeeLastName;
                employeeToDb.EmployeeName = employee.EmployeeName;
                employeeToDb.EmployeeNumber = employee.EmployeeNumber;
                employeeToDb.username = employee.username;
                employeeToDb.IsActive = employee.IsActive;
                _entityFramework.Add(employeeToDb);
                if (_entityFramework.SaveChanges() > 0)
                {
                    return Ok(employeeToDb);
                }
            throw new Exception("Failed to Add Employee");
        }

        [HttpDelete("DeleteEmployee/{employeeNumber}")]
        public IActionResult DeleteEmployee(int employeeNumber)
        {
            EmployeeModel? employeeOnDb = _entityFramework.Employees.Where(emp => emp.EmployeeNumber == employeeNumber).FirstOrDefault<EmployeeModel>();

            if (employeeOnDb != null)
            {
                _entityFramework.Employees.Remove(employeeOnDb);
                if (_entityFramework.SaveChanges() > 0)
                {
                    return Ok(employeeOnDb);
                }
            }
            throw new Exception("Failed to Update or delete Employee");

        }
    }
}
