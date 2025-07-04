using Cinema.EcommerceTicket.Domain.Models.Interfaces;
using Cinema.EcommerceTicket.Domain.Shared;

namespace Cinema.EcommerceTicket.Domain.Models;

/// <summary>
/// Modelo de domínio para o check-in de um cliente em um filme.
/// </summary>
/// <remarks>
/// Representa os dados necessários para registrar um check-in, incluindo validação dos campos obrigatórios.
/// </remarks>
public class CheckInModel : IModelValidator
{
    const string MESSAGE_VALIDATION_ERROR = "O identificador do filme e do cliente não podem ser nulos ou zero.";

    /// <summary>
    /// Identificador do filme.
    /// </summary>
    public int MovieId { get; set; }

    /// <summary>
    /// Identificador do cliente.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Valida o modelo de check-in, garantindo que os identificadores do filme e do cliente sejam maiores que zero.
    /// </summary>
    /// <returns>
    /// Um <see cref="ValidationResult"/> indicando se o modelo é válido e contendo mensagens de erro, se houver.
    /// </returns>
    public ValidationResult Validation()
    {
        var result = new ValidationResult();

        if (MovieId <= 0 || CustomerId <= 0)
            result.AddError(MESSAGE_VALIDATION_ERROR);

        return result;
    }
}
