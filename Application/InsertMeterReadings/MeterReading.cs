namespace Ensek.Energy.Command.Application.InsertMeterReadings
{
    using System;
    public class MeterReading
    {
        public int AccountId { get; set; }
        public string MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }

    }
}
