using AutoFixture;
using Cinema.EcommerceTicket.Domain.Exceptions;
using Cinema.EcommerceTicket.Domain.Infrastructure.ApiFacades;
using Cinema.EcommerceTicket.Domain.Infrastructure.Cache;
using Cinema.EcommerceTicket.Domain.Infrastructure.Repositories;
using Cinema.EcommerceTicket.Domain.Models;
using Cinema.EcommerceTicket.Domain.Models.Catalog;
using Cinema.EcommerceTicket.Domain.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Core.Misc;
using Moq;

namespace Cinema.EcommerceTicket.Tests.Domain.Services;

public class TicketServiceTests
{
    private readonly Mock<ILogger<TicketService>> _loggerMock = new();
    private readonly Mock<ITicketRepository> _ticketRepositoryMock = new();
    private readonly Mock<ICatalogApiFacade> _catalogApiFacadeMock = new();
    private readonly Mock<ICacheRepository> _cacheRepositoryMock = new();

    private readonly Fixture _fixture = new Fixture();

    private TicketService CreateService() =>
        new(_loggerMock.Object, _ticketRepositoryMock.Object, _catalogApiFacadeMock.Object, _cacheRepositoryMock.Object);

    [Fact]
    public async Task CreateTicketAsync_ShouldCreate_WhenTicketIsValidAndMovieExists()
    {
        // Arrange
        var ticket = new TicketModel { MovieId = 1, CustomerId = 1, Price = 10m };
        var detailsMovie = _fixture.Create<DetailsMovieModel>();
        _cacheRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _catalogApiFacadeMock.Setup(x => x.GetDetailsMovieAsync(ticket.MovieId, It.IsAny<CancellationToken>())).ReturnsAsync(detailsMovie);
        _ticketRepositoryMock.Setup(x => x.CreateTicketAsync(It.IsAny<TicketModel>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var service = CreateService();

        // Act
        await service.CreateTicketAsync(ticket);

        // Assert
        _ticketRepositoryMock.Verify(x => x.CreateTicketAsync(It.IsAny<TicketModel>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateTicketAsync_ShouldThrowValidationException_WhenTicketIsInvalid()
    {
        // Arrange
        var ticket = new TicketModel { MovieId = 0, CustomerId = 0, Price = 0 };
        var detailsMovie = new DetailsMovieModel();
        _cacheRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _catalogApiFacadeMock.Setup(x => x.GetDetailsMovieAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(detailsMovie);

        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => service.CreateTicketAsync(ticket));
        _ticketRepositoryMock.Verify(x => x.CreateTicketAsync(It.IsAny<TicketModel>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateTicketAsync_ShouldThrowValidationException_WhenMovieNotFound()
    {
        // Arrange
        var ticket = new TicketModel { MovieId = 1, CustomerId = 1, Price = 10m };
        _cacheRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _catalogApiFacadeMock.Setup(x => x.GetDetailsMovieAsync(ticket.MovieId, It.IsAny<CancellationToken>())).ReturnsAsync((DetailsMovieModel?)null);

        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => service.CreateTicketAsync(ticket));
        _ticketRepositoryMock.Verify(x => x.CreateTicketAsync(It.IsAny<TicketModel>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetTicketsByCostumerAsync_ShouldReturnTickets()
    {
        // Arrange
        var tickets = new List<TicketModel> { new TicketModel { MovieId = 1, CustomerId = 1, Price = 10m } };
        _ticketRepositoryMock.Setup(x => x.GetTicketsByCustomerAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(tickets);

        var service = CreateService();

        // Act
        var result = await service.GetTicketsByCostumerAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetDetailsMovieAsync_ShouldReturnFromCache_WhenExists()
    {
        // Arrange
        var movieId = 1;
        var detailsMovie = new DetailsMovieModel { Id = movieId };
        _cacheRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _cacheRepositoryMock.Setup(x => x.GetAsync<DetailsMovieModel>(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(detailsMovie);

        var service = CreateService();

        // Act
        var result = await service.GetDetailsMovieAsync(movieId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(movieId, result.Id);
        _catalogApiFacadeMock.Verify(x => x.GetDetailsMovieAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetDetailsMovieAsync_ShouldFetchAndCache_WhenNotInCache()
    {
        // Arrange
        var movieId = 2;
        var detailsMovie = new DetailsMovieModel { Id = movieId };
        _cacheRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _catalogApiFacadeMock.Setup(x => x.GetDetailsMovieAsync(movieId, It.IsAny<CancellationToken>())).ReturnsAsync(detailsMovie);

        var service = CreateService();

        // Act
        var result = await service.GetDetailsMovieAsync(movieId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(movieId, result.Id);
        _cacheRepositoryMock.Verify(x => x.SetAsync(It.IsAny<string>(), detailsMovie, It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
