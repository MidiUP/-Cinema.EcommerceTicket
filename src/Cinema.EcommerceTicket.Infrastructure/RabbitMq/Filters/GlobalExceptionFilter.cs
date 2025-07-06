using Cinema.EcommerceTicket.Domain.Exceptions;
using MassTransit;
using Microsoft.Extensions.Logging;

public class GlobalExceptionFilter<T>(ILogger<GlobalExceptionFilter<T>> logger) : IFilter<ConsumeContext<T>> where T : class
{
    private readonly ILogger<GlobalExceptionFilter<T>> _logger = logger;

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        try
        {
            var retryCount = context.GetRetryCount();
            if (retryCount > 0) 
                _logger.LogWarning("Executando retentativa de mensagem do tipo {MessageType}. Retentativa de número {retryCount}", typeof(T).Name, retryCount + 1); //adicionando +1 por conta de comportamento do mass transit de no primeiro retry o getRetryCount trazer 0
            await next.Send(context);
        }
        catch(CinemaEcommerceTicketException ex)
        {
            _logger.LogWarning(ex, "Erro de regra de negócio ao processar mensagem do tipo {MessageType}: {Errors}", typeof(T).Name, ex.Errors);
        }catch(OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Timeout ao processar mensagem do tipo {MessageType}", typeof(T).Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro desconhecido ao processar mensagem do tipo {MessageType} haverá retentivas e em último caso mensagem irá para a deadletter", typeof(T).Name);
            throw; // Re-throw para manter o comportamento padrão (retry/dead-letter)
        }
    }

    public void Probe(ProbeContext context) { }
}
