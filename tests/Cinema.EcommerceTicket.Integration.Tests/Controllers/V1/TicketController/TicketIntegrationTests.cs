using AutoFixture;
using Cinema.EcommerceTicket.Domain.Dtos.Responses;
using Cinema.EcommerceTicket.Domain.Models;
using Cinema.EcommerceTicket.Domain.Shared;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Cinema.EcommerceTicket.Integration.Tests.Controllers.V1.TicketController;

public class TicketIntegrationTests(CinemaEcommerceTicketWebApplicationFactory factory) : IClassFixture<CinemaEcommerceTicketWebApplicationFactory>
{
    private readonly CinemaEcommerceTicketWebApplicationFactory _factory = factory;
    private readonly Fixture _fixture = new Fixture();

    [Fact]
    public async Task Shold_Returns200_And_Find_Ticket()
    {
        //Arrange
        _factory.ConfigureEnvironmentVariables();
        var mongoDatabase = _factory._mongoDatabase;

        var ticket = _fixture.Create<TicketModel>();
        ticket.Id = null; // Garante que o Id seja nulo para que o MongoDB gere um novo Id
        ticket.CustomerId = 1;

        // Limpa a coleção e cadastra um ticket antes do teste
        await mongoDatabase.DropCollectionAsync(Constants.MongoDb.MONGODB_TICKETS_COLLECTION_NAME);
        var collection = mongoDatabase.GetCollection<TicketModel>(Constants.MongoDb.MONGODB_TICKETS_COLLECTION_NAME);
        await collection.InsertOneAsync(ticket);

        var client = _factory.CreateClient();

        //Act
        var resultHttp = await client.GetAsync("/api/v1/tickets/1");
        var tickets = JsonConvert.DeserializeObject<IEnumerable<GetTicketResponseDto>>(await resultHttp.Content.ReadAsStringAsync());

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, resultHttp.StatusCode);
        Assert.NotNull(tickets);
        Assert.Single(tickets);
        Assert.Equal(ticket.MovieId, tickets.First().MovieId);
        Assert.Equal(ticket.CustomerId, tickets.First().CustomerId);
    }

    [Fact]
    public async Task Shold_Returns204_And_Not_Find_Ticket()
    {
        //Arrange
        _factory.ConfigureEnvironmentVariables();

        // Limpa a coleção antes do teste
        var mongoDatabase = _factory._mongoDatabase;
        await mongoDatabase.DropCollectionAsync(Constants.MongoDb.MONGODB_TICKETS_COLLECTION_NAME);

        var client = _factory.CreateClient();

        //Act
        var resultHttp = await client.GetAsync("/api/v1/tickets/1");
        var tickets = JsonConvert.DeserializeObject<IEnumerable<GetTicketResponseDto>>(await resultHttp.Content.ReadAsStringAsync());

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.NoContent, resultHttp.StatusCode);
        Assert.Null(tickets);
    }
}
