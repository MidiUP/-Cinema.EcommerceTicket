using Cinema.EcommerceTicket.API;
using Cinema.EcommerceTicket.Domain.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mongo2Go;
using MongoDB.Driver;

namespace Cinema.EcommerceTicket.IntegrationTests;

public class CinemaEcommerceTicketWebApplicationFactory:WebApplicationFactory<Program>
{
    private MongoDbRunner? _runner;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //remove qualquer instâncias de mongo db
            {
                services.RemoveAll<IMongoClient>();
                services.RemoveAll<IMongoDatabase>();
            }

            //adiciona um mongodb em memória para os testes
            _runner = MongoDbRunner.Start();

            services.AddSingleton<IMongoClient>(sp =>
                new MongoClient(_runner.ConnectionString));

            services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                // Use o nome do banco igual ao de produção ou um nome de teste
                return client.GetDatabase(Constants.MongoDb.MONGODB_DATABASE_NAME);
            });
        });
        base.ConfigureWebHost(builder);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _runner?.Dispose();
    }
}
