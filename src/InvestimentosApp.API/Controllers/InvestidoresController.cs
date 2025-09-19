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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Investidor>>> GetAll()
        {
            var investidores = await _investidorRepository.GetAllAsync();
            return Ok(investidores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Investidor>> GetById(int id)
        {
            var investidor = await _investidorRepository.GetByIdAsync(id);
            if (investidor == null)
                return NotFound();

            return Ok(investidor);
        }

        [HttpPost]
        public async Task<ActionResult<Investidor>> Create(Investidor investidor)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return BadRequest(new { message = "Dados inválidos", errors = errors });
                }

                // Garantir que o ID seja 0 para novo registro
                investidor.Id = 0;

                var success = await _investidorRepository.AddAsync(investidor);
                if (!success)
                    return BadRequest(new { message = "Não foi possível criar o investidor" });

                return CreatedAtAction(nameof(GetById), new { id = investidor.Id }, investidor);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro interno", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Investidor investidor)
        {
            if (id != investidor.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _investidorRepository.UpdateAsync(investidor);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _investidorRepository.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}