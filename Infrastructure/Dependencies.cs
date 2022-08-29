namespace Ensek.Energy.Command.API.Infrastructure
{
    using Ensek.Energy.Command.API.Infrastructure.Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    public static class Dependencies
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services
                .AddSingleton<IMeterReadingsRepository, MeterReadingsRepository>()
                ;
        }
    }
}