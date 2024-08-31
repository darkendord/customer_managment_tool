using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Data
{
    public interface ICustomerRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entity);
        public void RemoveEntity<T>(T entity);
        public IEnumerable<CustomerModel> GetCustomers();
        public CustomerModel GetSingleCustomer(int identificationNumber);
        public CustomerModel EditCustomer(CustomerModel customer);
        public IActionResult AddCustomer(CustomerModel customer);
        public IActionResult DeleteCustomer(int identificationNumber);

    }
}
