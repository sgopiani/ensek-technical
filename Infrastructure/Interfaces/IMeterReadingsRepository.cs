namespace Ensek.Energy.Command.API.Infrastructure.Interfaces
{
    using Ensek.Energy.Command.API.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMeterReadingsRepository
    {
        Task<int> InsertMeterReadings(IEnumerable<MeterReading> meterReadings);
    }
}
