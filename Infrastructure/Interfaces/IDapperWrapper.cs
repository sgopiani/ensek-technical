namespace Ensek.Energy.Command.API.Infrastructure.Interfaces
{
    using System.Data;
    using System.Threading.Tasks;

    public interface IDapperWrapper
    {
        Task<T> QuerySingleAsync<T>(IDbConnection cnn, string sql, object param = null, CommandType? commandType = null);
    }
}
