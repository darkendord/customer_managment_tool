using Initial_API.Data;
using Initial_API.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Initial_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly DataContextEF _entityFramework;
        private readonly DataContextDapper _dapper;
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config, DataContextDapper dapper) 
        {
            _entityFramework = new DataContextEF(config);
            _dapper = dapper;
            _config = config;
        }

        [HttpPost("Register")]
        public IActionResult Register(RegistrationModel newRegister)
        {
            if (newRegister.Password == newRegister.PasswordConfirm) 
            {
                string sqlRegisterExists = @$"SELECT [Email],
                                           [PasswordHash],
                                           [PasswordSalt] FROM Auth WHERE Email = '{newRegister.Email}'";

                IEnumerable<string> existingRegisters = _dapper.LoadData<string>(sqlRegisterExists);

                if(existingRegisters.Count() == 0)
                {
                    byte[] passwordSalt = new byte[128 / 8];

                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }

                    //string passwordSaltPlusString = $"{_config.GetSection("AppSettings:PasswordKey").Value}{Convert.ToBase64String(passwordSalt)}";

                    byte[] passwordHash = GetPasswordHash(newRegister.Password, passwordSalt);

                    string sqlAddAuth = @$"INSERT INTO AUTH ([Email],[PasswordHash],[PasswordSalt]) VALUES ('{newRegister.Email}', @PasswordHash, @PasswordSalt)";

                    List<SqlParameter> sqlParams = new List<SqlParameter>();

                    SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    passwordSaltParameter.Value = passwordSalt;

                    SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                    passwordHashParameter.Value = passwordHash;

                    sqlParams.Add(passwordSaltParameter);
                    sqlParams.Add(passwordHashParameter);

                    if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParams))
                    {
                        return Ok();
                    }
                    throw new Exception("Failed to register user!");
                }
                //EmployeeModel EmployeeOnDb = _entityFramework.Employees.Where(employee => employee.Email == newRegister.Email).FirstOrDefault();

                /*
                if (EmployeeOnDb == null) 
                {
                    byte[] passwordSalt = new byte[128 / 8];

                    using (var rng = RandomNumberGenerator.Create()) 
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }

                    string passwordSaltPlusString = $"{_config.GetSection("AppSettings:PasswordKey").Value}{Convert.ToBase64String(passwordSalt)}";

                    byte[] passwordHash = GetPasswordHash(newRegister.Password, passwordSalt);

                    string sqlAddAuth = @"INSERT INTO AUTH ([Email],
                                        [PasswordHash],
                                        [PasswordSalt]) VALUE ('" + newRegister.Email +
                                        "', @PasswordHash, @PasswordSalt)";

                    List<SqlParameter> sqlParams = new List<SqlParameter>();
                    SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    passwordSaltParameter.Value = passwordSalt;

                    SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                    passwordHashParameter.Value = passwordHash;

                    sqlParams.Add(passwordSaltParameter);
                    sqlParams.Add(passwordHashParameter);

                    if(_dapper.ExecuteSqlWithParameter(sqlAddAuth, sqlParams))
                    {
                        return Ok();
                    }
                    throw new Exception("Failed to register user!");
                }

                */
                throw new Exception("User with this email already exists!");
            }
            throw new Exception("Passwords do not match!");
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginModel login)
        {

            string sqlForHashAndSalt = @"SELECT [PasswordHash],
                                       [PasswordSalt] FROM Auth WHERE Email = '" + login.Email + "'";

            LoginConfirmationModel confirmation = _dapper.LoadDataSingle<LoginConfirmationModel>(sqlForHashAndSalt);

            byte[] passwordHash = GetPasswordHash(login.Password, confirmation.PasswordSalt);

            //if (passwordHash == confirmation.PasswordHash) { } won't work

            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != confirmation.PasswordHash[i])
                {
                    return StatusCode(401, "Incorrect password!");
                }
            }
            return Ok();
        }

        private byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            string passwordKey = _config["AppSettings:PasswordKey"];

            var base64Password = Convert.ToBase64String(passwordSalt);

        string passwordSaltPlusString = $"{passwordKey}{base64Password}";

            return KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
                );
        }
    }
}
