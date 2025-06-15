using Cinema.EcommerceTicket.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Cinema.EcommerceTicket.Infrastructure.Redis;

public static class Setup
{
    public static void AddRedis(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(Constants.Redis.REDIS_CONNECTION_STRING));
    }
}
