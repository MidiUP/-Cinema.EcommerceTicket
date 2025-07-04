namespace Cinema.EcommerceTicket.Domain.Shared;

/// <summary>
/// Representa o resultado de uma validação de modelo, contendo o status e as mensagens de erro encontradas.
/// </summary>
/// <remarks>
/// Utilizada para indicar se uma operação de validação foi bem-sucedida e, em caso negativo, quais erros ocorreram.
/// </remarks>
public class ValidationResult
{
    /// <summary>
    /// Indica se a validação foi bem sucedida.
    /// Retorna <c>true</c> se não houver erros; caso contrário, <c>false</c>.
    /// </summary>
    public bool IsValid
    {
        get
        {
            return Errors.Count == 0;
        }
    }

    /// <summary>
    /// Lista de mensagens de erro encontradas durante a validação.
    /// </summary>
    public List<string> Errors { get; private set; } = new List<string>();

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ValidationResult"/> sem erros.
    /// </summary>
    public ValidationResult() { }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ValidationResult"/> com um erro inicial.
    /// </summary>
    /// <param name="error">Mensagem de erro a ser adicionada.</param>
    /// <exception cref="ArgumentException">Lançada se o erro for nulo ou vazio.</exception>
    public ValidationResult(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
            throw new ArgumentException("O erro não pode ser nulo ou vazio.");

        Errors.Add(error);
    }

    /// <summary>
    /// Adiciona uma mensagem à lista de erros.
    /// </summary>
    /// <param name="error">Mensagem de erro a ser adicionada.</param>
    public void AddError(string error)
    {
        Errors.Add(error);
    }
}
