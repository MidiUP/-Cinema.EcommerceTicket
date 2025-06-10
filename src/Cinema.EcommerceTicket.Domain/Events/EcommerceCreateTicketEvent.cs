﻿using Cinema.EcommerceTicket.Domain.Events;

namespace Cinema.EcommerceTicket.Domain.Events;

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
