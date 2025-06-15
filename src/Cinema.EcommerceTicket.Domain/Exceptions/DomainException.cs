using System.Net;

namespace Cinema.EcommerceTicket.Domain.Exceptions;

public class DomainException : CinemaEcommerceTicketException
{
    const string ERROR_EXCEPTION_MESSAGE = "Domain Exception";
    public override int ERROR_CODE => (int)HttpStatusCode.UnprocessableEntity;
    public DomainException(List<string> errors) : base(errors, ERROR_EXCEPTION_MESSAGE) { }
    public DomainException(string error) : base([error], ERROR_EXCEPTION_MESSAGE) { }
}
