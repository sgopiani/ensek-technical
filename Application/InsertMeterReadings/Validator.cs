namespace Ensek.Energy.Command.Application.InsertMeterReadings
{
    using FluentValidation;
    public class MeterReadingValidator : AbstractValidator<MeterReading>
    {
        public MeterReadingValidator()
        {
            RuleFor(x => x.AccountId).NotNull();
            RuleFor(x => x.MeterReadingDateTime).NotNull();
            RuleFor(x => x.MeterReadValue)
                .Matches("^\\d{5}$")
                .WithMessage((reading, readingValue) => $"Invalid Meter Read Value for Account {reading.AccountId}");
        }
    }
}
