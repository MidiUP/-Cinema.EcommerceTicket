namespace Cinema.EcommerceTicket.Domain.Infrastructure.Cache;

/// <summary>
/// Interface para operações de cache distribuído na aplicação.
/// </summary>
/// <remarks>
/// Define métodos para obter, armazenar, remover e verificar a existência de itens no cache.
/// </remarks>
public interface ICacheRepository
{
    /// <summary>
    /// Obtém um item do cache a partir da chave informada.
    /// </summary>
    /// <typeparam name="T">Tipo do objeto a ser recuperado.</typeparam>
    /// <param name="key">Chave do item no cache.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>O objeto do tipo <typeparamref name="T"/> se encontrado; caso contrário, <c>null</c>.</returns>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Armazena um item no cache com a chave e tempo de expiração informados.
    /// </summary>
    /// <typeparam name="T">Tipo do objeto a ser armazenado.</typeparam>
    /// <param name="key">Chave do item no cache.</param>
    /// <param name="value">Objeto a ser armazenado.</param>
    /// <param name="expiration">Tempo de expiração do item no cache.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Remove um item do cache a partir da chave informada.
    /// </summary>
    /// <param name="key">Chave do item a ser removido.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    Task RemoveAsync(string key, CancellationToken cancellationToken);

    /// <summary>
    /// Verifica se existe um item no cache com a chave informada.
    /// </summary>
    /// <param name="key">Chave do item no cache.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns><c>true</c> se o item existir no cache; caso contrário, <c>false</c>.</returns>
    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken);
}
