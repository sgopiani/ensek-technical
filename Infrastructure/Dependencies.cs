namespace Ensek.Energy.Command.Infrastructure
{
    using Ensek.Energy.Command.Infrastructure.Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    public static class Dependencies
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services
                .AddSingleton<IMeterReadingsRepository, MeterReadingsRepository>()
                ;
        }
    }
}