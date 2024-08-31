using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Data
{
    public interface IEmployeeRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entity);
        public void RemoveEntity<T>(T entity);
        public IEnumerable<EmployeeModel> GetEmployees();
        public EmployeeModel EditEmployee(EmployeeModel employee);
        public EmployeeModel AddEmployee(EmployeeModel employee);
        public EmployeeModel GetSingleEmployee(int employeeNumber);
        public IActionResult DeleteEmployee(int employeeNumber);
    }
}
