using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace apicooperativa.Data
{
    public class MySqlContext
    {
        private readonly IConfiguration _config;

        public MySqlContext(IConfiguration config)
        {
            _config = config;
        }

        public MySqlConnection GetConnection()
        {
            var connStr = _config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connStr))
                throw new Exception("ConnectionStrings:DefaultConnection no definida");

            return new MySqlConnection(connStr);
        }
    }
}
