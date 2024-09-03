using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Initial_API.Data
{
    public class DataContextDapper
    {
        private readonly IConfiguration _configuration;
        public DataContextDapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnectionString"));
        }

        public IEnumerable<T> LoadData<T>(string sql)
        {
            IDbConnection dbConnection = CreateConnection();
            return dbConnection.Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            IDbConnection dbConnection = CreateConnection();
            return dbConnection.QuerySingle<T>(sql);
        }
        public bool ExecuteSql<T>(string sql)
        {
            IDbConnection dbConnection = CreateConnection();
            return dbConnection.Execute(sql) > 0;
        }
        public int ExecuteSqlWithRowCount<T>(string sql)
        {
            IDbConnection dbConnection = CreateConnection();
            return dbConnection.Execute(sql);
        }
        public bool ExecuteSqlWithParameters(string sql, List<SqlParameter> parameters)
        {
            SqlCommand commandWithParams = new SqlCommand(sql);

            foreach (SqlParameter parameter in parameters)
            {
                commandWithParams.Parameters.Add(parameter);
            }
            SqlConnection dbConnection = (SqlConnection)CreateConnection();

            dbConnection.Open();

            commandWithParams.Connection = dbConnection;

            int rowsAffected = commandWithParams.ExecuteNonQuery(); 

            dbConnection.Close(); 
            
            return rowsAffected > 0;
        }

    }
}
