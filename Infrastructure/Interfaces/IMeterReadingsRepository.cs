namespace Ensek.Energy.Command.Infrastructure.Interfaces
{
    using Ensek.Energy.Command.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMeterReadingsRepository
    {
        Task<int> InsertMeterReadings(IEnumerable<MeterReading> meterReadings);
    }
}
