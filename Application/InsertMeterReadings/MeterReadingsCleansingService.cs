namespace Ensek.Energy.Command.Application.InsertMeterReadings
{
    using Ensek.Energy.Command.Application.InsertMeterReadings.Interfaces;
    using Ensek.Energy.Command.Model;
    using FluentValidation;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static Ensek.Energy.Command.Application.InsertMeterReadings.InsertMeterReadings;

    public class MeterReadingsCleansingService : IMeterReadingsCleansingService
    {
        private IValidator<MeterReading> _meterReadingValidator;

        public MeterReadingsCleansingService(
            IValidator<MeterReading> meterReadingValidator)
        {
            _meterReadingValidator = meterReadingValidator;
        }

        public async Task<IEnumerable<MeterReading>> Cleanse(Request request)
        {
            var meterReadings = GetLatestMeterReadingsByAccountId(request.MeterReadings);

            var meterReadingsToProcess = new List<MeterReading>();

            foreach (var reading in meterReadings)
            {
                var result = await _meterReadingValidator.ValidateAsync(reading);

                if (result.IsValid)
                {
                    meterReadingsToProcess.Add(reading);
                }
            }

            return meterReadingsToProcess;
        }

        private IEnumerable<MeterReading> GetLatestMeterReadingsByAccountId(List<MeterReading> meterReadings) =>
            meterReadings.GroupBy(r => r.AccountId).Select(grp => grp.MaxBy(y => y.MeterReadingDateTime));

    }
}
