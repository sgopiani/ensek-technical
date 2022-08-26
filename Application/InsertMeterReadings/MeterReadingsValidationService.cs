namespace Ensek.Energy.Command.Application.InsertMeterReadings
{
    using Ensek.Energy.Command.Application.InsertMeterReadings.Interfaces;
    using FluentValidation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static Ensek.Energy.Command.Application.InsertMeterReadings.InsertMeterReadings;

    public class MeterReadingsValidationService : IMeterReadingsValidationService
    {
        private IValidator<MeterReading> _meterReadingValidator;

        public MeterReadingsValidationService(
            IValidator<MeterReading> meterReadingValidator)
        {
            _meterReadingValidator = meterReadingValidator;
        }

        public async Task<Tuple<IEnumerable<MeterReading>, IEnumerable<string>>> Validate(Request request)
        {
            var meterReadingsToProcess = new List<MeterReading>();
            var meterReadingFailures = new List<string>();

            foreach (var reading in request.MeterReadings)
            {
                var result = await _meterReadingValidator.ValidateAsync(reading);

                if (result.IsValid)
                {
                    meterReadingsToProcess.Add(reading);
                }
                else
                {
                    meterReadingFailures.AddRange(result.Errors.Select(x => x.ErrorMessage));
                }
            }

            return new Tuple<IEnumerable<MeterReading>, IEnumerable<string>>(meterReadingsToProcess, meterReadingFailures);
        }
    }
}
