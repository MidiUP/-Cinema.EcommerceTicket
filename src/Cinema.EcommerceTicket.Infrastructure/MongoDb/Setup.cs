using Cinema.EcommerceTicket.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Cinema.EcommerceTicket.Infrastructure.MongoDb;

public static class Setup
{
    public static void AddMongoDb(this IServiceCollection services)
    {
        AddClient(services);
        AddUnitOfWork(services);
    }

    private static void AddUnitOfWork(IServiceCollection services)
    {
        services.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();
    }

    private static void AddClient(IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(sp =>
            new MongoClient(Constants.MongoDb.MONGODB_CONNECTION_STRING));

        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(Constants.MongoDb.MONGODB_DATABASE_NAME);
        });
    }
}
