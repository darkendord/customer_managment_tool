using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        DataContextEF _entityFramework;
        public EmployeeRepository(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
        }
        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }
        public void AddEntity<T>(T entity)
        {
            if (entity != null)
            {
                _entityFramework.Add(entity);
            }
        }
        public void RemoveEntity<T>(T entity)
        {
            if (entity != null)
            {
                _entityFramework.Remove(entity);
            }
        }
        public IEnumerable<EmployeeModel> GetEmployees()
        {
            IEnumerable<EmployeeModel> employees = _entityFramework.Employees.ToList<EmployeeModel>();
            return employees;
        }
        public EmployeeModel GetSingleEmployee(int employeeNumber)
        {
            EmployeeModel? employee = _entityFramework.Employees.Where(emp => emp.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employee != null)
            {
                return employee;
            }
            throw new Exception("Failed to Get Employee");
        }
        public EmployeeModel EditEmployee(EmployeeModel employee)
        {
            EmployeeModel? employeeOnDb = _entityFramework.Employees.Where(emp => emp.EmployeeNumber == employee.EmployeeNumber).FirstOrDefault();

            if (employeeOnDb != null)
            {
                employeeOnDb.EmployeeLastName = employee.EmployeeLastName ?? employeeOnDb.EmployeeLastName;
                employeeOnDb.EmployeeName = employee.EmployeeName ?? employeeOnDb.EmployeeName;
                employeeOnDb.username = employee.username ?? employeeOnDb.username;
                employeeOnDb.IsActive = employee.IsActive || employeeOnDb.IsActive;
                employeeOnDb.Email = employee.Email ?? employeeOnDb.Email;
                if (SaveChanges())
                {
                    return employeeOnDb;
                }
            }
            throw new Exception("Failed to Update or edit Employee or was not found");
        }
        public EmployeeModel AddEmployee(EmployeeModel employee)
        {
            EmployeeModel employeeToDb = new EmployeeModel();


            employeeToDb.EmployeeLastName = employee.EmployeeLastName;
            employeeToDb.EmployeeName = employee.EmployeeName;
            employeeToDb.EmployeeNumber = employee.EmployeeNumber;
            employeeToDb.username = employee.username;
            employeeToDb.IsActive = employee.IsActive;
            employeeToDb.Email = employee.Email;

            AddEntity<EmployeeModel>(employeeToDb);
            if (SaveChanges())
            {
                return employeeToDb;
            }
            throw new Exception("Failed to Add Employee");
        }
        public IActionResult DeleteEmployee(int employeeNumber)
        {
            EmployeeModel? employeeOnDb = _entityFramework.Employees.Where(emp => emp.EmployeeNumber == employeeNumber).FirstOrDefault<EmployeeModel>();

            if (employeeOnDb != null)
            {
                RemoveEntity<EmployeeModel>(employeeOnDb);
                if (SaveChanges())
                {
                    return (IActionResult)employeeOnDb;
                }
            }
            throw new Exception("Failed to Update or delete Employee");

        }
    }
}
