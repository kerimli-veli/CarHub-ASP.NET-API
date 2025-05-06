using Microsoft.Data.SqlClient;


namespace DAL.SqlServer.Infrastructure;

public class BaseSqlRepository(string connectionString)
{
    private readonly string _connectionString = connectionString;

    protected SqlConnection OpenConnection()
    {
        var conn = new SqlConnection(_connectionString);
        conn.Open();
        return conn;
    }
}
