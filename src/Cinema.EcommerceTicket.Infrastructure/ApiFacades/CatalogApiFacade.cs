using Cinema.EcommerceTicket.Domain.Infrastructure.ApiFacades;
using Cinema.EcommerceTicket.Domain.Models.Catalog;
using Cinema.EcommerceTicket.Domain.Shared;
using Cinema.EcommerceTicket.Infrastructure.HttpClients;

namespace Cinema.EcommerceTicket.Infrastructure.ApiFacades;

class CatalogApiFacade(IHttpClientFactory httpClientFactory) : ICatalogApiFacade
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Constants.CatalogApi.NAME);

    public async Task<DetailsMovieModel> GetDetailsMovieAsync(int movieId, CancellationToken cancellationToken)
    {
        var path = $"v1/movies/{movieId}";
        return await _httpClient.GetAsync<DetailsMovieModel>(path, cancellationToken);
    }
}
