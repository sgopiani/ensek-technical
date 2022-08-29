namespace Ensek.Energy.Command.API.Infrastructure
{
    using Dapper;
    using Ensek.Energy.Command.API.Infrastructure.Interfaces;
    using Ensek.Energy.Command.API.Model;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public class MeterReadingsRepository : IMeterReadingsRepository
    {
        private const string INSERT_METER_READINGS = "usp_Ensek_Readings_InsertMeterReadings";

        public async Task<int> InsertMeterReadings(IEnumerable<MeterReading> meterReadings)
        {
            var result = 0;
            using (IDbConnection db = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=EnsekMeterReading;Trusted_Connection=True;"))
            {
                db.Open();
                result = await db.QuerySingleAsync<int>(
                    INSERT_METER_READINGS,
                    new
                    {
                        MeterReadings = BuilDataTableParameter(meterReadings).AsTableValuedParameter("dt_MeterReadings")
                    },
                    commandType: CommandType.StoredProcedure);
            }
            return result;

        }

        private DataTable BuilDataTableParameter(IEnumerable<MeterReading> meterReadings)
        {
            var dt = new DataTable();
            dt.Columns.Add(nameof(MeterReading.AccountId), typeof(int));
            dt.Columns.Add(nameof(MeterReading.MeterReadingDateTime), typeof(DateTime));
            dt.Columns.Add(nameof(MeterReading.MeterReadValue), typeof(string));

            foreach (var reading in meterReadings)
            {
                dt.Rows.Add(reading.AccountId, reading.MeterReadingDateTime, reading.MeterReadValue);
            }

            return dt;
        }
    }
}
