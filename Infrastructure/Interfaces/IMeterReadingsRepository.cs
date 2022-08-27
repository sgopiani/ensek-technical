namespace Ensek.Energy.Command.Infrastructure.Interfaces
{
    using Ensek.Energy.Command.Application.InsertMeterReadings;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMeterReadingsRepository
    {
        Task<int> InsertMeterReadings(IEnumerable<MeterReading> meterReadings);
    }
}
