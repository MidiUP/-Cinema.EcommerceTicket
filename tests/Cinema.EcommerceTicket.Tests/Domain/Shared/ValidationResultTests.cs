using Cinema.EcommerceTicket.Domain.Shared;

namespace Cinema.EcommerceTicket.Tests.Domain.Shared;

public class ValidationResultTests
{
    [Fact]
    public void ValidationResult_ShouldBeValid_WhenNoErrors()
    {
        // Arrange
        var result = new ValidationResult();

        // Act & Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ValidationResult_ShouldNotBeValid_WhenErrorIsAdded()
    {
        // Arrange
        var result = new ValidationResult();

        // Act
        result.AddError("Erro de validação");

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains("Erro de validação", result.Errors);
    }

    [Fact]
    public void ValidationResult_ConstructorWithError_ShouldAddError()
    {
        // Arrange
        var result = new ValidationResult("Erro inicial");

        // Act & Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains("Erro inicial", result.Errors);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidationResult_ConstructorWithNullOrEmptyError_ShouldThrow(string? error)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ValidationResult(error!));
    }
}
