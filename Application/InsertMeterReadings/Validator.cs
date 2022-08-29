namespace Ensek.Energy.Command.API.Application.InsertMeterReadings
{
    using Ensek.Energy.Command.Model;
    using FluentValidation;
    public class MeterReadingValidator : AbstractValidator<MeterReading>
    {
        public MeterReadingValidator()
        {
            RuleFor(x => x.AccountId).NotNull();
            RuleFor(x => x.MeterReadingDateTime).NotNull();
            RuleFor(x => x.MeterReadValue)
                .NotEmpty()
                .Matches("^\\d{5}$");
        }
    }
}
