using Cinema.EcommerceTicket.Domain.Models;
using Cinema.EcommerceTicket.Domain.Models.Catalog;

namespace Cinema.EcommerceTicket.Domain.Services.Interfaces;

public interface ITicketService
{
    Task CreateTicketAsync(TicketModel ticketModel);
    Task<IEnumerable<TicketModel>> GetTicketsByCostumerAsync(int customerId);
    Task<DetailsMovieModel?> GetDetailsMovieAsync(int movieId, CancellationToken? cancellationToken = default);
}
