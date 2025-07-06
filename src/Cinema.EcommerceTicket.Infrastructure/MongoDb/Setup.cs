using Cinema.EcommerceTicket.Domain.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;

namespace Cinema.EcommerceTicket.Infrastructure.MongoDb;

public static class Setup
{
    public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        AddClient(services, configuration);
        AddUnitOfWork(services);
    }

    private static void AddUnitOfWork(IServiceCollection services)
    {
        services.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();
    }

    private static void AddClient(IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbOptions = configuration.GetSection("MongoDb").Get<MongoDbOptions>()!;

        services.AddSingleton<IMongoClient>(sp =>
            new MongoClient(mongoDbOptions.ConnectionString))
            .AddHealthChecks()
            .AddMongoDb(
                sp => sp.GetRequiredService<IMongoDatabase>(),
                name: "mongodb",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["mongodb", "database"],
                timeout: TimeSpan.FromSeconds(2)
            );

        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoDbOptions.DatabaseName);
        });
    }
}
