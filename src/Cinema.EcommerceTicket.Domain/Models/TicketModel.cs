using Cinema.EcommerceTicket.Domain.Models.Interfaces;
using Cinema.EcommerceTicket.Domain.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Cinema.EcommerceTicket.Domain.Models;

/// <summary>
/// Modelo de domínio que representa um ticket de ingresso.
/// </summary>
/// <remarks>
/// Contém as informações essenciais de um ticket, incluindo identificadores, preço, data de criação e validação dos dados.
/// </remarks>
public class TicketModel : IModelValidator
{
    /// <summary>
    /// Identificador único do ticket.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    /// <summary>
    /// Identificador do filme associado ao ticket.
    /// </summary>
    public int MovieId { get; set; }

    /// <summary>
    /// Identificador do cliente que adquiriu o ticket.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Identificador do check-in relacionado ao ticket.
    /// </summary>
    public int CheckInId { get; set; }

    /// <summary>
    /// Preço do ticket.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Data e hora de criação do ticket (em UTC).
    /// </summary>
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Valida o modelo de ticket, garantindo que os campos obrigatórios estejam preenchidos corretamente.
    /// </summary>
    /// <returns>
    /// Um <see cref="ValidationResult"/> indicando se o modelo é válido e contendo mensagens de erro, se houver.
    /// </returns>
    public ValidationResult Validation()
    {
        var result = new ValidationResult();

        if (MovieId <= 0 || CustomerId <= 0)
            result.AddError("O identificador do filme e do cliente não podem ser nulos ou zero.");

        if (Price <= 0)
            result.AddError("O preço do ticket não pode ser zero.");

        return result;
    }
}
