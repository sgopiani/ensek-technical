namespace Ensek.Energy.Command.Application
{
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    using MediatR;

    public static class Dependencies
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return services
                .AddMediatR(assembly);
        }
    }
}
