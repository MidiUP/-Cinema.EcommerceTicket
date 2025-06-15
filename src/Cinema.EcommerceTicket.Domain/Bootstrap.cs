using Cinema.EcommerceTicket.Domain.Services;
using Cinema.EcommerceTicket.Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Cinema.EcommerceTicket.Domain;

[ExcludeFromCodeCoverage]
public static class Bootstrap
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ITicketService, TicketService>();
    }
}
