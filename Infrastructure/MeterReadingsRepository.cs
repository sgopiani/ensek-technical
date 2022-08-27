namespace Ensek.Energy.Command.Infrastructure
{
    using Ensek.Energy.Command.Application.InsertMeterReadings;
    using Ensek.Energy.Command.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MeterReadingsRepository : IMeterReadingsRepository
    {
        public MeterReadingsRepository()
        {

        }
        public async Task<int> InsertMeterReadings(IEnumerable<MeterReading> meterReadings)
        {
            throw new NotImplementedException();
        }
    }
}
