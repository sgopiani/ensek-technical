namespace Ensek.Energy.Command.Application.InsertMeterReadings.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IMeterReadingsValidationService
    {
        Task<Tuple<IEnumerable<MeterReading>, IEnumerable<string>>> Validate(InsertMeterReadings.Request request);
    }
}
