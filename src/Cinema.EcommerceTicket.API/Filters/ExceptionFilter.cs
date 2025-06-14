using Cinema.EcommerceTicket.Domain.Dtos.Responses;
using Cinema.EcommerceTicket.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Cinema.EcommerceTicket.API.Filters;

public class ExceptionFilter(ILogger<ExceptionFilter> logger) : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger = logger;

    const string SERVER_ERROR_MESSAGE = "Ocorreu um inesperado. Por favor tente novamente mais tarde";
    const string TIMEOUT_ERROR_MESSAGE = "Sua requisição excedeu o tempo limite.";

    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case CinemaEcommerceTicketException cinemaEx:
                _logger.LogWarning(context.Exception, "Erro conhecido no filtro de exceção");
                HandleResultException(
                    context,
                    new ErrorResponseDto(cinemaEx.Errors ?? [], cinemaEx.Message),
                    cinemaEx.ERROR_CODE
                );
                break;

            case OperationCanceledException canceledEx:
                _logger.LogWarning(canceledEx, "Timeout no filtro de exceção");
                HandleResultException(
                    context,
                    new ErrorResponseDto(TIMEOUT_ERROR_MESSAGE),
                    (int)HttpStatusCode.RequestTimeout
                );
                break;

            default:
                _logger.LogError(context.Exception, "Erro desconhecido no filtro de exceção");
                HandleResultException(
                    context,
                    new ErrorResponseDto(SERVER_ERROR_MESSAGE),
                    (int)HttpStatusCode.InternalServerError
                );
                break;
        }
    }

    private static void HandleResultException(ExceptionContext context, ErrorResponseDto errorResponseDto, int statusCode)
    {
        context.Result = new ObjectResult(errorResponseDto);
        context.HttpContext.Response.StatusCode = statusCode;
    }

}
