using Example.Domain.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Domain;
public static class Startup
{
    public static IServiceCollection AddExampleHandlers(this IServiceCollection services)
    {
        return services.AddScoped<OrderCreateHandler>()
            .AddScoped<UserCreateHandler>()
            .AddScoped<UserUpdateHandler>();
    }
}
