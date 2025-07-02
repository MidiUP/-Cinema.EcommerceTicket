using System.Diagnostics.CodeAnalysis;

namespace Cinema.EcommerceTicket.Domain.Shared;

[ExcludeFromCodeCoverage]
public static class Constants
{
    public static string ENVIRONMENT => Environment.GetEnvironmentVariable("ENV") ?? "ENV";
}
