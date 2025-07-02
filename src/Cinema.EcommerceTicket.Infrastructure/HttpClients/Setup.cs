using Cinema.EcommerceTicket.Domain.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace Cinema.EcommerceTicket.Infrastructure.HttpClients;

public static class Setup
{
    public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCatalogApiHttpClient(configuration);
    }

    private static void AddCatalogApiHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var catalogApiOptions = configuration.GetSection("CatalogApi").Get<CatalogApiOptions>()!;

        services.AddHttpClient(catalogApiOptions.Name, httpClient =>
        {
            httpClient.BaseAddress = new Uri(catalogApiOptions.BaseUrl);
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
