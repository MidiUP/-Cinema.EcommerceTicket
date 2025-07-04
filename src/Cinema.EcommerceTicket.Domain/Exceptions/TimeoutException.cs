using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Cinema.EcommerceTicket.Domain.Exceptions;

/// <summary>
/// Exceção de domínio utilizada para indicar que uma operação excedeu o tempo limite de execução.
/// </summary>
/// <remarks>
/// Lançada quando uma operação demora mais do que o tempo permitido para ser concluída.
/// Retorna o código de erro HTTP 408 (Request Timeout).
/// </remarks>
[ExcludeFromCodeCoverage]
public class TimeoutException : CinemaEcommerceTicketException
{
    /// <summary>
    /// Mensagem padrão para exceções de timeout.
    /// </summary>
    const string ERROR_EXCEPTION_MESSAGE = "Timeout Exception";

    /// <summary>
    /// Código de erro HTTP associado à exceção de timeout (408 - Request Timeout).
    /// </summary>
    public override int ERROR_CODE => (int)HttpStatusCode.RequestTimeout;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="TimeoutException"/> com mensagem padrão.
    /// </summary>
    public TimeoutException() : base([], ERROR_EXCEPTION_MESSAGE) { }
}
