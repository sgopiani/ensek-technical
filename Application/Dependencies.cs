namespace Ensek.Energy.Command.Application
{
    using Ensek.Energy.Command.Application.InsertMeterReadings;
    using Ensek.Energy.Command.Application.InsertMeterReadings.Interfaces;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;

    public static class Dependencies
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return services
                .AddMediatR(assembly)
                .AddValidatorsFromAssembly(assembly)
                .AddTransient<IMeterReadingsValidationService, MeterReadingsValidationService>()
                ;
        }
    }
}
