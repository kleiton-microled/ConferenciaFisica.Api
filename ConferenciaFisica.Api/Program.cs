using ConferenciaFisica.Application.Mapping;
using ConferenciaFisica.Infra.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Text.Json.Serialization;
using ConferenciaFisica.Infra.Extensions;
using ConferenciaFisica.Infra.Middleware;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });


// Habilitando CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Authorization"));
});


// Adicionando serviços ao container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT assim: Bearer {seu_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();

builder.Services.AddExtensionServices();
builder.Services.AddAutoMapper(typeof(MappingProfile));


// Adicionando Health Check
builder.Services.AddHealthChecks()
    //.AddCheck<OracleHealthCheck>("oracle_health_check")
    .AddCheck<SqlServerHealthCheck>("sqlserver_health_check"); ;

var app = builder.Build();

// Configuração do pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adiciona o middleware de ambiente
app.UseMiddleware<EnvironmentMiddleware>();


app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // 🔐 Primeiro autentica...
app.UseAuthorization();  // 🔒 Depois autoriza!

// 🔥 Middleware Global de Tratamento de Erros
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseEndpoints(endpoints =>
{
    // Mapeia os controllers
    endpoints.MapControllers();

    // Health Check
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            var result = report.Status == HealthStatus.Healthy ? "Healthy" : "Unhealthy";
            await context.Response.WriteAsync(result);
        }
    });

    // (Opcional) Rota padrão - normalmente usada em MVC tradicional
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});


app.Run();

