using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidJones.Core.Contexts;

public interface IDapperContext
{
    SqlConnection CreateConnection();
}
public class DapperContext : IDapperContext, IDisposable
{
    private readonly string connectionString;
    private SqlConnection _connection = new SqlConnection();

    public DapperContext()
    {
        connectionString = "Data Source = ARSHAMDESK\\MSSQLDEV; Initial catalog = DavidJonsSyncer; TrustServerCertificate = True; User Id = sa; Password = 1234;";
    }
    public SqlConnection CreateConnection()
    {
        _connection = new SqlConnection(connectionString);
        return _connection;
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}