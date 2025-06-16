using Cinema.EcommerceTicket.Domain.Dtos.Responses;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Cinema.EcommerceTicket.IntegrationTests.Controllers.V1.TicketController;

public class TicketIntegrationTests : IClassFixture<CinemaEcommerceTicketWebApplicationFactory>
{
    private readonly CinemaEcommerceTicketWebApplicationFactory _factory;
    private readonly IMongoDatabase _mongoDatabase;

    public TicketIntegrationTests(CinemaEcommerceTicketWebApplicationFactory factory)
    {
        _factory = factory;
        // Cria um escopo para resolver o IMongoDatabase
        using var scope = _factory.Services.CreateScope();
        _mongoDatabase = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
    }

    [Fact]
    public async Task Teste_Exemplo()
    {
        //Arrange

        // Limpa a coleção antes do teste
        await _mongoDatabase.DropCollectionAsync("tickets");

        var collection = _mongoDatabase.GetCollection<BsonDocument>("tickets");
        await collection.InsertOneAsync(new BsonDocument { { "MovieId", 1 }, { "CustomerId", 1 } });

        var client = _factory.CreateClient();

        //Act
        var resultHttp = await client.GetAsync("/api/v1/ticket/1");
        var tickets = JsonConvert.DeserializeObject<IEnumerable<GetTicketResponseDto>>(await resultHttp.Content.ReadAsStringAsync());

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, resultHttp.StatusCode);
        Assert.NotNull(tickets);
        Assert.Single(tickets);
        Assert.Equal(1, tickets.First().MovieId);
        Assert.Equal(1, tickets.First().CustomerId);

    }
}
