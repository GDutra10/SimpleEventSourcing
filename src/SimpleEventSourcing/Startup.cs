using Microsoft.Extensions.DependencyInjection;
using SimpleEventSourcing.InMemory;
using SimpleEventSourcing.Interfaces.Repositories;

namespace SimpleEventSourcing;
public static class Startup
{
    public static IServiceCollection UseEventSourcing(
        this IServiceCollection services,
        Action<EventSourcingOptions> options)
        => services
            .AddScoped(typeof(EventSource<,>))
            .ApplyConfiguration(options);

    private static IServiceCollection ApplyConfiguration(this IServiceCollection services, Action<EventSourcingOptions> options)
    {
        var configuration = new EventSourcingOptions();

        options(configuration);

        if (!configuration!.UseInMemory) 
            return services;

        services.AddScoped(typeof(IEventRepository<>), typeof(InMemoryEventRepository<>));
        services.AddScoped(typeof(IProjectionRepository<>), typeof(InMemoryProjectionRepository<>));

        return services;
    }
}
