using Microsoft.AspNetCore.Mvc;
using InvestimentosApp.API.Services;
using InvestimentosApp.Domain.Interfaces;

namespace InvestimentosApp.API.Controllers
{
    // Controller para operações de arquivo (importar/exportar)
    [ApiController]
    [Route("api/[controller]")]
    public class ArquivosController : ControllerBase
    {
        // Dependências
        private readonly ArquivoService _arquivoService;
        private readonly IInvestidorRepository _investidorRepository;
        private readonly IInvestimentoRepository _investimentoRepository;

        // Construtor - injeta dependências
        public ArquivosController(
            ArquivoService arquivoService,
            IInvestidorRepository investidorRepository,
            IInvestimentoRepository investimentoRepository)
        {
            _arquivoService = arquivoService;
            _investidorRepository = investidorRepository;
            _investimentoRepository = investimentoRepository;
        }

        // GET: Exportar investidores para JSON
        [HttpGet("exportar/investidores/json")]
        public async Task<IActionResult> ExportarInvestidoresJson()
        {
            try
            {
                var investidores = await _investidorRepository.GetAllAsync();
                var nomeArquivo = await _arquivoService.ExportarInvestidoresAsync(investidores);
                return Ok(new { arquivo = nomeArquivo, mensagem = "Investidores exportados com sucesso para JSON!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao exportar investidores: {ex.Message}");
            }
        }

        // GET: Exportar investidores para TXT
        [HttpGet("exportar/investidores/txt")]
        public async Task<IActionResult> ExportarInvestidoresTxt()
        {
            try
            {
                var investidores = await _investidorRepository.GetAllAsync();
                var nomeArquivo = await _arquivoService.ExportarInvestidoresTxtAsync(investidores);
                return Ok(new { arquivo = nomeArquivo, mensagem = "Investidores exportados com sucesso para TXT!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao exportar investidores: {ex.Message}");
            }
        }

        // GET: Exportar investimentos para JSON
        [HttpGet("exportar/investimentos")]
        public async Task<IActionResult> ExportarInvestimentos()
        {
            try
            {
                var investimentos = await _investimentoRepository.GetAllAsync();
                var nomeArquivo = await _arquivoService.ExportarInvestimentosAsync(investimentos);
                return Ok(new { arquivo = nomeArquivo, mensagem = "Investimentos exportados com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao exportar investimentos: {ex.Message}");
            }
        }

        // POST: Importar investidores de arquivo JSON
        [HttpPost("importar/investidores")]
        public async Task<IActionResult> ImportarInvestidores(IFormFile arquivo)
        {
            try
            {
                var investidores = await _arquivoService.ImportarInvestidoresAsync(arquivo);
                int contadorSucesso = 0;
                
                foreach (var investidor in investidores)
                {
                    investidor.Id = 0; // Remove ID para inserção
                    var resultado = await _investidorRepository.AddAsync(investidor);
                    if (resultado) contadorSucesso++;
                }
                
                return Ok(new { mensagem = $"Importados {contadorSucesso} de {investidores.Count} investidores com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao importar investidores: {ex.Message}");
            }
        }

        // POST: Importar investimentos de arquivo JSON
        [HttpPost("importar/investimentos")]
        public async Task<IActionResult> ImportarInvestimentos(IFormFile arquivo)
        {
            try
            {
                // Processa o arquivo e converte para lista de investimentos
                var investimentos = await _arquivoService.ImportarInvestimentosAsync(arquivo);
                int contadorSucesso = 0;
                
                foreach (var investimento in investimentos)
                {
                    investimento.Id = 0; // Remove ID para inserção
                    var resultado = await _investimentoRepository.AddAsync(investimento);
                    if (resultado) contadorSucesso++;
                }
                
                return Ok(new { mensagem = $"Importados {contadorSucesso} de {investimentos.Count} investimentos com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao importar investimentos: {ex.Message}");
            }
        }
    } 
} 