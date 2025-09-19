using Microsoft.OpenApi.Models;
using InvestimentosApp.Data.Context;
using InvestimentosApp.Data.Repositories;
using InvestimentosApp.Domain.Interfaces;
using InvestimentosApp.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração explícita das URLs
builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

// Adiciona serviços ao container
builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Investimentos",
        Version = "v1",
        Description = "API para gerenciar investimentos",
        Contact = new OpenApiContact
        {
            Name = "Seu Nome",
            Email = "seu.email@example.com"
        }
    });
});

// Configuração do DbContext do Entity Framework
// ⚠️ IMPORTANTE: Substitua pelas suas credenciais reais da FIAP
builder.Services.AddScoped<AppDbContext>(provider =>
    new AppDbContext("SEU_RM", "SUA_SENHA"));

// Registrar os repositórios
builder.Services.AddScoped<IInvestidorRepository, InvestidorRepository>();
builder.Services.AddScoped<IInvestimentoRepository, InvestimentoRepository>();

// Registrar o serviço de arquivo
builder.Services.AddScoped<ArquivoService>();

var app = builder.Build();

// Configure o pipeline de requisição HTTP
// Habilitando o Swagger em todos os ambientes para fins de teste
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./v1/swagger.json", "API de Investimentos v1");
    c.RoutePrefix = "swagger";
});

app.UseAuthorization();
app.MapControllers();

app.Run();