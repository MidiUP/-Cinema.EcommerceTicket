using Cinema.EcommerceTicket.API;
using Cinema.EcommerceTicket.Domain.Models;
using Cinema.EcommerceTicket.Domain.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
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
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        Client = CreateClient();    
        Scope = Services.CreateScope();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
        });

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
            return client.GetDatabase("any_databaseName");
        }); 
    }

    [TearDown]
    protected async Task ClearAfterEachTest()
    {
        
        var mongoDatabase = Scope!.ServiceProvider.GetRequiredService<IMongoDatabase>();
        var collection = mongoDatabase.GetCollection<TicketModel>("any_ticketsCollectionName");
        await mongoDatabase.DropCollectionAsync("any_ticketsCollectionName");
        await collection.DeleteManyAsync(FilterDefinition<TicketModel>.Empty);
    }

    [OneTimeTearDown]
    protected void ClearAfterAllTests()
    {
        _runner?.Dispose();
        base.Dispose();
    }
}
