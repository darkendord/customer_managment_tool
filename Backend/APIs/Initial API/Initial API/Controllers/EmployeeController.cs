using Microsoft.AspNetCore.Mvc;

namespace Initial_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        public EmployeeController() 
        { 
        }

        [HttpGet("GetEmployees/{testValue}")]
        // public IActionResult GetEmployees
        public string[] GetEmployees(string testValue)
        {
            string[] responseArray = new string[]
            {
                "Employee 1",
                "Employee 2",
                "Employee 3"
            };
            return responseArray;
        }
    }
}
