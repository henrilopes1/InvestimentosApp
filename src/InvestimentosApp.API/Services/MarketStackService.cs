using InvestimentosApp.API.Models.ExternalAPIs;
using Newtonsoft.Json;
using System.Globalization;

namespace InvestimentosApp.API.Services
{
    public class MarketStackService : IMarketStackService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MarketStackService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public MarketStackService(HttpClient httpClient, ILogger<MarketStackService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
            
            _apiKey = _configuration["MarketStack:ApiKey"] ?? throw new InvalidOperationException("MarketStack API Key não configurada");
            _baseUrl = _configuration["MarketStack:BaseUrl"] ?? "https://api.marketstack.com/v1";
            
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<MarketStackEodResponse?> GetEndOfDayDataAsync(string symbols, string? exchange = null, 
            DateTime? dateFrom = null, DateTime? dateTo = null, int limit = 100, int offset = 0)
        {
            try
            {
                _logger.LogInformation("Buscando dados End-of-Day para símbolos: {Symbols}", symbols);

                var queryParams = new List<string>
                {
                    $"access_key={_apiKey}",
                    $"symbols={symbols}",
                    $"limit={limit}",
                    $"offset={offset}"
                };

                if (!string.IsNullOrEmpty(exchange))
                    queryParams.Add($"exchange={exchange}");

                if (dateFrom.HasValue)
                    queryParams.Add($"date_from={dateFrom.Value:yyyy-MM-dd}");

                if (dateTo.HasValue)
                    queryParams.Add($"date_to={dateTo.Value:yyyy-MM-dd}");

                var url = $"/eod?{string.Join("&", queryParams)}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MarketStackEodResponse>(content);
                }
                else
                {
                    _logger.LogWarning("Erro na requisição MarketStack EOD: {StatusCode}", response.StatusCode);
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Detalhes do erro: {Error}", errorContent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados End-of-Day para {Symbols}", symbols);
            }

            return null;
        }

        public async Task<MarketStackEodResponse?> GetLatestEndOfDayDataAsync(string symbols, string? exchange = null)
        {
            try
            {
                _logger.LogInformation("Buscando dados End-of-Day mais recentes para símbolos: {Symbols}", symbols);

                var queryParams = new List<string>
                {
                    $"access_key={_apiKey}",
                    $"symbols={symbols}"
                };

                if (!string.IsNullOrEmpty(exchange))
                    queryParams.Add($"exchange={exchange}");

                var url = $"/eod/latest?{string.Join("&", queryParams)}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MarketStackEodResponse>(content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados End-of-Day mais recentes para {Symbols}", symbols);
            }

            return null;
        }

        public async Task<MarketStackEodResponse?> GetEndOfDayDataByDateAsync(string symbols, DateTime date, string? exchange = null)
        {
            try
            {
                _logger.LogInformation("Buscando dados End-of-Day para símbolos: {Symbols} na data: {Date}", symbols, date);

                var queryParams = new List<string>
                {
                    $"access_key={_apiKey}",
                    $"symbols={symbols}"
                };

                if (!string.IsNullOrEmpty(exchange))
                    queryParams.Add($"exchange={exchange}");

                var url = $"/eod/{date:yyyy-MM-dd}?{string.Join("&", queryParams)}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MarketStackEodResponse>(content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados End-of-Day para {Symbols} na data {Date}", symbols, date);
            }

            return null;
        }

        public async Task<MarketStackIntradayResponse?> GetIntradayDataAsync(string symbols, string? exchange = null, 
            string interval = "1hour", DateTime? dateFrom = null, DateTime? dateTo = null, int limit = 100, int offset = 0)
        {
            try
            {
                _logger.LogInformation("Buscando dados Intraday para símbolos: {Symbols} com intervalo: {Interval}", symbols, interval);

                var queryParams = new List<string>
                {
                    $"access_key={_apiKey}",
                    $"symbols={symbols}",
                    $"interval={interval}",
                    $"limit={limit}",
                    $"offset={offset}"
                };

                if (!string.IsNullOrEmpty(exchange))
                    queryParams.Add($"exchange={exchange}");

                if (dateFrom.HasValue)
                    queryParams.Add($"date_from={dateFrom.Value:yyyy-MM-dd}");

                if (dateTo.HasValue)
                    queryParams.Add($"date_to={dateTo.Value:yyyy-MM-dd}");

                var url = $"/intraday?{string.Join("&", queryParams)}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MarketStackIntradayResponse>(content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados Intraday para {Symbols}", symbols);
            }

            return null;
        }

        public async Task<MarketStackIntradayResponse?> GetLatestIntradayDataAsync(string symbols, string? exchange = null, string interval = "1hour")
        {
            try
            {
                _logger.LogInformation("Buscando dados Intraday mais recentes para símbolos: {Symbols}", symbols);

                var queryParams = new List<string>
                {
                    $"access_key={_apiKey}",
                    $"symbols={symbols}",
                    $"interval={interval}"
                };

                if (!string.IsNullOrEmpty(exchange))
                    queryParams.Add($"exchange={exchange}");

                var url = $"/intraday/latest?{string.Join("&", queryParams)}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MarketStackIntradayResponse>(content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados Intraday mais recentes para {Symbols}", symbols);
            }

            return null;
        }

        public async Task<MarketStackTickersResponse?> GetTickersAsync(string? exchange = null, string? search = null, int limit = 100, int offset = 0)
        {
            try
            {
                _logger.LogInformation("Buscando tickers com filtros - Exchange: {Exchange}, Search: {Search}", exchange, search);

                var queryParams = new List<string>
                {
                    $"access_key={_apiKey}",
                    $"limit={limit}",
                    $"offset={offset}"
                };

                if (!string.IsNullOrEmpty(exchange))
                    queryParams.Add($"exchange={exchange}");

                if (!string.IsNullOrEmpty(search))
                    queryParams.Add($"search={search}");

                var url = $"/tickers?{string.Join("&", queryParams)}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MarketStackTickersResponse>(content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar tickers");
            }

            return null;
        }

        public async Task<MarketStackTickerData?> GetTickerInfoAsync(string symbol)
        {
            try
            {
                _logger.LogInformation("Buscando informações do ticker: {Symbol}", symbol);

                var url = $"/tickers/{symbol}?access_key={_apiKey}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MarketStackTickerData>(content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar informações do ticker {Symbol}", symbol);
            }

            return null;
        }

        public async Task<MarketStackExchangesResponse?> GetExchangesAsync(string? search = null, int limit = 100, int offset = 0)
        {
            try
            {
                _logger.LogInformation("Buscando exchanges com filtro: {Search}", search);

                var queryParams = new List<string>
                {
                    $"access_key={_apiKey}",
                    $"limit={limit}",
                    $"offset={offset}"
                };

                if (!string.IsNullOrEmpty(search))
                    queryParams.Add($"search={search}");

                var url = $"/exchanges?{string.Join("&", queryParams)}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MarketStackExchangesResponse>(content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar exchanges");
            }

            return null;
        }

        public async Task<MarketStackExchangeData?> GetExchangeInfoAsync(string mic)
        {
            try
            {
                _logger.LogInformation("Buscando informações da exchange: {MIC}", mic);

                var url = $"/exchanges/{mic}?access_key={_apiKey}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MarketStackExchangeData>(content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar informações da exchange {MIC}", mic);
            }

            return null;
        }

        public async Task<MarketStackDividendsResponse?> GetDividendsAsync(string symbols, DateTime? dateFrom = null, 
            DateTime? dateTo = null, int limit = 100, int offset = 0)
        {
            try
            {
                _logger.LogInformation("Buscando dividendos para símbolos: {Symbols}", symbols);

                var queryParams = new List<string>
                {
                    $"access_key={_apiKey}",
                    $"symbols={symbols}",
                    $"limit={limit}",
                    $"offset={offset}"
                };

                if (dateFrom.HasValue)
                    queryParams.Add($"date_from={dateFrom.Value:yyyy-MM-dd}");

                if (dateTo.HasValue)
                    queryParams.Add($"date_to={dateTo.Value:yyyy-MM-dd}");

                var url = $"/dividends?{string.Join("&", queryParams)}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MarketStackDividendsResponse>(content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dividendos para {Symbols}", symbols);
            }

            return null;
        }

        public async Task<MarketStackSplitsResponse?> GetSplitsAsync(string symbols, DateTime? dateFrom = null, 
            DateTime? dateTo = null, int limit = 100, int offset = 0)
        {
            try
            {
                _logger.LogInformation("Buscando splits para símbolos: {Symbols}", symbols);

                var queryParams = new List<string>
                {
                    $"access_key={_apiKey}",
                    $"symbols={symbols}",
                    $"limit={limit}",
                    $"offset={offset}"
                };

                if (dateFrom.HasValue)
                    queryParams.Add($"date_from={dateFrom.Value:yyyy-MM-dd}");

                if (dateTo.HasValue)
                    queryParams.Add($"date_to={dateTo.Value:yyyy-MM-dd}");

                var url = $"/splits?{string.Join("&", queryParams)}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MarketStackSplitsResponse>(content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar splits para {Symbols}", symbols);
            }

            return null;
        }
    }
}