using Cinema.EcommerceTicket.Domain.Models.Catalog;

namespace Cinema.EcommerceTicket.Domain.Infrastructure.ApiFacades;

public interface ICatalogApiFacade
{
    public Task<DetailsMovieModel> GetDetailsMovieAsync(int movieId, CancellationToken cancellationToken);
}
