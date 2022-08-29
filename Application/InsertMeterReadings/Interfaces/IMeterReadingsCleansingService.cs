namespace Ensek.Energy.Command.API.Application.InsertMeterReadings.Interfaces
{
    using Ensek.Energy.Command.API.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IMeterReadingsCleansingService
    {
        Task<IEnumerable<MeterReading>> Cleanse(InsertMeterReadings.Request request);
    }
}
