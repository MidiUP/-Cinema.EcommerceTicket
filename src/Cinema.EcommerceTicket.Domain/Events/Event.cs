using System.Diagnostics.CodeAnalysis;

namespace Cinema.Events;

/// <summary>
/// Classe base para eventos de domínio utilizados no sistema.
/// </summary>
/// <remarks>
/// Todas as classes de evento devem herdar desta classe para padronização e identificação de eventos no domínio.
/// Pode ser estendida para incluir propriedades comuns a todos os eventos, como data de ocorrência ou identificador.
/// </remarks>
[ExcludeFromCodeCoverage]
public class Event { }
