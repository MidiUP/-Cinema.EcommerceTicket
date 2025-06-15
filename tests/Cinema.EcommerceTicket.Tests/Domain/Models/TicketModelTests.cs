using Cinema.EcommerceTicket.Domain.Models;

namespace Cinema.EcommerceTicket.Tests.Domain.Models;

public class TicketModelTests
{
    [Fact]
    public void Validation_ShouldBeValid_WhenAllFieldsAreValid()
    {
        // Arrange
        var model = new TicketModel
        {
            MovieId = 1,
            CustomerId = 1,
            Price = 10.0m,
            CheckInId = 0,
        };

        // Act
        var result = model.Validation();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(0, 1, 10.0)]
    [InlineData(1, 0, 10.0)]
    [InlineData(0, 0, 10.0)]
    [InlineData(-1, 1, 10.0)]
    [InlineData(1, -1, 10.0)]
    public void Validation_ShouldBeInvalid_WhenMovieIdOrCustomerIdIsZeroOrNegative(int movieId, int customerId, decimal price)
    {
        // Arrange
        var model = new TicketModel
        {
            MovieId = movieId,
            CustomerId = customerId,
            Price = price
        };

        // Act
        var result = model.Validation();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("O identificador do filme e do cliente não podem ser nulos ou zero.", result.Errors);
    }

    [Theory]
    [InlineData(1, 1, 0)]
    [InlineData(1, 1, -5)]
    public void Validation_ShouldBeInvalid_WhenPriceIsZeroOrNegative(int movieId, int customerId, decimal price)
    {
        // Arrange
        var model = new TicketModel
        {
            MovieId = movieId,
            CustomerId = customerId,
            Price = price
        };

        // Act
        var result = model.Validation();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("O preço do ticket não pode ser zero.", result.Errors);
    }

    [Fact]
    public void Validation_ShouldReturnAllErrors_WhenAllFieldsAreInvalid()
    {
        // Arrange
        var model = new TicketModel
        {
            MovieId = 0,
            CustomerId = 0,
            Price = 0
        };

        // Act
        var result = model.Validation();

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains("O identificador do filme e do cliente não podem ser nulos ou zero.", result.Errors);
        Assert.Contains("O preço do ticket não pode ser zero.", result.Errors);
    }
}
