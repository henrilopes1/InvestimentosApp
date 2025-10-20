using Microsoft.OpenApi.Models;
using InvestimentosApp.Data.Context;
using InvestimentosApp.Data.Repositories;
using InvestimentosApp.Domain.Interfaces;
using InvestimentosApp.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração de porta
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://*:{port}");

// Serviços básicos
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Investimentos - FIAP",
        Version = "v1",
        Description = "API completa para gerenciamento de investimentos com LINQ, Alpha Vantage e MarketStack",
        Contact = new OpenApiContact
        {
            Name = "Henri Lopes",
            Email = "henri@fiap.com.br"
        }
    });
});

// Configuração do Oracle Database
const string OracleDataSource = "oracle.fiap.com.br:1521/ORCL"; 
var oracleUser = builder.Configuration["ORACLE_USER"] ?? Environment.GetEnvironmentVariable("ORACLE_USER") ?? ""; 
var oraclePassword = builder.Configuration["ORACLE_PASSWORD"] ?? Environment.GetEnvironmentVariable("ORACLE_PASSWORD") ?? ""; 

// Log das configurações (sem senha)
Console.WriteLine($"🔗 Oracle User: {oracleUser}");
Console.WriteLine($"🔗 DataSource: {OracleDataSource}");
Console.WriteLine($"🔗 Environment: {builder.Environment.EnvironmentName}");

var oracleConnectionString = $"User Id={oracleUser};Password={oraclePassword};Data Source={OracleDataSource}";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(oracleConnectionString)
);

// Repositórios
try 
{
    builder.Services.AddScoped<IInvestidorRepository, InvestidorRepository>();
    builder.Services.AddScoped<IInvestimentoRepository, InvestimentoRepository>();
    Console.WriteLine("✅ Repositórios configurados");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Erro nos repositórios: {ex.Message}");
}

// Serviços auxiliares
builder.Services.AddScoped<ArquivoService>();

// APIs externas
builder.Services.AddHttpClient<IAlphaVantageService, AlphaVantageService>();
builder.Services.AddScoped<IAlphaVantageService, AlphaVantageService>();

builder.Services.AddHttpClient<IMarketStackService, MarketStackService>();
builder.Services.AddScoped<IMarketStackService, MarketStackService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Pipeline HTTP
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./v1/swagger.json", "API de Investimentos FIAP v1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "API Investimentos - Documentação";
});

app.UseCors();
app.UseAuthorization();
app.MapControllers();

// Mensagens de inicialização
Console.WriteLine("🚀 API de Investimentos iniciada!");
Console.WriteLine($"📊 Swagger: /swagger");
Console.WriteLine($"🔌 Porta: {port}");

app.Run();