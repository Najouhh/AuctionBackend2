using AuktionBackend.Repository.Interfaces;
using Microsoft.Data.SqlClient;

namespace AuktionBackend.Repository
{
    public class JensenContext :IJensenContext
    {
        private readonly string? _connString;
        public JensenContext(IConfiguration config)
        {
            _connString = config.GetConnectionString("JENSEN_DB");
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connString);
        }
    }
}
