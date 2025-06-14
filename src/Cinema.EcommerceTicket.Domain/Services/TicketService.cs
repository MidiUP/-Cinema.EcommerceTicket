using Cinema.EcommerceTicket.Domain.Exceptions;
using Cinema.EcommerceTicket.Domain.Infrastructure.ApiFacades;
using Cinema.EcommerceTicket.Domain.Infrastructure.Repositories;
using Cinema.EcommerceTicket.Domain.Models;
using Cinema.EcommerceTicket.Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Cinema.EcommerceTicket.Domain.Services;

public class TicketService(ILogger<TicketService> logger, 
    ITicketRepository ticketRepository, 
    ICatalogApiFacade catalogApiFacade) : ITicketService
{
    private readonly ILogger<TicketService> _logger = logger;
    private readonly ITicketRepository _ticketRepository = ticketRepository;
    private readonly ICatalogApiFacade _catalogApiFacade = catalogApiFacade;

    private readonly TimeSpan DEFAULT_TIMEOUT = TimeSpan.FromSeconds(30);

    public async Task CreateTicketAsync(TicketModel ticketModel)
    {
        var cts = new CancellationTokenSource(DEFAULT_TIMEOUT);

        //validar se filme existe
        _ = await _catalogApiFacade.GetDetailsMovieAsync(ticketModel.MovieId, cts.Token)
            ?? throw new ValidationException($"Filme com ID {ticketModel.MovieId} não encontrado.");

        //lógica para calcular preço seria aplicada aqui, gerando número aleatório para simular preço
        ticketModel.Price = Math.Round((decimal)(new Random().NextDouble() * 100), 2);
        ticketModel.CreatedAt = DateTime.Now;

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
}
