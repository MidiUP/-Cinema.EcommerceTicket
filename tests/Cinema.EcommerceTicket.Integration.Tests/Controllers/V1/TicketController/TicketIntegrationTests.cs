using AutoFixture;
using Cinema.EcommerceTicket.Domain.Dtos.Responses;
using Cinema.EcommerceTicket.Domain.Models;
using Cinema.EcommerceTicket.Domain.Shared;
using Cinema.EcommerceTicket.Integration.Tests.Config;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Cinema.EcommerceTicket.Integration.Tests.Controllers.V1.TicketController;

[TestFixture]
public class TicketIntegrationTests : CinemaEcommerceTicketWebApplicationFactory
{
    private readonly Fixture _fixture = new Fixture();

    [Test]
    public async Task Shold_Returns200_If_Find_Ticket()
    {
        //Arrange
        var mongoDatabase = Scope!.ServiceProvider.GetRequiredService<IMongoDatabase>();

        var ticket = _fixture.Create<TicketModel>();
        ticket.Id = string.Empty; // Garante que o Id seja nulo para que o MongoDB gere um novo Id
        ticket.CustomerId = 1;

        // Limpa a coleção e cadastra um ticket antes do teste
        await ClearMongoDb();
        var collection = mongoDatabase.GetCollection<TicketModel>(Constants.MongoDb.MONGODB_TICKETS_COLLECTION_NAME);
        await collection.InsertOneAsync(ticket);

        //Act
        var resultHttp = await Client!.GetAsync("/api/v1/tickets/1");
        var tickets = JsonConvert.DeserializeObject<IEnumerable<GetTicketResponseDto>>(await resultHttp.Content.ReadAsStringAsync());

        //Assert
        Assert.That(resultHttp.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        Assert.That(tickets, Is.Not.Null);
        Assert.That(tickets?.Count(), Is.EqualTo(1));
        Assert.That(tickets?.First().MovieId, Is.EqualTo(ticket.MovieId));
        Assert.That(tickets?.First().CustomerId, Is.EqualTo(ticket.CustomerId));
    }

    [Test]
    public async Task Shold_Returns204_If_Not_Find_Ticket()
    {
        //Arrange
        await ClearMongoDb();

        //Act
        var resultHttp = await Client!.GetAsync("/api/v1/tickets/1");
        var tickets = JsonConvert.DeserializeObject<IEnumerable<GetTicketResponseDto>>(await resultHttp.Content.ReadAsStringAsync());

        //Assert
        Assert.That(resultHttp.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NoContent));
        Assert.That(tickets, Is.Null);
    }

    private async Task ClearMongoDb()
    {
        var mongoDatabase = Scope!.ServiceProvider.GetRequiredService<IMongoDatabase>();
        await mongoDatabase.DropCollectionAsync(Constants.MongoDb.MONGODB_TICKETS_COLLECTION_NAME);
        var collection = mongoDatabase.GetCollection<TicketModel>(Constants.MongoDb.MONGODB_TICKETS_COLLECTION_NAME);
        await collection.DeleteManyAsync(FilterDefinition<TicketModel>.Empty);
    }
}
