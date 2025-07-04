using Cinema.EcommerceTicket.Domain.Models.Catalog;

namespace Cinema.EcommerceTicket.Domain.Infrastructure.ApiFacades;

/// <summary>
/// Facade para comunicação com a API de catálogo de filmes.
/// </summary>
/// <remarks>
/// Fornece métodos para obter detalhes de filmes a partir de um serviço externo de catálogo.
/// </remarks>
public interface ICatalogApiFacade
{
    /// <summary>
    /// Obtém os detalhes de um filme a partir do catálogo externo.
    /// </summary>
    /// <param name="movieId">Identificador do filme a ser consultado.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Um <see cref="DetailsMovieModel"/> contendo as informações detalhadas do filme.
    /// </returns>
    public Task<DetailsMovieModel> GetDetailsMovieAsync(int movieId, CancellationToken cancellationToken);
}
