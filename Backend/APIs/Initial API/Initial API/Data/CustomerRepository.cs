using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        DataContextEF _entityFramework;
        public CustomerRepository(IConfiguration config)
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
        public IEnumerable<CustomerModel> GetCustomers()
        {
            IEnumerable<CustomerModel> Customers = _entityFramework.Customers.ToList<CustomerModel>();
            return Customers;
        }
        public CustomerModel GetSingleCustomer(int identificationNumber)
        {
            CustomerModel? Customer = _entityFramework.Customers.Where(customer => customer.IdentificationNumber == identificationNumber).FirstOrDefault();

            if (Customer != null)
            {
                return Customer;
            }
            throw new Exception("Failed to Get Customer");
        }
        public CustomerModel EditCustomer(CustomerModel customer)
        {
            CustomerModel? CustomerOnDb = _entityFramework.Customers.Where(customer => customer.IdentificationNumber == customer.IdentificationNumber).FirstOrDefault();

            if (CustomerOnDb != null)
            {
                CustomerOnDb.Name = customer.Name ?? CustomerOnDb.Name;
                CustomerOnDb.LastNamer = customer.LastNamer ?? CustomerOnDb.LastNamer;
                CustomerOnDb.Email = customer.Email ?? CustomerOnDb.Email;
                CustomerOnDb.Phone = customer.Phone ?? CustomerOnDb.Phone;
                CustomerOnDb.Adress = customer.Adress ?? CustomerOnDb.Adress;
                CustomerOnDb.TypeOfCustomer = customer.TypeOfCustomer ?? CustomerOnDb.TypeOfCustomer;
                CustomerOnDb.IsCustomerActicve = customer.IsCustomerActicve || CustomerOnDb.IsCustomerActicve;
                if (_entityFramework.SaveChanges() > 0)
                {
                    return CustomerOnDb;
                }
            }
            throw new Exception("Failed to Update or edit Customer or was not found");
        }
        public IActionResult AddCustomer(CustomerModel customer)
        {
            CustomerModel customerToDb = new CustomerModel();


            customerToDb.Name = customer.Name;
            customerToDb.LastNamer = customer.LastNamer;
            customerToDb.Email = customer.Email;
            customerToDb.Phone = customer.Phone;
            customerToDb.Adress = customer.Adress;
            customerToDb.TypeOfCustomer = customer.TypeOfCustomer;
            customerToDb.IsCustomerActicve = customer.IsCustomerActicve;
            customerToDb.IdentificationNumber = customer.IdentificationNumber;
            _entityFramework.Add(customerToDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return (IActionResult)customerToDb;
            }
            throw new Exception("Failed to Add Customer");
        }
        public IActionResult DeleteCustomer(int identificationNumber)
        {
            CustomerModel? CustomerOnDb = _entityFramework.Customers.Where(customer => customer.IdentificationNumber == identificationNumber).FirstOrDefault<CustomerModel>();

            if (CustomerOnDb != null)
            {
                _entityFramework.Customers.Remove(CustomerOnDb);
                if (_entityFramework.SaveChanges() > 0)
                {
                    return (IActionResult)CustomerOnDb;
                }
            }
            throw new Exception("Failed to Update or delete Customer");

        }
    }
}
