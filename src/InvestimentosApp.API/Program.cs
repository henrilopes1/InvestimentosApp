using Microsoft.OpenApi.Models;
using InvestimentosApp.Data.Context;
using InvestimentosApp.Data.Repositories;
using InvestimentosApp.Domain.Interfaces;
using InvestimentosApp.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração das URLs
builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

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

// Configuração do DbContext do Entity Framework
// ⚠️ IMPORTANTE: Substitua pelas suas credenciais reais da FIAP
builder.Services.AddScoped<AppDbContext>(provider =>
    new AppDbContext("SEU_RM", "SUA_SENHA"));

// Repositórios
builder.Services.AddScoped<IInvestidorRepository, InvestidorRepository>();
builder.Services.AddScoped<IInvestimentoRepository, InvestimentoRepository>();

// Serviços auxiliares
builder.Services.AddScoped<ArquivoService>();

// APIs externas
builder.Services.AddHttpClient<IAlphaVantageService, AlphaVantageService>();
builder.Services.AddScoped<IAlphaVantageService, AlphaVantageService>();

builder.Services.AddHttpClient<IMarketStackService, MarketStackService>();
builder.Services.AddScoped<IMarketStackService, MarketStackService>();

builder.Services.AddScoped<IMarketStackService, MarketStackService>();

var app = builder.Build();

// Pipeline HTTP
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./v1/swagger.json", "API de Investimentos FIAP v1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "API Investimentos - Documentação";
});

app.UseAuthorization();
app.MapControllers();

// Mensagens de inicialização
Console.WriteLine("🚀 API de Investimentos iniciada!");
Console.WriteLine("📊 Swagger: http://localhost:5000/swagger");

app.Run();