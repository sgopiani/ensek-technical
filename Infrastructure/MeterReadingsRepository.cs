namespace Ensek.Energy.Command.Infrastructure
{
    using Dapper;
    using Ensek.Energy.Command.Infrastructure.Interfaces;
    using Ensek.Energy.Command.Model;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public class MeterReadingsRepository : IMeterReadingsRepository
    {
        private const string INSERT_METER_READINGS = "usp_Ensek_Readings_InsertMeterReadings";
       
        public async Task<int> InsertMeterReadings(IEnumerable<MeterReading> meterReadings)
        {
            var meterReadingDataTable = meterReadings;
            using (IDbConnection db = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=EnsekMeterReading;Trusted_Connection=True;"))
            {
                db.Open();
                var result = db.QuerySingle<int>(INSERT_METER_READINGS, meterReadingDataTable, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
