namespace Ensek.Energy.Command.API.Application
{
    using Ensek.Energy.Command.API.Application.InsertMeterReadings;
    using Ensek.Energy.Command.API.Application.InsertMeterReadings.Interfaces;
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
                .AddTransient<IMeterReadingsCleansingService, MeterReadingsCleansingService>()
                ;
        }
    }
}
