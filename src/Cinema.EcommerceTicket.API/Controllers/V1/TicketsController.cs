using Cinema.EcommerceTicket.API.Attributes;
using Cinema.EcommerceTicket.Domain.Dtos.Responses;
using Cinema.EcommerceTicket.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Cinema.EcommerceTicket.API.Controllers.V1;

[ExcludeFromCodeCoverage]
[ApiVersion("1.0")]
[CinemaEcommerceTicketVersionedRoute("1")]
public class TicketsController(ITicketService ticketService) : CinemaEcommerceTicketControllerBase
{
    public readonly ITicketService _ticketService = ticketService;

    [HttpGet("{customerId}")]
    [ProducesResponseType<IEnumerable<GetTicketResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ErrorResponseDto>(StatusCodes.Status408RequestTimeout)]
    [ProducesResponseType<ErrorResponseDto>(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CheckInTicketAsync([FromRoute] string customerId)
    {
        var response = await _ticketService.GetTicketsByCostumerAsync(int.Parse(customerId));
        
        if(response is null || !response.Any())
            return NoContent();

        return Ok(response);
    }
}
