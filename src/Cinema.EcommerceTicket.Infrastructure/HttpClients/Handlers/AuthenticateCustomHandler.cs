using Cinema.EcommerceTicket.Infrastructure.HttpClients.GatewayAdapters;

namespace Cinema.EcommerceTicket.Infrastructure.HttpClients.Handlers;

public class AuthenticateCustomHandler<T>(T gatewayAdapter) : DelegatingHandler where T : GatewayAdapterBase
{
    private readonly T _gatewayAdapter = gatewayAdapter;
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        
        // Autentica a requisição
        await _gatewayAdapter.Authenticate(request);

        // Chama o próximo handler na cadeia (ou envia a requisição)
        var response = await base.SendAsync(request, cancellationToken);

        return response;
    }
}
