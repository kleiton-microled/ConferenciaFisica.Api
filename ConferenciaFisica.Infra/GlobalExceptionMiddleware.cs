using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using ConferenciaFisica.Application.Common.Models; // Importe sua classe ServiceResult

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro capturado pelo middleware global: {ex.Message}");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = exception switch
        {
            ApplicationException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        // Criando o ServiceResult para padronizar a resposta
        var errorResponse = new ServiceResult<object>
        {
            Status = false,
            Error = exception.Message,
            Mensagens = new List<string> { "Ocorreu um erro ao processar a requisição." },
            Result = null
        };

        var json = JsonSerializer.Serialize(errorResponse);
        return response.WriteAsync(json);
    }
}
