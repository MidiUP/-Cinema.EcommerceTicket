using Cinema.EcommerceTicket.Domain.Mappers;
using Cinema.EcommerceTicket.Domain.Models;
using Cinema.Events;

namespace Cinema.EcommerceTicket.Tests.Domain.Mappers;

public class TicketMappersTests
{
    [Fact]
    public void MapToTicketModel_ShouldMapPropertiesCorrectly()
    {
        // Arrange
        var evt = new EcommerceCreateTicketEvent
        {
            MovieId = 42,
            CustomerId = 99
        };

        // Act
        var result = evt.MapToTicketModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(evt.MovieId, result.MovieId);
        Assert.Equal(evt.CustomerId, result.CustomerId);
    }

    [Fact]
    public void MapToTicketModel_ShouldCreateNewTicketModelInstance()
    {
        // Arrange
        var evt = new EcommerceCreateTicketEvent
        {
            MovieId = 1,
            CustomerId = 2
        };

        // Act
        var result = evt.MapToTicketModel();

        // Assert
        Assert.IsType<TicketModel>(result);
        Assert.NotSame(evt, result);
    }
}
