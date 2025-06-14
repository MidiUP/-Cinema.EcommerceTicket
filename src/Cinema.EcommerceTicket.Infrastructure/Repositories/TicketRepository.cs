using Cinema.EcommerceTicket.Domain.Infrastructure.Repositories;
using Cinema.EcommerceTicket.Domain.Models;
using Cinema.EcommerceTicket.Domain.Shared;
using MongoDB.Driver;

namespace Cinema.EcommerceTicket.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly IMongoCollection<TicketModel> _ticketCollection;
    private readonly string COLLECTION_NAME = Constants.MongoDb.MONGODB_TICKETS_COLLECTION_NAME;

    public TicketRepository(IMongoDatabase database)
    {
        _ticketCollection = database.GetCollection<TicketModel>(COLLECTION_NAME);
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
