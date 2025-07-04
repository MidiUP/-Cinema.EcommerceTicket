using Cinema.EcommerceTicket.Domain.Exceptions;
using Cinema.EcommerceTicket.Domain.Infrastructure.ApiFacades;
using Cinema.EcommerceTicket.Domain.Infrastructure.Cache;
using Cinema.EcommerceTicket.Domain.Infrastructure.Repositories;
using Cinema.EcommerceTicket.Domain.Models;
using Cinema.EcommerceTicket.Domain.Models.Catalog;
using Cinema.EcommerceTicket.Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Cinema.EcommerceTicket.Domain.Services;

/// <summary>
/// Serviço responsável pelo gerenciamento de tickets, incluindo criação, consulta e obtenção de detalhes de filmes.
/// </summary>
/// <remarks>
/// Implementa as regras de negócio para manipulação de tickets, validação de dados, integração com repositórios, cache e API de catálogo.
/// </remarks>
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

    /// <summary>
    /// Cria um novo ticket após validar o filme, calcular o preço e validar o modelo.
    /// </summary>
    /// <param name="ticketModel">Objeto <see cref="TicketModel"/> representando o ticket a ser criado.</param>
    /// <exception cref="DomainException">Lançada se o filme não for encontrado.</exception>
    /// <exception cref="ValidationException">Lançada se o modelo do ticket for inválido.</exception>
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
        if (!validationModel.IsValid)
        {
            _logger.LogWarning("Erro ao validar o ticket: {Errors}", validationModel.Errors);
            throw new ValidationException(validationModel.Errors);
        }

        await _ticketRepository.CreateTicketAsync(ticketModel, cts.Token);
    }

    /// <summary>
    /// Obtém todos os tickets associados a um cliente específico.
    /// </summary>
    /// <param name="customerId">Identificador do cliente.</param>
    /// <returns>Uma coleção de <see cref="TicketModel"/> pertencentes ao cliente informado.</returns>
    public async Task<IEnumerable<TicketModel>> GetTicketsByCostumerAsync(int customerId)
    {
        var cts = new CancellationTokenSource(DEFAULT_TIMEOUT);
        return await _ticketRepository.GetTicketsByCustomerAsync(customerId, cts.Token);
    }

    /// <summary>
    /// Obtém os detalhes de um filme a partir do cache ou da API de catálogo, armazenando em cache se necessário.
    /// </summary>
    /// <param name="movieId">Identificador do filme.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona (opcional).</param>
    /// <returns>Um <see cref="DetailsMovieModel"/> com as informações detalhadas do filme, ou <c>null</c> se não encontrado.</returns>
    public async Task<DetailsMovieModel?> GetDetailsMovieAsync(int movieId, CancellationToken? cancellationToken = default)
    {
        cancellationToken ??= new CancellationTokenSource(DEFAULT_TIMEOUT).Token;
        var cacheKey = $"MovieDetails_{movieId}";

        //obter em cache
        var existsKey = await _cacheRepository.ExistsAsync(cacheKey, (CancellationToken)cancellationToken);
        if (existsKey)
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
