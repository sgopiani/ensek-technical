namespace Ensek.Energy.Command.API.Mappers
{
    using CsvHelper;
    using CsvHelper.Configuration;
    using CsvHelper.TypeConversion;
    using Ensek.Energy.Command.Application.InsertMeterReadings;
    using System;

    public class MeterReadingCsvMapper : ClassMap<MeterReading>
    {
        public MeterReadingCsvMapper()
        {
            Map(x => x.AccountId);
            Map(x => x.MeterReadingDateTime).TypeConverter<DateTimeConverter>();
            Map(x => x.MeterReadValue);
        }

        public class DateTimeConverter : DefaultTypeConverter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                var parseResult =  DateTime.TryParse(text, out DateTime dateTime);

                return parseResult ? dateTime : null;
            }
        }
    }
}
