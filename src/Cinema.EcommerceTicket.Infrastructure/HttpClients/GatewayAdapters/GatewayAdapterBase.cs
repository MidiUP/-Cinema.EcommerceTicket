namespace Cinema.EcommerceTicket.Infrastructure.HttpClients.GatewayAdapters;

public abstract class GatewayAdapterBase
{
    public abstract HttpClient Client { get; set; }
    public abstract Task Authenticate(HttpRequestMessage requestMessage);
}
