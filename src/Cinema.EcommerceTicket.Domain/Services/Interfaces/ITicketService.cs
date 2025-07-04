using Cinema.EcommerceTicket.Domain.Models;
using Cinema.EcommerceTicket.Domain.Models.Catalog;

namespace Cinema.EcommerceTicket.Domain.Services.Interfaces;

/// <summary>
/// Interface para serviços de gerenciamento de tickets.
/// </summary>
/// <remarks>
/// Define operações para criação de tickets, consulta de tickets por cliente e obtenção de detalhes de filmes.
/// </remarks>
public interface ITicketService
{
    /// <summary>
    /// Cria um novo ticket.
    /// </summary>
    /// <param name="ticketModel">Objeto <see cref="TicketModel"/> representando o ticket a ser criado.</param>
    Task CreateTicketAsync(TicketModel ticketModel);

    /// <summary>
    /// Obtém todos os tickets associados a um cliente específico.
    /// </summary>
    /// <param name="customerId">Identificador do cliente.</param>
    /// <returns>Uma coleção de <see cref="TicketModel"/> pertencentes ao cliente informado.</returns>
    Task<IEnumerable<TicketModel>> GetTicketsByCostumerAsync(int customerId);

    /// <summary>
    /// Obtém os detalhes de um filme a partir do catálogo externo.
    /// </summary>
    /// <param name="movieId">Identificador do filme.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona (opcional).</param>
    /// <returns>Um <see cref="DetailsMovieModel"/> com as informações detalhadas do filme, ou <c>null</c> se não encontrado.</returns>
    Task<DetailsMovieModel?> GetDetailsMovieAsync(int movieId, CancellationToken? cancellationToken = default);
}
