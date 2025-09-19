using System.Text.Json;
using InvestimentosApp.Domain.Models;

namespace InvestimentosApp.API.Services
{
    public class ArquivoService
    {
        private readonly string _diretorioBase;

        public ArquivoService(IWebHostEnvironment env)
        {
            _diretorioBase = Path.Combine(env.ContentRootPath, "Data", "Exports");
            if (!Directory.Exists(_diretorioBase))
            {
                Directory.CreateDirectory(_diretorioBase);
            }
        }

        public async Task<string> ExportarInvestidoresAsync(IEnumerable<Investidor> investidores)
        {
            var nomeArquivo = $"investidores_{DateTime.Now:yyyyMMddHHmmss}.json";
            var caminhoCompleto = Path.Combine(_diretorioBase, nomeArquivo);
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            var json = JsonSerializer.Serialize(investidores, options);
            await File.WriteAllTextAsync(caminhoCompleto, json);
            
            return $"Arquivo exportado: {caminhoCompleto}";
        }

        public async Task<string> ExportarInvestimentosAsync(IEnumerable<Investimento> investimentos)
        {
            var nomeArquivo = $"investimentos_{DateTime.Now:yyyyMMddHHmmss}.json";
            var caminhoCompleto = Path.Combine(_diretorioBase, nomeArquivo);
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            var json = JsonSerializer.Serialize(investimentos, options);
            await File.WriteAllTextAsync(caminhoCompleto, json);
            
            return $"Arquivo exportado: {caminhoCompleto}";
        }

        public async Task<List<Investidor>> ImportarInvestidoresAsync(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
                throw new ArgumentException("Arquivo não fornecido");

            using var stream = arquivo.OpenReadStream();
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Arquivo está vazio");
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var investidores = JsonSerializer.Deserialize<List<Investidor>>(json, options);
            
            if (investidores == null)
                throw new ArgumentException("Formato de arquivo inválido");
                
            // Validar se todos os campos obrigatórios estão presentes
            foreach (var investidor in investidores)
            {
                if (string.IsNullOrWhiteSpace(investidor.Nome))
                    throw new ArgumentException("Campo 'Nome' é obrigatório para todos os investidores");
                if (string.IsNullOrWhiteSpace(investidor.CPF))
                    throw new ArgumentException("Campo 'CPF' é obrigatório para todos os investidores");
                if (string.IsNullOrWhiteSpace(investidor.Email))
                    throw new ArgumentException("Campo 'Email' é obrigatório para todos os investidores");
                if (string.IsNullOrWhiteSpace(investidor.PerfilRisco))
                    throw new ArgumentException("Campo 'PerfilRisco' é obrigatório para todos os investidores");
                    
                // Garantir que o ID seja 0 para novos registros
                investidor.Id = 0;
            }
            
            return investidores;
        }

        public async Task<List<Investimento>> ImportarInvestimentosAsync(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
                throw new ArgumentException("Arquivo não fornecido");

            using var stream = arquivo.OpenReadStream();
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Arquivo está vazio");
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var investimentos = JsonSerializer.Deserialize<List<Investimento>>(json, options);
            
            if (investimentos == null)
                throw new ArgumentException("Formato de arquivo inválido");
                
            // Garantir que o ID seja 0 para novos registros
            foreach (var investimento in investimentos)
            {
                investimento.Id = 0;
            }
            
            return investimentos;
        }
        
        // Método para exportar para TXT (demonstrando versatilidade)
        public async Task<string> ExportarInvestidoresTxtAsync(IEnumerable<Investidor> investidores)
        {
            var nomeArquivo = $"investidores_{DateTime.Now:yyyyMMddHHmmss}.txt";
            var caminhoCompleto = Path.Combine(_diretorioBase, nomeArquivo);
            
            using var writer = new StreamWriter(caminhoCompleto);
            await writer.WriteLineAsync("ID,Nome,CPF,Email,DataNascimento,SaldoTotal,PerfilRisco");
            
            foreach (var investidor in investidores)
            {
                await writer.WriteLineAsync(
                    $"{investidor.Id},{investidor.Nome},{investidor.CPF},{investidor.Email}," +
                    $"{investidor.DataNascimento:yyyy-MM-dd},{investidor.SaldoTotal},{investidor.PerfilRisco}");
            }
            
            return nomeArquivo;
        }
    }
}