using Cinema.EcommerceTicket.Domain.Models;
using Cinema.Events;

namespace Cinema.EcommerceTicket.Domain.Mappers;

/// <summary>
/// Classe de métodos de extensão para mapear eventos de criação de ticket em modelos de domínio.
/// </summary>
public static class TicketMappers
{
    /// <summary>
    /// Converte um <see cref="EcommerceCreateTicketEvent"/> em um <see cref="TicketModel"/>.
    /// </summary>
    /// <param name="ecommerceCreateTicketEvent">Evento de criação de ticket recebido via mensageria.</param>
    /// <returns>Uma instância de <see cref="TicketModel"/> preenchida com os dados do evento.</returns>
    public static TicketModel MapToTicketModel(this EcommerceCreateTicketEvent ecommerceCreateTicketEvent)
    {
        return new TicketModel
        {
            MovieId = ecommerceCreateTicketEvent.MovieId,
            CustomerId = ecommerceCreateTicketEvent.CustomerId
        };
    }
}
