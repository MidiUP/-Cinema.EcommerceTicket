using System.Diagnostics.CodeAnalysis;

namespace Cinema.EcommerceTicket.Domain.Exceptions;

[ExcludeFromCodeCoverage]
public abstract class CinemaEcommerceTicketException : Exception
{
    public abstract int ERROR_CODE { get; }
    public List<string>? Errors { get; set; }

    public CinemaEcommerceTicketException(List<string> errors, string message) : base(message)
    {
        Errors = errors;
    }

    public CinemaEcommerceTicketException(List<string> errors) : this(errors, "Cinema Ecommerce Ticket exception") { }

}
