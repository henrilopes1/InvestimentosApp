using InvestimentosApp.API.Models.ExternalAPIs;
using InvestimentosApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlphaVantageController : ControllerBase
    {
        private readonly IAlphaVantageService _alphaVantageService;
        private readonly ILogger<AlphaVantageController> _logger;

        public AlphaVantageController(IAlphaVantageService alphaVantageService, ILogger<AlphaVantageController> logger)
        {
            _alphaVantageService = alphaVantageService;
            _logger = logger;
        }

        /// <summary>
        /// Obter cotação atual de uma ação usando Alpha Vantage
        /// </summary>
        /// <param name="symbol">Símbolo da ação (ex: AAPL, GOOGL, MSFT)</param>
        [HttpGet("quote/{symbol}")]
        public async Task<ActionResult<AlphaVantageStockData>> GetStockQuote(string symbol)
        {
            try
            {
                if (string.IsNullOrEmpty(symbol))
                    return BadRequest("Símbolo é obrigatório");

                var stockData = await _alphaVantageService.GetStockQuoteAsync(symbol.ToUpper());
                
                if (stockData == null)
                    return NotFound($"Dados não encontrados para o símbolo: {symbol}");

                return Ok(stockData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cotação para {Symbol}", symbol);
                return StatusCode(500, new { 
                    message = "Erro interno do servidor", 
                    details = ex.Message 
                });
            }
        }

        /// <summary>
        /// Obter dados históricos de uma ação
        /// </summary>
        /// <param name="symbol">Símbolo da ação</param>
        /// <param name="outputSize">Tamanho da saída (compact = últimos 100 dias, full = 20+ anos)</param>
        [HttpGet("historical/{symbol}")]
        public async Task<ActionResult<object>> GetHistoricalData(string symbol, [FromQuery] string outputSize = "compact")
        {
            try
            {
                if (string.IsNullOrEmpty(symbol))
                    return BadRequest("Símbolo é obrigatório");

                if (outputSize != "compact" && outputSize != "full")
                    return BadRequest("outputSize deve ser 'compact' ou 'full'");

                var historicalData = await _alphaVantageService.GetDailyTimeSeriesAsync(symbol.ToUpper(), outputSize);
                
                if (historicalData?.TimeSeriesDaily == null)
                    return NotFound($"Dados históricos não encontrados para: {symbol}");

                // Processar e ordenar dados
                var processedData = historicalData.TimeSeriesDaily
                    .Select(kvp => new
                    {
                        date = kvp.Key,
                        open = kvp.Value.Open,
                        high = kvp.Value.High,
                        low = kvp.Value.Low,
                        close = kvp.Value.Close,
                        volume = kvp.Value.Volume
                    })
                    .OrderByDescending(x => x.date)
                    .Take(100) // Limitar para não sobrecarregar
                    .ToList();

                return Ok(new
                {
                    symbol = symbol.ToUpper(),
                    metadata = historicalData.MetaData,
                    dataCount = processedData.Count,
                    timeSeries = processedData
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados históricos para {Symbol}", symbol);
                return StatusCode(500, new { 
                    message = "Erro interno do servidor", 
                    details = ex.Message 
                });
            }
        }

        /// <summary>
        /// Buscar símbolos de ações por palavra-chave
        /// </summary>
        /// <param name="keywords">Palavras-chave para busca (ex: Apple, Microsoft)</param>
        [HttpGet("search")]
        public async Task<ActionResult<List<AlphaVantageSearchMatch>>> SearchSymbols([FromQuery] string keywords)
        {
            try
            {
                if (string.IsNullOrEmpty(keywords))
                    return BadRequest("Palavras-chave são obrigatórias");

                if (keywords.Length < 2)
                    return BadRequest("Palavras-chave devem ter pelo menos 2 caracteres");

                var searchResults = await _alphaVantageService.SearchSymbolAsync(keywords);
                
                if (searchResults == null || !searchResults.Any())
                    return NotFound($"Nenhum símbolo encontrado para: {keywords}");

                return Ok(searchResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar símbolos com palavras-chave: {Keywords}", keywords);
                return StatusCode(500, new { 
                    message = "Erro interno do servidor", 
                    details = ex.Message 
                });
            }
        }

        /// <summary>
        /// Obter indicador técnico (SMA, EMA, RSI)
        /// </summary>
        /// <param name="symbol">Símbolo da ação</param>
        /// <param name="indicator">Tipo de indicador (SMA, EMA, RSI, MACD)</param>
        /// <param name="interval">Intervalo (daily, weekly, monthly)</param>
        /// <param name="timePeriod">Período de tempo (padrão: 20)</param>
        [HttpGet("technical/{symbol}/{indicator}")]
        public async Task<ActionResult<object>> GetTechnicalIndicator(
            string symbol, 
            string indicator,
            [FromQuery] string interval = "daily",
            [FromQuery] int timePeriod = 20)
        {
            try
            {
                if (string.IsNullOrEmpty(symbol))
                    return BadRequest("Símbolo é obrigatório");

                var validIndicators = new[] { "SMA", "EMA", "RSI", "MACD", "STOCH", "BBANDS" };
                if (!validIndicators.Contains(indicator.ToUpper()))
                    return BadRequest($"Indicador deve ser um dos seguintes: {string.Join(", ", validIndicators)}");

                var validIntervals = new[] { "daily", "weekly", "monthly" };
                if (!validIntervals.Contains(interval.ToLower()))
                    return BadRequest($"Intervalo deve ser um dos seguintes: {string.Join(", ", validIntervals)}");

                if (timePeriod < 1 || timePeriod > 200)
                    return BadRequest("Período deve estar entre 1 e 200");

                var technicalData = await _alphaVantageService.GetTechnicalIndicatorAsync(
                    symbol.ToUpper(), 
                    indicator.ToUpper(), 
                    interval.ToLower(), 
                    timePeriod);
                
                if (technicalData == null)
                    return NotFound($"Dados do indicador {indicator} não encontrados para: {symbol}");

                return Ok(new
                {
                    symbol = symbol.ToUpper(),
                    indicator = indicator.ToUpper(),
                    interval,
                    timePeriod,
                    metadata = technicalData.MetaData,
                    data = technicalData
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar indicador técnico {Indicator} para {Symbol}", indicator, symbol);
                return StatusCode(500, new { 
                    message = "Erro interno do servidor", 
                    details = ex.Message 
                });
            }
        }

        /// <summary>
        /// Comparar múltiplas ações usando Alpha Vantage
        /// </summary>
        /// <param name="symbols">Símbolos separados por vírgula (ex: AAPL,GOOGL,MSFT)</param>
        [HttpGet("compare")]
        public async Task<ActionResult<object>> CompareStocks([FromQuery] string symbols)
        {
            try
            {
                if (string.IsNullOrEmpty(symbols))
                    return BadRequest("Símbolos são obrigatórios");

                var symbolList = symbols.Split(',')
                    .Select(s => s.Trim().ToUpper())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Distinct()
                    .ToList();

                if (!symbolList.Any())
                    return BadRequest("Nenhum símbolo válido fornecido");

                if (symbolList.Count > 10)
                    return BadRequest("Máximo de 10 símbolos para comparação");

                // Buscar dados de todas as ações em paralelo
                var tasks = symbolList.Select(symbol => _alphaVantageService.GetStockQuoteAsync(symbol));
                var results = await Task.WhenAll(tasks);

                var comparison = results
                    .Where(r => r != null)
                    .Select(stock => new
                    {
                        symbol = stock!.Symbol,
                        companyName = stock.CompanyName,
                        currentPrice = stock.CurrentPrice,
                        change = stock.Change,
                        changePercent = stock.ChangePercent,
                        volume = stock.Volume,
                        lastUpdated = stock.LastUpdated
                    })
                    .OrderByDescending(s => s.changePercent)
                    .ToList();

                if (!comparison.Any())
                    return NotFound("Não foi possível obter dados para nenhum dos símbolos fornecidos");

                return Ok(new
                {
                    requestedSymbols = symbolList,
                    successfulQuotes = comparison.Count,
                    comparison,
                    summary = new
                    {
                        bestPerformer = comparison.FirstOrDefault(),
                        worstPerformer = comparison.LastOrDefault(),
                        averageChange = comparison.Any() ? comparison.Average(c => (double)c.changePercent) : 0
                    },
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao comparar ações: {Symbols}", symbols);
                return StatusCode(500, new { 
                    message = "Erro interno do servidor", 
                    details = ex.Message 
                });
            }
        }

        /// <summary>
        /// Obter análise completa de uma ação (cotação + dados históricos + indicadores)
        /// </summary>
        /// <param name="symbol">Símbolo da ação</param>
        [HttpGet("analysis/{symbol}")]
        public async Task<ActionResult<object>> GetCompleteAnalysis(string symbol)
        {
            try
            {
                if (string.IsNullOrEmpty(symbol))
                    return BadRequest("Símbolo é obrigatório");

                // Buscar dados em paralelo
                var quoteTask = _alphaVantageService.GetStockQuoteAsync(symbol.ToUpper());
                var historicalTask = _alphaVantageService.GetDailyTimeSeriesAsync(symbol.ToUpper(), "compact");
                var smaTask = _alphaVantageService.GetTechnicalIndicatorAsync(symbol.ToUpper(), "SMA", "daily", 20);
                var rsiTask = _alphaVantageService.GetTechnicalIndicatorAsync(symbol.ToUpper(), "RSI", "daily", 14);

                await Task.WhenAll(quoteTask, historicalTask, smaTask, rsiTask);

                var quote = await quoteTask;
                var historical = await historicalTask;
                var sma = await smaTask;
                var rsi = await rsiTask;

                if (quote == null)
                    return NotFound($"Dados não encontrados para o símbolo: {symbol}");

                // Processar dados históricos (últimos 30 dias)
                var recentHistory = historical?.TimeSeriesDaily?
                    .OrderByDescending(kvp => kvp.Key)
                    .Take(30)
                    .Select(kvp => new
                    {
                        date = kvp.Key,
                        close = decimal.TryParse(kvp.Value.Close, out var close) ? close : 0,
                        volume = long.TryParse(kvp.Value.Volume, out var vol) ? vol : 0
                    })
                    .ToList();

                var analysis = new
                {
                    symbol = symbol.ToUpper(),
                    timestamp = DateTime.UtcNow,
                    currentQuote = quote,
                    technicalAnalysis = new
                    {
                        trend = DetermineTrend(quote),
                        recommendation = GenerateRecommendation(quote),
                        riskLevel = CalculateRiskLevel(quote),
                        sma20 = ExtractIndicatorValue(sma, "SMA"),
                        rsi14 = ExtractIndicatorValue(rsi, "RSI")
                    },
                    recentPerformance = new
                    {
                        last30Days = recentHistory,
                        averageVolume = recentHistory?.Any() == true ? recentHistory.Average(h => (double)h.volume) : 0,
                        priceRange30d = recentHistory?.Any() == true ? new
                        {
                            high = recentHistory.Max(h => h.close),
                            low = recentHistory.Min(h => h.close)
                        } : null
                    }
                };

                return Ok(analysis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar análise completa para {Symbol}", symbol);
                return StatusCode(500, new { 
                    message = "Erro interno do servidor", 
                    details = ex.Message 
                });
            }
        }

        // Métodos auxiliares para análise
        private string DetermineTrend(AlphaVantageStockData quote)
        {
            if (quote.ChangePercent > 2) return "Forte Alta";
            if (quote.ChangePercent > 0) return "Alta";
            if (quote.ChangePercent > -2) return "Baixa";
            return "Forte Baixa";
        }

        private string GenerateRecommendation(AlphaVantageStockData quote)
        {
            if (quote.ChangePercent > 5) return "Atenção - Possível sobrecompra";
            if (quote.ChangePercent > 2) return "Comprar";
            if (quote.ChangePercent > -2) return "Manter";
            if (quote.ChangePercent > -5) return "Vender";
            return "Evitar";
        }

        private string CalculateRiskLevel(AlphaVantageStockData quote)
        {
            var absChange = Math.Abs(quote.ChangePercent);
            if (absChange > 5) return "Alto";
            if (absChange > 2) return "Médio";
            return "Baixo";
        }

        private decimal ExtractIndicatorValue(AlphaVantageTechnicalIndicatorResponse? indicator, string type)
        {
            // Simplificação - em implementação real, extrairia o valor mais recente
            return 0;
        }
    }
}