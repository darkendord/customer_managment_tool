using Initial_API.Data;

namespace Initial_API.Services
{
    public class ApiServices
    {
        DataContextEF EntityFramework(IConfiguration config)
        {
            return new DataContextEF(config);
        }
    }
}
