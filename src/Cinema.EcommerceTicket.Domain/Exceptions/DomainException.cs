using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Cinema.EcommerceTicket.Domain.Exceptions;

/// <summary>
/// Exceção de domínio utilizada para indicar erros de regra de negócio na aplicação.
/// </summary>
/// <remarks>
/// Lançada quando uma operação viola regras de negócio ou validações específicas do domínio.
/// Retorna o código de erro HTTP 422 (Unprocessable Entity).
/// </remarks>
[ExcludeFromCodeCoverage]
public class DomainException : CinemaEcommerceTicketException
{
    /// <summary>
    /// Mensagem padrão para exceções de domínio.
    /// </summary>
    const string ERROR_EXCEPTION_MESSAGE = "Domain Exception";

    /// <summary>
    /// Código de erro HTTP associado à exceção de domínio (422 - Unprocessable Entity).
    /// </summary>
    public override int ERROR_CODE => (int)HttpStatusCode.UnprocessableEntity;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="DomainException"/> com uma lista de erros.
    /// </summary>
    /// <param name="errors">Lista de mensagens de erro de domínio.</param>
    public DomainException(List<string> errors) : base(errors, ERROR_EXCEPTION_MESSAGE) { }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="DomainException"/> com uma mensagem de erro única.
    /// </summary>
    /// <param name="error">Mensagem de erro de domínio.</param>
    public DomainException(string error) : base([error], ERROR_EXCEPTION_MESSAGE) { }
}
