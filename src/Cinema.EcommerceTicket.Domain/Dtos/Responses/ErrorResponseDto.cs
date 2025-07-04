using System.Diagnostics.CodeAnalysis;

namespace Cinema.EcommerceTicket.Domain.Dtos.Responses
{
    /// <summary>
    /// DTO utilizado para padronizar as respostas de erro da API.
    /// </summary>
    /// <remarks>
    /// Pode conter uma lista de erros detalhados e/ou uma mensagem geral de erro.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public record ErrorResponseDto
    {
        /// <summary>
        /// Lista de mensagens de erro detalhadas.
        /// </summary>
        public List<string>? Errors { get; set; }

        /// <summary>
        /// Mensagem geral de erro.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Cria uma instância de <see cref="ErrorResponseDto"/> com uma lista de erros e uma mensagem.
        /// </summary>
        /// <param name="errors">Lista de mensagens de erro.</param>
        /// <param name="message">Mensagem geral de erro.</param>
        public ErrorResponseDto(List<string> errors, string message)
        {
            Errors = errors;
            Message = message;
        }

        /// <summary>
        /// Cria uma instância de <see cref="ErrorResponseDto"/> com um erro único e uma mensagem.
        /// </summary>
        /// <param name="error">Mensagem de erro.</param>
        /// <param name="message">Mensagem geral de erro.</param>
        public ErrorResponseDto(string error, string message) : this([error], message) { }

        /// <summary>
        /// Cria uma instância de <see cref="ErrorResponseDto"/> apenas com uma mensagem.
        /// </summary>
        /// <param name="message">Mensagem geral de erro.</param>
        public ErrorResponseDto(string message)
        {
            Message = message;
        }
    }
}
