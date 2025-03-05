using ConferenciaFisica.Application.UseCases.Agendamento;
using ConferenciaFisica.Application.UseCases.Agendamento.Interfaces;
using ConferenciaFisica.Application.UseCases.Conferencia;
using ConferenciaFisica.Application.UseCases.Conferencia.Interfaces;
using ConferenciaFisica.Application.UseCases.Lacres;
using ConferenciaFisica.Application.UseCases.Lacres.Interfaces;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.HealthChecks;
using ConferenciaFisica.Infra.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Habilitando CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin() // Libera qualquer origem
                        .AllowAnyMethod() // Permite qualquer método (GET, POST, PUT, DELETE, etc.)
                        .AllowAnyHeader()); // Permite qualquer cabeçalho
});

// Adicionando serviços ao container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

    }); ;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddOpenApi();
builder.Services.AddSingleton<SqlServerConnectionFactory>();

builder.Services.AddScoped<IBuscarConferenciaUseCase, BuscarConferenciaUseCase>();
builder.Services.AddScoped<IConferenciaRepository, ConferenciaRepository>();

builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
builder.Services.AddScoped<ICarregarLotesAgendamentoUseCase, CarregarLotesAgendamentoUseCase>();
builder.Services.AddScoped<ICarregarCntrAgendamentoUseCase, CarregarCntrAgendamentoUseCase>();
builder.Services.AddScoped<IIniciarConferenciaUseCase, IniciarConferenciaUseCase>();
builder.Services.AddScoped<IAtualizarConferenciaUseCase, AtualizarConferenciaUseCase>();
builder.Services.AddScoped<ICadastrosAdicionaisUseCase, CadastrosAdicionaisUseCase>();
builder.Services.AddScoped<ITiposLacresUseCase, TiposLacresUseCase>();



// Adicionando Health Check
builder.Services.AddHealthChecks()
    //.AddCheck<OracleHealthCheck>("oracle_health_check")
    .AddCheck<SqlServerHealthCheck>("sqlserver_health_check"); ;

var app = builder.Build();

// Configuração do pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");


app.UseRouting();

// 🔥 Middleware Global de Tratamento de Erros
app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapControllers();

// Criando um endpoint para Health Check
app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            var result = report.Status == HealthStatus.Healthy ? "Healthy" : "Unhealthy";
            await context.Response.WriteAsync(result);
        }
    });
});

app.Run();

