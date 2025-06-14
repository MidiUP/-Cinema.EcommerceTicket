using MongoDB.Driver;

namespace Cinema.EcommerceTicket.Infrastructure.MongoDb
{
    public interface IMongoUnitOfWork
    {
        IClientSessionHandle Session { get; }
        Task StartTransactionAsync();
        Task CommitAsync();
        Task AbortAsync();
    }
}
