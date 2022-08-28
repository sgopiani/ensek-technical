﻿namespace Ensek.Energy.Command.Application.InsertMeterReadings.Interfaces
{
    using Ensek.Energy.Command.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IMeterReadingsCleansingService
    {
        Task<IEnumerable<MeterReading>> Cleanse(InsertMeterReadings.Request request);
    }
}
