using Initial_API.Data;

namespace Initial_API.Services
{
    public interface IApiServices
    {
        public DataContextEF EntityFramework(IConfiguration config);
    }
}
