using InvestimentosApp.Domain.Interfaces;
using InvestimentosApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestidoresController : ControllerBase
    {
        private readonly IInvestidorRepository _investidorRepository;

        public InvestidoresController(IInvestidorRepository investidorRepository)
        {
            _investidorRepository = investidorRepository;
        }

        // GET: Listar todos os investidores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Investidor>>> GetAll()
        {
            var investidores = await _investidorRepository.GetAllAsync();
            return Ok(investidores);
        }

        // GET: Obter investidor por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Investidor>> GetById(int id)
        {
            var investidor = await _investidorRepository.GetByIdAsync(id);
            if (investidor == null)
                return NotFound();

            return Ok(investidor);
        }

        // POST: Criar novo investidor
        [HttpPost]
        public async Task<ActionResult<Investidor>> Create(Investidor investidor)
        {
            var sucesso = await _investidorRepository.AddAsync(investidor);
            if (!sucesso)
                return BadRequest("Não foi possível criar o investidor");

            return CreatedAtAction(nameof(GetById), new { id = investidor.Id }, investidor);
        }

        // PUT: Atualizar investidor
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Investidor investidor)
        {
            if (id != investidor.Id)
                return BadRequest();

            var sucesso = await _investidorRepository.UpdateAsync(investidor);
            if (!sucesso)
                return NotFound();

            return NoContent();
        }

        // DELETE: Deletar investidor
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sucesso = await _investidorRepository.DeleteAsync(id);
            if (!sucesso)
                return NotFound();

            return NoContent();
        }

        // =================== ENDPOINTS DE PESQUISA AVANÇADA ===================

        // GET: Buscar por nome
        [HttpGet("buscar/nome/{nome}")]
        public async Task<ActionResult<IEnumerable<Investidor>>> SearchByName(string nome)
        {
            var investidores = await _investidorRepository.SearchByNameAsync(nome);
            return Ok(investidores);
        }

        // GET: Buscar por perfil de risco
        [HttpGet("buscar/perfil/{perfilRisco}")]
        public async Task<ActionResult<IEnumerable<Investidor>>> GetByPerfilRisco(string perfilRisco)
        {
            var investidores = await _investidorRepository.GetByPerfilRiscoAsync(perfilRisco);
            return Ok(investidores);
        }

        // GET: Buscar por faixa de saldo
        [HttpGet("buscar/saldo")]
        public async Task<ActionResult<IEnumerable<Investidor>>> GetBySaldoRange(
            [FromQuery] decimal saldoMinimo, 
            [FromQuery] decimal saldoMaximo)
        {
            var investidores = await _investidorRepository.GetBySaldoRangeAsync(saldoMinimo, saldoMaximo);
            return Ok(investidores);
        }

        // GET: Buscar por faixa de idade
        [HttpGet("buscar/idade")]
        public async Task<ActionResult<IEnumerable<Investidor>>> GetByIdadeRange(
            [FromQuery] int idadeMinima, 
            [FromQuery] int idadeMaxima)
        {
            var investidores = await _investidorRepository.GetByIdadeRangeAsync(idadeMinima, idadeMaxima);
            return Ok(investidores);
        }

        // GET: Busca avançada com múltiplos filtros
        [HttpGet("buscar/avancada")]
        public async Task<ActionResult<IEnumerable<Investidor>>> SearchAdvanced(
            [FromQuery] string? nome,
            [FromQuery] string? perfilRisco,
            [FromQuery] decimal? saldoMinimo,
            [FromQuery] decimal? saldoMaximo)
        {
            var investidores = await _investidorRepository.SearchMultipleFiltersAsync(nome, perfilRisco, saldoMinimo, saldoMaximo);
            return Ok(investidores);
        }

        // GET: Estatísticas - Total de saldo
        [HttpGet("estatisticas/total-saldo")]
        public async Task<ActionResult<object>> GetTotalSaldo()
        {
            var total = await _investidorRepository.GetTotalSaldoAsync();
            return Ok(new { totalSaldo = total });
        }

        // GET: Estatísticas - Contagem por perfil de risco
        [HttpGet("estatisticas/count-perfil/{perfilRisco}")]
        public async Task<ActionResult<object>> CountByPerfilRisco(string perfilRisco)
        {
            var count = await _investidorRepository.CountByPerfilRiscoAsync(perfilRisco);
            return Ok(new { perfilRisco, quantidade = count });
        }

        // GET: Estatísticas - Média de saldo por perfil
        [HttpGet("estatisticas/media-saldo-perfil/{perfilRisco}")]
        public async Task<ActionResult<object>> GetMediaSaldoByPerfil(string perfilRisco)
        {
            var media = await _investidorRepository.GetMediaSaldoByPerfilAsync(perfilRisco);
            return Ok(new { perfilRisco, mediaSaldo = media });
        }
    }
}
