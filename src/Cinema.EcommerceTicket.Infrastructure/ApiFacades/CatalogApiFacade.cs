using Cinema.EcommerceTicket.Domain.Infrastructure.ApiFacades;
using Cinema.EcommerceTicket.Domain.Models.Catalog;
using Cinema.EcommerceTicket.Domain.Shared;
using Cinema.EcommerceTicket.Infrastructure.HttpClients;
using Microsoft.Extensions.Options;

namespace Cinema.EcommerceTicket.Infrastructure.ApiFacades;

public class CatalogApiFacade : ICatalogApiFacade
{
    private readonly HttpClient _httpClient;
    private readonly CatalogApiOptions _catalogApiOptions;
    public CatalogApiFacade(IHttpClientFactory httpClientFactory, IOptions<CatalogApiOptions> catalogApiOptions)
    {
        _catalogApiOptions = catalogApiOptions.Value;
        _httpClient = httpClientFactory.CreateClient(_catalogApiOptions.Name);
    }

    public async Task<DetailsMovieModel> GetDetailsMovieAsync(int movieId, CancellationToken cancellationToken)
    {
        var path = $"v1/movies/{movieId}";
        return await _httpClient.GetAsync<DetailsMovieModel>(path, cancellationToken);
    }
}
