using Cinema.EcommerceTicket.Domain.Mappers;
using Cinema.EcommerceTicket.Domain.Services.Interfaces;
using Cinema.Events;
using MassTransit;

namespace Cinema.EcommerceTicket.Infrastructure.RabbitMq.Consumers;

public class CreateTicketConsumer(ITicketService ecommerceTicketService) : IConsumer<EcommerceCreateTicketEvent>
{
    private readonly ITicketService _ecommerceTicketService = ecommerceTicketService;

    public async Task Consume(ConsumeContext<EcommerceCreateTicketEvent> context)
    {
        var message = context.Message;
        var ticketModel = message.MapToTicketModel();

        await _ecommerceTicketService.CreateTicketAsync(ticketModel);
    }
}
