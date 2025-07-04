using System.Diagnostics.CodeAnalysis;

namespace Cinema.EcommerceTicket.Domain.Dtos.Requests;

/// <summary>
/// DTO para requisição de criação de check-in de ingresso.
/// </summary>
/// <remarks>
/// Utilizado para registrar um check-in de um cliente em um filme específico.
/// </remarks>
[ExcludeFromCodeCoverage]
public record CreateCheckInRequestDto
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
