using InvestimentosApp.Domain.Interfaces;
using InvestimentosApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestimentosController : ControllerBase
    {
        private readonly IInvestimentoRepository _investimentoRepository;

        public InvestimentosController(IInvestimentoRepository investimentoRepository)
        {
            _investimentoRepository = investimentoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Investimento>>> GetAll()
        {
            try
            {
                var investimentos = await _investimentoRepository.GetAllAsync();
                return Ok(investimentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Investimento>> GetById(int id)
        {
            try
            {
                var investimento = await _investimentoRepository.GetByIdAsync(id);
                if (investimento == null)
                    return NotFound();

                return Ok(investimento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        [HttpGet("investidor/{investidorId}")]
        public async Task<ActionResult<IEnumerable<Investimento>>> GetByInvestidorId(int investidorId)
        {
            try
            {
                var investimentos = await _investimentoRepository.GetByInvestidorIdAsync(investidorId);
                return Ok(investimentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Investimento>> Create(Investimento investimento)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return BadRequest(new { message = "Dados inválidos", errors = errors });
                }

                // Garantir que o ID seja 0 para novo registro
                investimento.Id = 0;

                var success = await _investimentoRepository.AddAsync(investimento);
                if (!success)
                    return BadRequest(new { message = "Não foi possível criar o investimento. Verifique se o investidor existe." });

                return CreatedAtAction(nameof(GetById), new { id = investimento.Id }, investimento);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro interno", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Investimento investimento)
        {
            if (id != investimento.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _investimentoRepository.UpdateAsync(investimento);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _investimentoRepository.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // =================== ENDPOINTS DE PESQUISA AVANÇADA ===================

        // GET: Buscar por tipo
        [HttpGet("buscar/tipo/{tipo}")]
        public async Task<ActionResult<IEnumerable<Investimento>>> GetByTipo(string tipo)
        {
            var investimentos = await _investimentoRepository.GetByTipoAsync(tipo);
            return Ok(investimentos);
        }

        // GET: Buscar por status
        [HttpGet("buscar/status/{status}")]
        public async Task<ActionResult<IEnumerable<Investimento>>> GetByStatus(string status)
        {
            var investimentos = await _investimentoRepository.GetByStatusAsync(status);
            return Ok(investimentos);
        }

        // GET: Buscar por faixa de rentabilidade
        [HttpGet("buscar/rentabilidade")]
        public async Task<ActionResult<IEnumerable<Investimento>>> GetByRentabilidadeRange(
            [FromQuery] decimal rentabilidadeMinima,
            [FromQuery] decimal rentabilidadeMaxima)
        {
            var investimentos = await _investimentoRepository.GetByRentabilidadeRangeAsync(rentabilidadeMinima, rentabilidadeMaxima);
            return Ok(investimentos);
        }

        // GET: Buscar por faixa de valor
        [HttpGet("buscar/valor")]
        public async Task<ActionResult<IEnumerable<Investimento>>> GetByValorRange(
            [FromQuery] decimal valorMinimo,
            [FromQuery] decimal valorMaximo)
        {
            var investimentos = await _investimentoRepository.GetByValorRangeAsync(valorMinimo, valorMaximo);
            return Ok(investimentos);
        }

        // GET: Buscar por período
        [HttpGet("buscar/periodo")]
        public async Task<ActionResult<IEnumerable<Investimento>>> GetByPeriodo(
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim)
        {
            var investimentos = await _investimentoRepository.GetByPeriodoAsync(dataInicio, dataFim);
            return Ok(investimentos);
        }

        // GET: Busca avançada com múltiplos filtros
        [HttpGet("buscar/avancada")]
        public async Task<ActionResult<IEnumerable<Investimento>>> SearchAdvanced(
            [FromQuery] string? nome,
            [FromQuery] string? tipo,
            [FromQuery] string? status,
            [FromQuery] decimal? rentabilidadeMinima)
        {
            var investimentos = await _investimentoRepository.SearchMultipleFiltersAsync(
                nome, tipo, status, rentabilidadeMinima);
            return Ok(investimentos);
        }

        // GET: Top mais rentáveis
        [HttpGet("top-rentaveis/{quantidade}")]
        public async Task<ActionResult<IEnumerable<Investimento>>> GetTopRentaveis(int quantidade)
        {
            var investimentos = await _investimentoRepository.GetTopRentaveisAsync(quantidade);
            return Ok(investimentos);
        }

        // GET: Estatísticas - Total investido
        [HttpGet("estatisticas/total-investido")]
        public async Task<ActionResult<object>> GetTotalInvestido()
        {
            var total = await _investimentoRepository.GetTotalInvestidoAsync();
            return Ok(new { totalInvestido = total });
        }

        // GET: Estatísticas - Total atual
        [HttpGet("estatisticas/total-atual")]
        public async Task<ActionResult<object>> GetTotalAtual()
        {
            var total = await _investimentoRepository.GetTotalAtualAsync();
            return Ok(new { totalAtual = total });
        }

        // GET: Estatísticas - Média de rentabilidade por tipo
        [HttpGet("estatisticas/media-rentabilidade-tipo/{tipo}")]
        public async Task<ActionResult<object>> GetMediaRentabilidadeByTipo(string tipo)
        {
            var media = await _investimentoRepository.GetMediaRentabilidadeByTipoAsync(tipo);
            return Ok(new { tipo, mediaRentabilidade = media });
        }

        // GET: Estatísticas - Contagem por tipo
        [HttpGet("estatisticas/count-tipo/{tipo}")]
        public async Task<ActionResult<object>> CountByTipo(string tipo)
        {
            var count = await _investimentoRepository.CountByTipoAsync(tipo);
            return Ok(new { tipo, quantidade = count });
        }
    }
}