using Initial_API.Data;
using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        DataContextEF _entityFramework;
        public CustomerController(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
        }

        [HttpGet("GetCustomers")]
        public IEnumerable<CustomerModel> GetCustomers()
        {
            IEnumerable<CustomerModel> Customers = _entityFramework.Customers.ToList<CustomerModel>();
            return Customers;
        }

        [HttpGet("GetSingleCustomer/{identificationNumber}")]
        public CustomerModel GetSingleCustomer(int identificationNumber)
        {
            CustomerModel? Customer = _entityFramework.Customers.Where(customer => customer.IdentificationNumber == identificationNumber).FirstOrDefault();

            if (Customer != null)
            {
                return Customer;
            }
            throw new Exception("Failed to Get Customer");
        }

        [HttpPut("EditCustomer")]
        public IActionResult EditCustomer(CustomerModel customer)
        {
            CustomerModel? CustomerOnDb = _entityFramework.Customers.Where(customer => customer.IdentificationNumber == customer.IdentificationNumber).FirstOrDefault();

            if (CustomerOnDb != null)
            {
                CustomerOnDb.Name = customer.Name ?? CustomerOnDb.Name ;
                CustomerOnDb.LastNamer = customer.LastNamer ?? CustomerOnDb.LastNamer ;
                CustomerOnDb.Email = customer.Email ?? CustomerOnDb.Email ;
                CustomerOnDb.Phone = customer.Phone ?? CustomerOnDb.Phone ;
                CustomerOnDb.Adress = customer.Adress ?? CustomerOnDb.Adress ;
                CustomerOnDb.TypeOfCustomer = customer.TypeOfCustomer ?? CustomerOnDb.TypeOfCustomer ;
                CustomerOnDb.IsCustomerActicve = customer.IsCustomerActicve || CustomerOnDb.IsCustomerActicve;
                if (_entityFramework.SaveChanges() > 0)
                {
                    return Ok(CustomerOnDb);
                }
            }
            throw new Exception("Failed to Update or edit Customer or was not found");
        }

        [HttpPost("PostCustomer")]
        public IActionResult AddCustomer(CustomerModel customer)
        {
            CustomerModel CustomerToDb = new CustomerModel();


            CustomerToDb.Name = customer.Name;
            CustomerToDb.LastNamer = customer.LastNamer;
            CustomerToDb.Email = customer.Email;
            CustomerToDb.Phone = customer.Phone;
            CustomerToDb.Adress = customer.Adress;
            CustomerToDb.TypeOfCustomer = customer.TypeOfCustomer;
            CustomerToDb.IsCustomerActicve = customer.IsCustomerActicve;
            CustomerToDb.IdentificationNumber = customer.IdentificationNumber;
            _entityFramework.Add(CustomerToDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok(CustomerToDb);
            }
            throw new Exception("Failed to Add Customer");
        }

        [HttpDelete("DeleteCustomer/{identificationNumber}")]
        public IActionResult DeleteCustomer(int identificationNumber)
        {
            CustomerModel? CustomerOnDb = _entityFramework.Customers.Where(customer => customer.IdentificationNumber == identificationNumber).FirstOrDefault<CustomerModel>();

            if (CustomerOnDb != null)
            {
                _entityFramework.Customers.Remove(CustomerOnDb);
                if (_entityFramework.SaveChanges() > 0)
                {
                    return Ok(CustomerOnDb);
                }
            }
            throw new Exception("Failed to Update or delete Customer");

        }
    }
}
