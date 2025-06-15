using System.Diagnostics.CodeAnalysis;

namespace Cinema.EcommerceTicket.Domain.Dtos.Requests;

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
