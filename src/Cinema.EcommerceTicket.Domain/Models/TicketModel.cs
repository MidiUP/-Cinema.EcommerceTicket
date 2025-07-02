using Cinema.EcommerceTicket.Domain.Models.Interfaces;
using Cinema.EcommerceTicket.Domain.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Cinema.EcommerceTicket.Domain.Models;

public class TicketModel : IModelValidator
{
    /// <summary>
    /// Identificador do ticket
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    /// <summary>
    /// Identificador do filme
    /// </summary>
    public int MovieId { get; set; }

    /// <summary>
    /// Identificador do cliente
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Identificador do checkin
    /// </summary>
    public int CheckInId { get; set; }

    /// <summary>
    /// Preco do ticket
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Data de criação do ticket
    /// </summary>
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; }

    public ValidationResult Validation()
    {
        var result = new ValidationResult();

        if(MovieId <= 0 || CustomerId <= 0)
            result.AddError("O identificador do filme e do cliente não podem ser nulos ou zero.");

        if (Price <= 0)
            result.AddError("O preço do ticket não pode ser zero.");

        return result;
    }
}
