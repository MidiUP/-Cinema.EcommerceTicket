using Cinema.EcommerceTicket.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace Cinema.EcommerceTicket.Infrastructure.HttpClients;

public static class Setup
{
    public static void AddHttpClients(this IServiceCollection services)
    {
        services.AddCatalogApiHttpClient();
    }

    private static void AddCatalogApiHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient(Constants.CatalogApi.NAME, httpClient =>
        {
            httpClient.BaseAddress = new Uri(Constants.CatalogApi.BASE_URL);
        })
        .AddPolicyHandler(GetRetryPolicy());
    }

    private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
    }
}
