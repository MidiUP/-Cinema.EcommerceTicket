using Cinema.EcommerceTicket.Domain.Infrastructure.ApiFacades;
using Cinema.EcommerceTicket.Domain.Infrastructure.Cache;
using Cinema.EcommerceTicket.Domain.Infrastructure.Repositories;
using Cinema.EcommerceTicket.Infrastructure.ApiFacades;
using Cinema.EcommerceTicket.Infrastructure.HttpClients;
using Cinema.EcommerceTicket.Infrastructure.MongoDb;
using Cinema.EcommerceTicket.Infrastructure.RabbitMq;
using Cinema.EcommerceTicket.Infrastructure.Redis;
using Cinema.EcommerceTicket.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cinema.EcommerceTicket.Infrastructure;

public static class Bootstrap
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositories();
        services.AddMongoDb(configuration);
        services.AddRabbitMq(configuration);
        services.AddRedis(configuration);
        services.AddHttpClients(configuration);
        services.AddApiFacades();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<ICacheRepository, RedisRepository>();
    }

    private static void AddApiFacades(this IServiceCollection services)
    {
        services.AddScoped<ICatalogApiFacade, CatalogApiFacade>();
    }
}
