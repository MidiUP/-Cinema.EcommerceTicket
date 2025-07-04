using System.Diagnostics.CodeAnalysis;

namespace Cinema.Events;

/// <summary>
/// Evento utilizado para solicitar a criação de um ticket de ingresso.
/// </summary>
/// <remarks>
/// Contém as informações necessárias para criar um ticket, como o identificador do filme e do cliente.
/// Este evento pode ser publicado em sistemas de mensageria para processar a criação de tickets de forma assíncrona.
/// </remarks>
[ExcludeFromCodeCoverage]
public class EcommerceCreateTicketEvent : Event
{
    /// <summary>
    /// Identificador do filme.
    /// </summary>
    public int MovieId { get; set; }

    /// <summary>
    /// Identificador do cliente.
    /// </summary>
    public int CustomerId { get; set; }
}
