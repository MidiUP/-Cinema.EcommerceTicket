using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Cinema.EcommerceTicket.Domain.Exceptions;

/// <summary>
/// Exceção de domínio utilizada para indicar falhas de validação de dados na aplicação.
/// </summary>
/// <remarks>
/// Lançada quando uma ou mais validações de dados de entrada não são atendidas.
/// Retorna o código de erro HTTP 400 (Bad Request).
/// </remarks>
[ExcludeFromCodeCoverage]
public class ValidationException : CinemaEcommerceTicketException
{
    /// <summary>
    /// Mensagem padrão para exceções de validação.
    /// </summary>
    const string ERROR_EXCEPTION_MESSAGE = "Validation Exception";

    /// <summary>
    /// Código de erro HTTP associado à exceção de validação (400 - Bad Request).
    /// </summary>
    public override int ERROR_CODE => (int)HttpStatusCode.BadRequest;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ValidationException"/> com uma lista de erros de validação.
    /// </summary>
    /// <param name="errors">Lista de mensagens de erro de validação.</param>
    public ValidationException(List<string> errors) : base(errors, ERROR_EXCEPTION_MESSAGE) { }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ValidationException"/> com uma mensagem de erro única.
    /// </summary>
    /// <param name="error">Mensagem de erro de validação.</param>
    public ValidationException(string error) : base([error], ERROR_EXCEPTION_MESSAGE) { }
}
