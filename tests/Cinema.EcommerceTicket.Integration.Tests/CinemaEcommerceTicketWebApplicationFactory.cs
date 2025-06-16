using Cinema.EcommerceTicket.API;
using Cinema.EcommerceTicket.Domain.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mongo2Go;
using MongoDB.Driver;
using Moq;
using StackExchange.Redis;

namespace Cinema.EcommerceTicket.Integration.Tests;

public class CinemaEcommerceTicketWebApplicationFactory:WebApplicationFactory<Program>
{
    private MongoDbRunner? _runner = MongoDbRunner.Start();
    public IMongoDatabase _mongoDatabase { get; }
    private IServiceScope _serviceScope;

    public CinemaEcommerceTicketWebApplicationFactory()
    {
        _serviceScope = Services.CreateScope();
        _mongoDatabase = _serviceScope.ServiceProvider.GetRequiredService<IMongoDatabase>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ConfigureEnvironmentVariables();

        builder.ConfigureServices(services =>
        {
            
            //remove qualquer instâncias de mongo db
            services.RemoveAll<IMongoClient>();
            services.RemoveAll<IMongoDatabase>();

            // Remove a implementação real
            services.RemoveAll<IConnectionMultiplexer>();

            // Adiciona o mock
            var mockMultiplexer = new Mock<IConnectionMultiplexer>();
            services.AddSingleton<IConnectionMultiplexer>(mockMultiplexer.Object);


            services.AddSingleton<IMongoClient>(sp =>
                new MongoClient(_runner!.ConnectionString));

            services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                // Use o nome do banco igual ao de produção ou um nome de teste
                return client.GetDatabase(Constants.MongoDb.MONGODB_DATABASE_NAME);
            });
        });
        base.ConfigureWebHost(builder);
    }

    public void ConfigureEnvironmentVariables()
    {
      
        Environment.SetEnvironmentVariable("REDIS_CONNECTION_STRING", "any_database");
        Environment.SetEnvironmentVariable("RABBIMQ_HOST", "any_rabbimq_host");
        Environment.SetEnvironmentVariable("RABBIMQ_PORT", "any_rabbimq_port");
        Environment.SetEnvironmentVariable("RABBIMQ_USERNAME", "any_rabbimq_username");
        Environment.SetEnvironmentVariable("RABBIMQ_PASSWORD", "any_rabbimq_password");
        Environment.SetEnvironmentVariable("QUEUE_CREATE_ECOMMERCE_TICKET_NAME", "any_queue_create_ecommerce_ticket_name");
        Environment.SetEnvironmentVariable("MONGODB_CONNECTION_STRING", _runner!.ConnectionString);
        Environment.SetEnvironmentVariable("MONGODB_DATABASE_NAME", "any_mongodb_database_name");
        Environment.SetEnvironmentVariable("MONGODB_TICKETS_COLLECTION_NAME", "any_mongodb_tickets_collection_name");
        Environment.SetEnvironmentVariable("CATALOG_API_BASE_URL", "http://any_catalog_api_base_url/");
        Environment.SetEnvironmentVariable("REDIS_CONNECTION_STRING", "any_redis_connection_string");
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _runner?.Dispose();
    }
}
