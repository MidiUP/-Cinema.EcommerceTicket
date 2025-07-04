using System.Diagnostics.CodeAnalysis;

namespace Cinema.EcommerceTicket.Domain.Shared;

/// <summary>
/// Classe estática que centraliza constantes e variáveis globais da aplicação.
/// </summary>
/// <remarks>
/// Utilizada para acessar valores globais, como o ambiente de execução da aplicação.
/// </remarks>
[ExcludeFromCodeCoverage]
public static class Constants
{
    /// <summary>
    /// Obtém o valor do ambiente de execução da aplicação a partir da variável de ambiente "ENV".
    /// Caso a variável não esteja definida, retorna o valor padrão "ENV".
    /// </summary>
    public static string ENVIRONMENT => Environment.GetEnvironmentVariable("ENV") ?? "ENV";
}
