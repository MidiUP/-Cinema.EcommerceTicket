using Cinema.EcommerceTicket.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace Cinema.EcommerceTicket.Domain.Dtos.Responses;

/// <summary>
/// DTO de resposta utilizado para retornar os dados de um ticket.
/// </summary>
/// <remarks>
/// Herda todas as propriedades de <see cref="TicketModel"/>, incluindo:
/// <list type="bullet">
///   <item><description><b>Id</b>: Identificador do ticket.</description></item>
///   <item><description><b>MovieId</b>: Identificador do filme.</description></item>
///   <item><description><b>CustomerId</b>: Identificador do cliente.</description></item>
///   <item><description><b>CheckInId</b>: Identificador do check-in.</description></item>
///   <item><description><b>Price</b>: Preço do ticket.</description></item>
///   <item><description><b>CreatedAt</b>: Data de criação do ticket (UTC).</description></item>
/// </list>
/// </remarks>
[ExcludeFromCodeCoverage]
public class GetTicketResponseDto : TicketModel { }
