using Microsoft.AspNetCore.Mvc;
using InvestimentosApp.API.Services;
using InvestimentosApp.API.Models.ExternalAPIs;

namespace InvestimentosApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketStackController : ControllerBase
    {
        private readonly IMarketStackService _marketStackService;
        private readonly ILogger<MarketStackController> _logger;

        public MarketStackController(IMarketStackService marketStackService, ILogger<MarketStackController> logger)
        {
            _marketStackService = marketStackService;
            _logger = logger;
        }

        /// <summary>
        /// Obtém dados de fim de dia (End-of-Day) para símbolos específicos
        /// </summary>
        /// <param name="symbols">Símbolos separados por vírgula (ex: AAPL,MSFT)</param>
        /// <param name="exchange">Código da bolsa (opcional)</param>
        /// <param name="dateFrom">Data inicial (formato: yyyy-MM-dd)</param>
        /// <param name="dateTo">Data final (formato: yyyy-MM-dd)</param>
        /// <param name="limit">Limite de resultados (padrão: 100)</param>
        /// <param name="offset">Offset para paginação (padrão: 0)</param>
        [HttpGet("eod")]
        public async Task<ActionResult<MarketStackEodResponse>> GetEndOfDayData(
            [FromQuery] string symbols,
            [FromQuery] string? exchange = null,
            [FromQuery] DateTime? dateFrom = null,
            [FromQuery] DateTime? dateTo = null,
            [FromQuery] int limit = 100,
            [FromQuery] int offset = 0)
        {
            if (string.IsNullOrWhiteSpace(symbols))
            {
                return BadRequest("Símbolos são obrigatórios");
            }

            try
            {
                var result = await _marketStackService.GetEndOfDayDataAsync(symbols, exchange, dateFrom, dateTo, limit, offset);
                
                if (result == null)
                {
                    return NotFound("Nenhum dado encontrado para os símbolos especificados");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados End-of-Day para símbolos: {Symbols}", symbols);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obtém os dados de fim de dia mais recentes para símbolos específicos
        /// </summary>
        /// <param name="symbols">Símbolos separados por vírgula (ex: AAPL,MSFT)</param>
        /// <param name="exchange">Código da bolsa (opcional)</param>
        [HttpGet("eod/latest")]
        public async Task<ActionResult<MarketStackEodResponse>> GetLatestEndOfDayData(
            [FromQuery] string symbols,
            [FromQuery] string? exchange = null)
        {
            if (string.IsNullOrWhiteSpace(symbols))
            {
                return BadRequest("Símbolos são obrigatórios");
            }

            try
            {
                var result = await _marketStackService.GetLatestEndOfDayDataAsync(symbols, exchange);
                
                if (result == null)
                {
                    return NotFound("Nenhum dado encontrado para os símbolos especificados");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados End-of-Day mais recentes para símbolos: {Symbols}", symbols);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obtém dados intraday para símbolos específicos
        /// </summary>
        /// <param name="symbols">Símbolos separados por vírgula (ex: AAPL,MSFT)</param>
        /// <param name="exchange">Código da bolsa (opcional)</param>
        /// <param name="interval">Intervalo de dados (1min, 5min, 10min, 15min, 30min, 1hour, 3hour, 6hour, 12hour, 24hour)</param>
        /// <param name="dateFrom">Data inicial (formato: yyyy-MM-dd)</param>
        /// <param name="dateTo">Data final (formato: yyyy-MM-dd)</param>
        /// <param name="limit">Limite de resultados (padrão: 100)</param>
        /// <param name="offset">Offset para paginação (padrão: 0)</param>
        [HttpGet("intraday")]
        public async Task<ActionResult<MarketStackIntradayResponse>> GetIntradayData(
            [FromQuery] string symbols,
            [FromQuery] string? exchange = null,
            [FromQuery] string interval = "1hour",
            [FromQuery] DateTime? dateFrom = null,
            [FromQuery] DateTime? dateTo = null,
            [FromQuery] int limit = 100,
            [FromQuery] int offset = 0)
        {
            if (string.IsNullOrWhiteSpace(symbols))
            {
                return BadRequest("Símbolos são obrigatórios");
            }

            var validIntervals = new[] { "1min", "5min", "10min", "15min", "30min", "1hour", "3hour", "6hour", "12hour", "24hour" };
            if (!validIntervals.Contains(interval))
            {
                return BadRequest($"Intervalo inválido. Valores válidos: {string.Join(", ", validIntervals)}");
            }

            try
            {
                var result = await _marketStackService.GetIntradayDataAsync(symbols, exchange, interval, dateFrom, dateTo, limit, offset);
                
                if (result == null)
                {
                    return NotFound("Nenhum dado encontrado para os símbolos especificados");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados intraday para símbolos: {Symbols}", symbols);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obtém os dados intraday mais recentes para símbolos específicos
        /// </summary>
        /// <param name="symbols">Símbolos separados por vírgula (ex: AAPL,MSFT)</param>
        /// <param name="exchange">Código da bolsa (opcional)</param>
        /// <param name="interval">Intervalo de dados (padrão: 1hour)</param>
        [HttpGet("intraday/latest")]
        public async Task<ActionResult<MarketStackIntradayResponse>> GetLatestIntradayData(
            [FromQuery] string symbols,
            [FromQuery] string? exchange = null,
            [FromQuery] string interval = "1hour")
        {
            if (string.IsNullOrWhiteSpace(symbols))
            {
                return BadRequest("Símbolos são obrigatórios");
            }

            try
            {
                var result = await _marketStackService.GetLatestIntradayDataAsync(symbols, exchange, interval);
                
                if (result == null)
                {
                    return NotFound("Nenhum dado encontrado para os símbolos especificados");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados intraday mais recentes para símbolos: {Symbols}", symbols);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Busca por tickers disponíveis
        /// </summary>
        /// <param name="exchange">Código da bolsa para filtrar (opcional)</param>
        /// <param name="search">Termo de busca para nome ou símbolo (opcional)</param>
        /// <param name="limit">Limite de resultados (padrão: 100)</param>
        /// <param name="offset">Offset para paginação (padrão: 0)</param>
        [HttpGet("tickers")]
        public async Task<ActionResult<MarketStackTickersResponse>> GetTickers(
            [FromQuery] string? exchange = null,
            [FromQuery] string? search = null,
            [FromQuery] int limit = 100,
            [FromQuery] int offset = 0)
        {
            try
            {
                var result = await _marketStackService.GetTickersAsync(exchange, search, limit, offset);
                
                if (result == null)
                {
                    return NotFound("Nenhum ticker encontrado com os filtros especificados");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar tickers");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obtém informações específicas de um ticker
        /// </summary>
        /// <param name="symbol">Símbolo do ticker (ex: AAPL)</param>
        [HttpGet("tickers/{symbol}")]
        public async Task<ActionResult<MarketStackTickerData>> GetTickerInfo(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return BadRequest("Símbolo é obrigatório");
            }

            try
            {
                var result = await _marketStackService.GetTickerInfoAsync(symbol);
                
                if (result == null)
                {
                    return NotFound($"Ticker '{symbol}' não encontrado");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar informações do ticker: {Symbol}", symbol);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Lista todas as bolsas de valores disponíveis
        /// </summary>
        /// <param name="search">Termo de busca para nome ou MIC da bolsa (opcional)</param>
        /// <param name="limit">Limite de resultados (padrão: 100)</param>
        /// <param name="offset">Offset para paginação (padrão: 0)</param>
        [HttpGet("exchanges")]
        public async Task<ActionResult<MarketStackExchangesResponse>> GetExchanges(
            [FromQuery] string? search = null,
            [FromQuery] int limit = 100,
            [FromQuery] int offset = 0)
        {
            try
            {
                var result = await _marketStackService.GetExchangesAsync(search, limit, offset);
                
                if (result == null)
                {
                    return NotFound("Nenhuma bolsa encontrada");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar bolsas de valores");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obtém informações específicas de uma bolsa de valores
        /// </summary>
        /// <param name="mic">Código MIC da bolsa (ex: XNAS para NASDAQ)</param>
        [HttpGet("exchanges/{mic}")]
        public async Task<ActionResult<MarketStackExchangeData>> GetExchangeInfo(string mic)
        {
            if (string.IsNullOrWhiteSpace(mic))
            {
                return BadRequest("Código MIC é obrigatório");
            }

            try
            {
                var result = await _marketStackService.GetExchangeInfoAsync(mic);
                
                if (result == null)
                {
                    return NotFound($"Bolsa com código MIC '{mic}' não encontrada");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar informações da bolsa: {MIC}", mic);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obtém dados de dividendos para símbolos específicos
        /// </summary>
        /// <param name="symbols">Símbolos separados por vírgula (ex: AAPL,MSFT)</param>
        /// <param name="dateFrom">Data inicial (formato: yyyy-MM-dd)</param>
        /// <param name="dateTo">Data final (formato: yyyy-MM-dd)</param>
        /// <param name="limit">Limite de resultados (padrão: 100)</param>
        /// <param name="offset">Offset para paginação (padrão: 0)</param>
        [HttpGet("dividends")]
        public async Task<ActionResult<MarketStackDividendsResponse>> GetDividends(
            [FromQuery] string symbols,
            [FromQuery] DateTime? dateFrom = null,
            [FromQuery] DateTime? dateTo = null,
            [FromQuery] int limit = 100,
            [FromQuery] int offset = 0)
        {
            if (string.IsNullOrWhiteSpace(symbols))
            {
                return BadRequest("Símbolos são obrigatórios");
            }

            try
            {
                var result = await _marketStackService.GetDividendsAsync(symbols, dateFrom, dateTo, limit, offset);
                
                if (result == null)
                {
                    return NotFound("Nenhum dividendo encontrado para os símbolos especificados");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dividendos para símbolos: {Symbols}", symbols);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obtém dados de splits para símbolos específicos
        /// </summary>
        /// <param name="symbols">Símbolos separados por vírgula (ex: AAPL,MSFT)</param>
        /// <param name="dateFrom">Data inicial (formato: yyyy-MM-dd)</param>
        /// <param name="dateTo">Data final (formato: yyyy-MM-dd)</param>
        /// <param name="limit">Limite de resultados (padrão: 100)</param>
        /// <param name="offset">Offset para paginação (padrão: 0)</param>
        [HttpGet("splits")]
        public async Task<ActionResult<MarketStackSplitsResponse>> GetSplits(
            [FromQuery] string symbols,
            [FromQuery] DateTime? dateFrom = null,
            [FromQuery] DateTime? dateTo = null,
            [FromQuery] int limit = 100,
            [FromQuery] int offset = 0)
        {
            if (string.IsNullOrWhiteSpace(symbols))
            {
                return BadRequest("Símbolos são obrigatórios");
            }

            try
            {
                var result = await _marketStackService.GetSplitsAsync(symbols, dateFrom, dateTo, limit, offset);
                
                if (result == null)
                {
                    return NotFound("Nenhum split encontrado para os símbolos especificados");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar splits para símbolos: {Symbols}", symbols);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}