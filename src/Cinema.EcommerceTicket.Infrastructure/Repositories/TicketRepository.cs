using Cinema.EcommerceTicket.Domain.Infrastructure.Repositories;
using Cinema.EcommerceTicket.Domain.Models;
using Cinema.EcommerceTicket.Domain.Shared;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Cinema.EcommerceTicket.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly IMongoCollection<TicketModel> _ticketCollection;
    private readonly MongoDbOptions _mongoDbOptions;

    public TicketRepository(IMongoDatabase database, IOptions<MongoDbOptions> mongoDbOptions)
    {
        _mongoDbOptions = mongoDbOptions.Value;
        _ticketCollection = database.GetCollection<TicketModel>(_mongoDbOptions.TicketsCollectionName);
    }
    public async Task CreateTicketAsync(TicketModel ticketModel, CancellationToken cancellationToken)
    {
        await _ticketCollection.InsertOneAsync(ticketModel);
    }

    public async Task<IEnumerable<TicketModel>> GetTicketsByCustomerAsync(int customerId, CancellationToken cancellationToken)
    {
        var filter = Builders<TicketModel>.Filter.Eq(t => t.CustomerId, customerId);
        var result = await _ticketCollection.FindAsync(filter, null, cancellationToken);
        return await result.ToListAsync(cancellationToken);
    }
}
