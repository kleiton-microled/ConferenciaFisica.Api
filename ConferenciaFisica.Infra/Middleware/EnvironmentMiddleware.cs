using ConferenciaFisica.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ConferenciaFisica.Infra.Middleware
{
    public class EnvironmentMiddleware
    {
        private readonly RequestDelegate _next;

        public EnvironmentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ISchemaService schemaService)
        {
            // Verifica se existe o header X-Environment na requisição
            if (context.Request.Headers.TryGetValue("X-Environment", out var environmentHeader))
            {
                var environment = environmentHeader.FirstOrDefault();
                if (!string.IsNullOrEmpty(environment))
                {
                    schemaService.SetEnvironment(environment);
                }
            }

            await _next(context);
        }
    }
}
