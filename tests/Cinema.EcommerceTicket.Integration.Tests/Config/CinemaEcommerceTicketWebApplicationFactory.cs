using Cinema.EcommerceTicket.API;
using Cinema.EcommerceTicket.Domain.Models;
using Cinema.EcommerceTicket.Domain.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mongo2Go;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using StackExchange.Redis;

namespace Cinema.EcommerceTicket.Integration.Tests.Config;

public class CinemaEcommerceTicketWebApplicationFactory : WebApplicationFactory<Program>
{
    private static MongoDbRunner? _runner = MongoDbRunner.Start();
    protected IServiceScope? Scope { get; private set; }
    protected HttpClient? Client { get; private set; }

    public CinemaEcommerceTicketWebApplicationFactory()
    {
        Client = CreateClient();    
        Scope = Services.CreateScope();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ConfigureEnvironmentVariables();
        builder.ConfigureServices(ConfigureServices);

        base.ConfigureWebHost(builder);
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        //remove qualquer instâncias de mongo db
        services.RemoveAll<IMongoClient>();
        services.RemoveAll<IMongoDatabase>();

        // Remove a implementação real
        services.RemoveAll<IConnectionMultiplexer>();

        // Adiciona o mock
        var mockMultiplexer = new Mock<IConnectionMultiplexer>();
        services.AddSingleton(mockMultiplexer.Object);


        services.AddSingleton<IMongoClient>(sp =>
            new MongoClient(_runner!.ConnectionString));

        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(Constants.MongoDb.MONGODB_DATABASE_NAME);
        }); 
    }

    public static void ConfigureEnvironmentVariables()
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

    [TearDown]
    protected async Task ClearAfterEachTest()
    {
        
        var mongoDatabase = Scope!.ServiceProvider.GetRequiredService<IMongoDatabase>();
        var collection = mongoDatabase.GetCollection<TicketModel>(Constants.MongoDb.MONGODB_TICKETS_COLLECTION_NAME);
        await mongoDatabase.DropCollectionAsync(Constants.MongoDb.MONGODB_TICKETS_COLLECTION_NAME);
        await collection.DeleteManyAsync(FilterDefinition<TicketModel>.Empty);
    }

    [OneTimeTearDown]
    protected void ClearAfterAllTests()
    {
        _runner?.Dispose();
        base.Dispose();
    }
}
