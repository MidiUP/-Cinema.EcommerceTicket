using MongoDB.Driver;

namespace Cinema.EcommerceTicket.Infrastructure.MongoDb;

public class MongoUnitOfWork(IMongoClient client) : IMongoUnitOfWork, IDisposable
{
    private readonly IMongoClient _client = client;
    private IClientSessionHandle? _session;

    public IClientSessionHandle Session => _session!;

    public async Task StartTransactionAsync()
    {
        _session = await _client.StartSessionAsync();
        _session.StartTransaction();
    }

    public async Task CommitAsync()
    {
        if (_session != null && _session.IsInTransaction)
            await _session.CommitTransactionAsync();
    }

    public async Task AbortAsync()
    {
        if (_session != null && _session.IsInTransaction)
            await _session.AbortTransactionAsync();
    }

    public void Dispose()
    {
        _session?.Dispose();
    }
}
