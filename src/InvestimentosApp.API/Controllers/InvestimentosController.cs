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
    }
}