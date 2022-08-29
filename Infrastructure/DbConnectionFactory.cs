namespace Ensek.Energy.Command.API.Infrastructure
{
    using Ensek.Energy.Command.API.Infrastructure.Interfaces;
    using System.Data;
    using System.Data.SqlClient;
    public class DbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection GetConnection()
        {
            return new SqlConnection("Server=localhost\\SQLEXPRESS;Database=EnsekMeterReading;Trusted_Connection=True;");
        }
    }
}
