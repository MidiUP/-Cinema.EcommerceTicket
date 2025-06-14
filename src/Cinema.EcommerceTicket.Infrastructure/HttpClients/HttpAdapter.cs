using Newtonsoft.Json;

namespace Cinema.EcommerceTicket.Infrastructure.HttpClients;

public static class HttpAdapter
{
    public static async Task<TResponse> GetAsync<TResponse>(this HttpClient client, string url, CancellationToken cancellationToken = default)
    {
        // make request
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        //realizar request
        var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        //desserializar
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TResponse>(content)!;

    }
    public static Task<TResponse> PostAsync<TRequest, TResponse>(this HttpClient client, string url, TRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException(); // não implementado por questão de tempo, mas seguiria a base do get
    }
    public static Task<TResponse> PutAsync<TResponse>(this HttpClient client, string url, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException(); // não implementado por questão de tempo, mas seguiria a base do get
    }
    public static Task<TResponse> DeleteAsync<TResponse>(this HttpClient client, string url, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException(); // não implementado por questão de tempo, mas seguiria a base do get
    }
}
