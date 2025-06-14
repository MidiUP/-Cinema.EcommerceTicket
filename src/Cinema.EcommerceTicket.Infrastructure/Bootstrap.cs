using Cinema.EcommerceTicket.Domain.Infrastructure.ApiFacades;
using Cinema.EcommerceTicket.Domain.Infrastructure.Repositories;
using Cinema.EcommerceTicket.Infrastructure.ApiFacades;
using Cinema.EcommerceTicket.Infrastructure.HttpClients;
using Cinema.EcommerceTicket.Infrastructure.MongoDb;
using Cinema.EcommerceTicket.Infrastructure.RabbitMq;
using Cinema.EcommerceTicket.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Cinema.EcommerceTicket.Infrastructure;

public static class Bootstrap
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddRepositories();
        services.AddMongoDb();
        services.AddRabbitMq();
        services.AddHttpClients();
        services.AddApiFacades();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITicketRepository, TicketRepository>();
    }

    private static void AddApiFacades(this IServiceCollection services)
    {
        services.AddScoped<ICatalogApiFacade, CatalogApiFacade>();
    }
}
