using Cinema.EcommerceTicket.Domain.Services;
using Cinema.EcommerceTicket.Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Cinema.EcommerceTicket.Domain;

/// <summary>
/// Classe utilitária responsável por registrar os serviços de domínio na injeção de dependência.
/// </summary>
/// <remarks>
/// Utilize este bootstrap para adicionar todos os serviços do domínio ao <see cref="IServiceCollection"/> da aplicação.
/// </remarks>
[ExcludeFromCodeCoverage]
public static class Bootstrap
{
    /// <summary>
    /// Adiciona os serviços de domínio à coleção de serviços da aplicação.
    /// </summary>
    /// <param name="services">A coleção de serviços onde os serviços de domínio serão registrados.</param>
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ITicketService, TicketService>();
    }
}
