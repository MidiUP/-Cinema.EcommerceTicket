using Cinema.EcommerceTicket.Domain.Dtos.Requests;
using Cinema.EcommerceTicket.Domain.Models;
using Cinema.EcommerceTicket.Domain.Events;

namespace Cinema.EcommerceTicket.Domain.Mappers;

public static class TicketMappers
{
    public static TicketModel MapToTicketModel(this EcommerceCreateTicketEvent ecommerceCreateTicketEvent)
    {
        return new TicketModel
        {
            MovieId = ecommerceCreateTicketEvent.MovieId,
            CustomerId = ecommerceCreateTicketEvent.CustomerId
        };
    }
}
