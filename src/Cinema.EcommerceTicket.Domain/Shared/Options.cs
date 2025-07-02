namespace Cinema.EcommerceTicket.Domain.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public static class OptionsSetup
{
    public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomainOptions<RabbitMqOptions>(configuration, "RabbitMq");
        services.AddDomainOptions<MongoDbOptions>(configuration, "MongoDb");
        services.AddDomainOptions<CatalogApiOptions>(configuration, "CatalogApi");
    }

    private static void AddDomainOptions<T>(this IServiceCollection services, IConfiguration configuration, string section) where T : class
    {
        services.AddOptions<T>()
            .Bind(configuration.GetSection(section))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}

[ExcludeFromCodeCoverage]
public class RabbitMqOptions
{
    [Required]
    public string Host { get; set; } = string.Empty;
    [Required]
    public string Port { get; set; } = string.Empty;
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required] 
    public string QueueCreateEcommerceTicketName { get; set; } = string.Empty;
}

[ExcludeFromCodeCoverage]
public class MongoDbOptions
{
    [Required]
    public string ConnectionString { get; set; } = string.Empty;
    [Required]
    public string DatabaseName { get; set; } = string.Empty;
    [Required]
    public string TicketsCollectionName { get; set; } = string.Empty;
}

[ExcludeFromCodeCoverage]
public class CatalogApiOptions
{
    [Required]
    public string Name => "CatalogApi";
    [Required]
    public string BaseUrl { get; set; } = string.Empty;
}

[ExcludeFromCodeCoverage]
public class RedisOptions
{
    [Required]
    public string ConnectionString { get; set; } = string.Empty;
    [Required]
    public string RedisInstanceName => $"{Constants.ENVIRONMENT}.cinemaEcommerceTicket:";
}