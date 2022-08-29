namespace Ensek.Energy.Command.API.Infrastructure.Interfaces
{
    using System.Data;
    public interface IDbConnectionFactory
    {
        public IDbConnection GetConnection();
    }
}
