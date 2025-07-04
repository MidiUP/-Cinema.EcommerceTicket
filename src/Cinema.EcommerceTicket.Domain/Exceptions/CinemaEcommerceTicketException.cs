using System.Diagnostics.CodeAnalysis;

namespace Cinema.EcommerceTicket.Domain.Exceptions;

/// <summary>
/// Classe base abstrata para exceções específicas do domínio Cinema Ecommerce Ticket.
/// </summary>
/// <remarks>
/// Todas as exceções de domínio da aplicação devem herdar desta classe para padronização.
/// Permite associar um código de erro específico e uma lista de mensagens de erro detalhadas.
/// </remarks>
[ExcludeFromCodeCoverage]
public abstract class CinemaEcommerceTicketException : Exception
{
    /// <summary>
    /// Código de erro específico da exceção.
    /// </summary>
    public abstract int ERROR_CODE { get; }

    /// <summary>
    /// Lista de mensagens de erro detalhadas relacionadas à exceção.
    /// </summary>
    public List<string>? Errors { get; set; }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="CinemaEcommerceTicketException"/> com uma lista de erros e uma mensagem.
    /// </summary>
    /// <param name="errors">Lista de mensagens de erro.</param>
    /// <param name="message">Mensagem descritiva da exceção.</param>
    public CinemaEcommerceTicketException(List<string> errors, string message) : base(message)
    {
        Errors = errors;
    }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="CinemaEcommerceTicketException"/> com uma lista de erros e uma mensagem padrão.
    /// </summary>
    /// <param name="errors">Lista de mensagens de erro.</param>
    public CinemaEcommerceTicketException(List<string> errors) : this(errors, "Cinema Ecommerce Ticket exception") { }
}
