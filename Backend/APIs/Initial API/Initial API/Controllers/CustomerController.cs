using Initial_API.Data;
using Initial_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("GetCustomers")]
        public IEnumerable<CustomerModel> GetCustomers()
        {
            IEnumerable<CustomerModel> Customers = _customerRepository.GetCustomers();
            return Customers;
        }

        [HttpGet("GetSingleCustomer/{identificationNumber}")]
        public CustomerModel GetSingleCustomer(int identificationNumber)
        {
            CustomerModel Customer = _customerRepository.GetSingleCustomer(identificationNumber);
            if (Customer != null)
            {
                return Customer;
            }
            throw new Exception("Failed to Get Customer");
        }

        [HttpPut("EditCustomer")]
        public IActionResult EditCustomer(CustomerModel customer)
        {
            CustomerModel CustomerOnDb = _customerRepository.EditCustomer(customer);

          if (_customerRepository.SaveChanges())
            {
                return Ok(CustomerOnDb);
            }
            throw new Exception("Failed to Update or edit Customer or was not found");
        }

        [HttpPost("PostCustomer")]
        public IActionResult AddCustomer(CustomerModel customer)
        {
            IActionResult customerToDb = _customerRepository.AddCustomer(customer);
            if (_customerRepository.SaveChanges())
            {
                return Ok(customerToDb);
            }
            throw new Exception("Failed to Add Customer");
        }

        [HttpDelete("DeleteCustomer/{identificationNumber}")]
        public IActionResult DeleteCustomer(int identificationNumber)
        {
            IActionResult customerOnDb = _customerRepository.DeleteCustomer(identificationNumber);
            if (customerOnDb != null)
            {
                return Ok(customerOnDb);
            }
            throw new Exception("Failed to Update or delete Customer");
        }
    }
}
