namespace Cinema.EcommerceTicket.Domain.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Classe utilitária para configuração centralizada das options da aplicação.
/// </summary>
/// <remarks>
/// Utiliza o padrão Options do ASP.NET Core para registrar e validar configurações fortemente tipadas.
/// </remarks>
[ExcludeFromCodeCoverage]
public static class OptionsSetup
{
    /// <summary>
    /// Registra e valida as options da aplicação a partir do <see cref="IConfiguration"/>.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <param name="configuration">Configuração da aplicação.</param>
    public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomainOptions<RabbitMqOptions>(configuration, "RabbitMq");
        services.AddDomainOptions<MongoDbOptions>(configuration, "MongoDb");
        services.AddDomainOptions<CatalogApiOptions>(configuration, "CatalogApi");
    }

    /// <summary>
    /// Método auxiliar para registrar e validar uma option específica.
    /// </summary>
    /// <typeparam name="T">Tipo da option.</typeparam>
    /// <param name="services">Coleção de serviços.</param>
    /// <param name="configuration">Configuração da aplicação.</param>
    /// <param name="section">Nome da seção no appsettings.</param>
    private static void AddDomainOptions<T>(this IServiceCollection services, IConfiguration configuration, string section) where T : class
    {
        services.AddOptions<T>()
            .Bind(configuration.GetSection(section))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}

/// <summary>
/// Opções de configuração para integração com RabbitMQ.
/// </summary>
[ExcludeFromCodeCoverage]
public class RabbitMqOptions
{
    /// <summary>
    /// Host do servidor RabbitMQ.
    /// </summary>
    [Required]
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// Porta do servidor RabbitMQ.
    /// </summary>
    [Required]
    public string Port { get; set; } = string.Empty;

    /// <summary>
    /// Nome de usuário para autenticação no RabbitMQ.
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Senha para autenticação no RabbitMQ.
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Nome da fila para criação de tickets de ecommerce.
    /// </summary>
    [Required]
    public string QueueCreateEcommerceTicketName { get; set; } = string.Empty;
}

/// <summary>
/// Opções de configuração para integração com o MongoDB.
/// </summary>
[ExcludeFromCodeCoverage]
public class MongoDbOptions
{
    /// <summary>
    /// String de conexão com o MongoDB.
    /// </summary>
    [Required]
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Nome do banco de dados.
    /// </summary>
    [Required]
    public string DatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// Nome da coleção de tickets.
    /// </summary>
    [Required]
    public string TicketsCollectionName { get; set; } = string.Empty;
}

/// <summary>
/// Opções de configuração para integração com a API de catálogo.
/// </summary>
[ExcludeFromCodeCoverage]
public class CatalogApiOptions
{
    /// <summary>
    /// Nome da configuração da API de catálogo.
    /// </summary>
    [Required]
    public string Name => "CatalogApi";

    /// <summary>
    /// URL base da API de catálogo.
    /// </summary>
    [Required]
    public string BaseUrl { get; set; } = string.Empty;
}

/// <summary>
/// Opções de configuração para integração com o Redis.
/// </summary>
[ExcludeFromCodeCoverage]
public class RedisOptions
{
    /// <summary>
    /// String de conexão com o Redis.
    /// </summary>
    [Required]
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Nome da instância Redis utilizada como prefixo para as chaves.
    /// </summary>
    [Required]
    public string RedisInstanceName => $"{Constants.ENVIRONMENT}.cinemaEcommerceTicket:";
}
