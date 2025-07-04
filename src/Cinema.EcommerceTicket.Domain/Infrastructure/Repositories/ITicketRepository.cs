using Cinema.EcommerceTicket.Domain.Models;

namespace Cinema.EcommerceTicket.Domain.Infrastructure.Repositories;

/// <summary>
/// Interface para operações de persistência de tickets no repositório.
/// </summary>
/// <remarks>
/// Define métodos para criar e consultar tickets associados a clientes.
/// </remarks>
public interface ITicketRepository
{
    /// <summary>
    /// Cria um novo ticket no repositório.
    /// </summary>
    /// <param name="ticketModel">Objeto <see cref="TicketModel"/> representando o ticket a ser criado.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    Task CreateTicketAsync(TicketModel ticketModel, CancellationToken cancellationToken);

    /// <summary>
    /// Obtém todos os tickets associados a um cliente específico.
    /// </summary>
    /// <param name="customerId">Identificador do cliente.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>Uma coleção de <see cref="TicketModel"/> pertencentes ao cliente informado.</returns>
    Task<IEnumerable<TicketModel>> GetTicketsByCustomerAsync(int customerId, CancellationToken cancellationToken);
}
