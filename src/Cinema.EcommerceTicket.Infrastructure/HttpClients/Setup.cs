using Cinema.EcommerceTicket.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Cinema.EcommerceTicket.Infrastructure.HttpClients;

public static class Setup
{
    public static void AddHttpClients(this IServiceCollection services)
    {
        services.AddCatalogApiHttpClient();
        //services.AddHandlersHttp(); exemplo de como adicionar handlers personalizados
    }

    private static void AddCatalogApiHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient(Constants.CatalogApi.NAME, httpClient =>
        {
            httpClient.BaseAddress = new Uri(Constants.CatalogApi.BASE_URL);
        })
        .AddPolicyHandler(GetRetryPolicy());
        //.AddHttpMessageHandler<AuthenticateCustomHandler<SensediaGatewayAdapter>>();
    }

    private static void AddHandlersHttp(this IServiceCollection services)
    {
        //services.AddTransient<AuthenticateCustomHandler<SensediaGatewayAdapter>>(); // exemplo de como adicionar handler de autenticação personalizado
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
    }
}
