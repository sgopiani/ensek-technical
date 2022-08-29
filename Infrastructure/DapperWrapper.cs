namespace Ensek.Energy.Command.API.Infrastructure
{
    using Dapper;
    using Ensek.Energy.Command.API.Infrastructure.Interfaces;
    using System.Data;
    using System.Threading.Tasks;

    public class DapperWrapper : IDapperWrapper
    {
        public Task<T> QuerySingleAsync<T>(IDbConnection cnn, string sql, object param = null, CommandType? commandType = null)
        {
            return cnn.QuerySingleAsync<T>(sql, param, commandType: commandType);
        }
    }
}
