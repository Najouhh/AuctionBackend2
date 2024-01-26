using Microsoft.Data.SqlClient;

namespace AuktionBackend.Repository.Interfaces
{
    public interface IJensenContext
    {
        SqlConnection GetConnection();
    }
}
