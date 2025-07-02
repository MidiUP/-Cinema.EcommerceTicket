using Cinema.EcommerceTicket.Domain.Exceptions;
using Cinema.EcommerceTicket.Domain.Infrastructure.ApiFacades;
using Cinema.EcommerceTicket.Domain.Infrastructure.Cache;
using Cinema.EcommerceTicket.Domain.Infrastructure.Repositories;
using Cinema.EcommerceTicket.Domain.Models;
using Cinema.EcommerceTicket.Domain.Models.Catalog;
using Cinema.EcommerceTicket.Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Cinema.EcommerceTicket.Domain.Services;

public class TicketService(ILogger<TicketService> logger, 
    ITicketRepository ticketRepository, 
    ICatalogApiFacade catalogApiFacade,
    ICacheRepository cacheRepository) : ITicketService
{
    private readonly ILogger<TicketService> _logger = logger;
    private readonly ITicketRepository _ticketRepository = ticketRepository;
    private readonly ICatalogApiFacade _catalogApiFacade = catalogApiFacade;
    private readonly ICacheRepository _cacheRepository = cacheRepository;

    private readonly TimeSpan DEFAULT_TIMEOUT = TimeSpan.FromSeconds(30);
    private readonly TimeSpan DEFAULT_TIME_CACHE_DETAILS_MOVIE = TimeSpan.FromHours(24);

    public async Task CreateTicketAsync(TicketModel ticketModel)
    {
        var cts = new CancellationTokenSource(DEFAULT_TIMEOUT);

        //validar se filme existe
        _ = await GetDetailsMovieAsync(ticketModel.MovieId, cts.Token)
            ?? throw new DomainException($"Filme com ID {ticketModel.MovieId} não encontrado.");

        //lógica para calcular preço seria aplicada aqui, gerando número aleatório para simular preço
        ticketModel.Price = Math.Round((decimal)(new Random().NextDouble() * 100), 2);
        ticketModel.CreatedAt = DateTime.UtcNow;

        var validationModel = ticketModel.Validation();
        if(!validationModel.IsValid)
        {
            _logger.LogWarning("Erro ao validar o ticket: {Errors}", validationModel.Errors);
            throw new ValidationException(validationModel.Errors);
        }

        await _ticketRepository.CreateTicketAsync(ticketModel, cts.Token);
    }

    public async Task<IEnumerable<TicketModel>> GetTicketsByCostumerAsync(int customerId)
    {
        var cts = new CancellationTokenSource(DEFAULT_TIMEOUT);
        return await _ticketRepository.GetTicketsByCustomerAsync(customerId, cts.Token);
    }

    public async Task<DetailsMovieModel?> GetDetailsMovieAsync(int movieId, CancellationToken? cancellationToken = default)
    {
        cancellationToken ??= new CancellationTokenSource(DEFAULT_TIMEOUT).Token;
        var cacheKey = $"MovieDetails_{movieId}";

        //obter em cache
        var existsKey = await _cacheRepository.ExistsAsync(cacheKey, (CancellationToken)cancellationToken);
        if(existsKey)
        {
            _logger.LogInformation("Existe chave para os detalhes do filme {MovieId} no cache.", movieId);
            return await _cacheRepository.GetAsync<DetailsMovieModel>(cacheKey, (CancellationToken)cancellationToken);
        }

        //buscar na api
        var detailsMovie = await _catalogApiFacade.GetDetailsMovieAsync(movieId, (CancellationToken)cancellationToken);

        //salvar cache
        await _cacheRepository.SetAsync(cacheKey, detailsMovie, DEFAULT_TIME_CACHE_DETAILS_MOVIE, (CancellationToken)cancellationToken);

        return detailsMovie;
    }
}
