using InvestimentosApp.API.Models.ExternalAPIs;
using Newtonsoft.Json;
using System.Globalization;

namespace InvestimentosApp.API.Services
{
    public interface IAlphaVantageService
    {
        Task<AlphaVantageStockData?> GetStockQuoteAsync(string symbol);
        Task<AlphaVantageTimeSeriesResponse?> GetDailyTimeSeriesAsync(string symbol, string outputSize = "compact");
        Task<List<AlphaVantageSearchMatch>?> SearchSymbolAsync(string keywords);
        Task<AlphaVantageTechnicalIndicatorResponse?> GetTechnicalIndicatorAsync(string symbol, string indicator, string interval = "daily", int timePeriod = 20);
        Task<List<AlphaVantageStockData>?> GetTopGainersLosersAsync();
    }

    public class AlphaVantageService : IAlphaVantageService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AlphaVantageService> _logger;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://www.alphavantage.co/query";

        public AlphaVantageService(HttpClient httpClient, ILogger<AlphaVantageService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            
            // Buscar a API Key do configuration (appsettings.json ou variável de ambiente)
            _apiKey = configuration["AlphaVantage:ApiKey"] ?? "demo"; // demo é limitada
            
            // Configurar timeout
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "InvestimentosApp/1.0");
        }

        public async Task<AlphaVantageStockData?> GetStockQuoteAsync(string symbol)
        {
            try
            {
                _logger.LogInformation("Buscando cotação para símbolo: {Symbol}", symbol);

                var url = $"{_baseUrl}?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}";
                
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Erro HTTP ao buscar cotação: {StatusCode}", response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                
                // Verificar se há erro na resposta
                if (content.Contains("Error Message") || content.Contains("Invalid API call"))
                {
                    _logger.LogError("Erro na API Alpha Vantage: {Content}", content);
                    return null;
                }

                var quoteResponse = JsonConvert.DeserializeObject<AlphaVantageQuoteResponse>(content);
                
                if (quoteResponse?.GlobalQuote == null)
                {
                    _logger.LogWarning("Resposta vazia da Alpha Vantage para símbolo: {Symbol}", symbol);
                    return null;
                }

                // Converter para nosso modelo
                return ConvertToStockData(quoteResponse.GlobalQuote, symbol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cotação para símbolo: {Symbol}", symbol);
                return null;
            }
        }

        public async Task<AlphaVantageTimeSeriesResponse?> GetDailyTimeSeriesAsync(string symbol, string outputSize = "compact")
        {
            try
            {
                _logger.LogInformation("Buscando dados históricos para símbolo: {Symbol}", symbol);

                var url = $"{_baseUrl}?function=TIME_SERIES_DAILY&symbol={symbol}&outputsize={outputSize}&apikey={_apiKey}";
                
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Erro HTTP ao buscar dados históricos: {StatusCode}", response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                
                if (content.Contains("Error Message"))
                {
                    _logger.LogError("Erro na API Alpha Vantage: {Content}", content);
                    return null;
                }

                return JsonConvert.DeserializeObject<AlphaVantageTimeSeriesResponse>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados históricos para símbolo: {Symbol}", symbol);
                return null;
            }
        }

        public async Task<List<AlphaVantageSearchMatch>?> SearchSymbolAsync(string keywords)
        {
            try
            {
                _logger.LogInformation("Buscando símbolos com palavras-chave: {Keywords}", keywords);

                var url = $"{_baseUrl}?function=SYMBOL_SEARCH&keywords={Uri.EscapeDataString(keywords)}&apikey={_apiKey}";
                
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Erro HTTP ao buscar símbolos: {StatusCode}", response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                
                if (content.Contains("Error Message"))
                {
                    _logger.LogError("Erro na API Alpha Vantage: {Content}", content);
                    return null;
                }

                var searchResponse = JsonConvert.DeserializeObject<AlphaVantageSearchResponse>(content);
                return searchResponse?.BestMatches;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar símbolos com palavras-chave: {Keywords}", keywords);
                return null;
            }
        }

        public async Task<AlphaVantageTechnicalIndicatorResponse?> GetTechnicalIndicatorAsync(string symbol, string indicator, string interval = "daily", int timePeriod = 20)
        {
            try
            {
                _logger.LogInformation("Buscando indicador técnico {Indicator} para símbolo: {Symbol}", indicator, symbol);

                var url = $"{_baseUrl}?function={indicator}&symbol={symbol}&interval={interval}&time_period={timePeriod}&series_type=close&apikey={_apiKey}";
                
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Erro HTTP ao buscar indicador técnico: {StatusCode}", response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                
                if (content.Contains("Error Message"))
                {
                    _logger.LogError("Erro na API Alpha Vantage: {Content}", content);
                    return null;
                }

                return JsonConvert.DeserializeObject<AlphaVantageTechnicalIndicatorResponse>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar indicador técnico {Indicator} para símbolo: {Symbol}", indicator, symbol);
                return null;
            }
        }

        public async Task<List<AlphaVantageStockData>?> GetTopGainersLosersAsync()
        {
            try
            {
                _logger.LogInformation("Buscando top gainers/losers");

                var url = $"{_baseUrl}?function=TOP_GAINERS_LOSERS&apikey={_apiKey}";
                
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Erro HTTP ao buscar top gainers/losers: {StatusCode}", response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                
                if (content.Contains("Error Message"))
                {
                    _logger.LogError("Erro na API Alpha Vantage: {Content}", content);
                    return null;
                }

                // A resposta desta função tem formato diferente, precisaria de modelo específico
                // Por ora, retornamos lista vazia
                return new List<AlphaVantageStockData>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar top gainers/losers");
                return null;
            }
        }

        private AlphaVantageStockData ConvertToStockData(AlphaVantageGlobalQuote quote, string symbol)
        {
            var culture = CultureInfo.InvariantCulture;
            
            return new AlphaVantageStockData
            {
                Symbol = quote.Symbol ?? symbol,
                CurrentPrice = ParseDecimal(quote.Price),
                OpenPrice = ParseDecimal(quote.Open),
                HighPrice = ParseDecimal(quote.High),
                LowPrice = ParseDecimal(quote.Low),
                PreviousClose = ParseDecimal(quote.PreviousClose),
                Change = ParseDecimal(quote.Change),
                ChangePercent = ParsePercentage(quote.ChangePercent),
                Volume = ParseLong(quote.Volume),
                LastUpdated = ParseDateTime(quote.LatestTradingDay),
                Currency = "USD" // Alpha Vantage geralmente retorna em USD
            };
        }

        private decimal ParseDecimal(string? value)
        {
            if (string.IsNullOrEmpty(value)) return 0;
            
            if (decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
                return result;
            
            return 0;
        }

        private decimal ParsePercentage(string? value)
        {
            if (string.IsNullOrEmpty(value)) return 0;
            
            // Remove % se existir
            var cleanValue = value.Replace("%", "").Trim();
            
            if (decimal.TryParse(cleanValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
                return result;
            
            return 0;
        }

        private long ParseLong(string? value)
        {
            if (string.IsNullOrEmpty(value)) return 0;
            
            if (long.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
                return result;
            
            return 0;
        }

        private DateTime ParseDateTime(string? value)
        {
            if (string.IsNullOrEmpty(value)) return DateTime.UtcNow;
            
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                return result;
            
            return DateTime.UtcNow;
        }
    }
}