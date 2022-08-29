namespace Ensek.Energy.Command.API.Infrastructure
{
    using Dapper;
    using Ensek.Energy.Command.API.Infrastructure.Interfaces;
    using Ensek.Energy.Command.API.Model;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public class MeterReadingsRepository : IMeterReadingsRepository
    {
        private const string INSERT_METER_READINGS = "usp_Ensek_Readings_InsertMeterReadings";
        private IDbConnectionFactory _dbConnectionFactory;
        private IDapperWrapper _dapper;

        public MeterReadingsRepository(IDbConnectionFactory dbConnectionFactory,
            IDapperWrapper dapper)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _dapper = dapper;
        }

        public async Task<int> InsertMeterReadings(IEnumerable<MeterReading> meterReadings)
        {
            var result = 0;
            using (IDbConnection db = _dbConnectionFactory.GetConnection())
            {
                db.Open();
                result = await _dapper.QuerySingleAsync<int>(
                    db,
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
