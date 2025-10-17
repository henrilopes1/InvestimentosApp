using Newtonsoft.Json;

namespace InvestimentosApp.API.Models.ExternalAPIs
{
    // Modelo para Alpha Vantage Global Quote (cotação atual)
    public class AlphaVantageQuoteResponse
    {
        [JsonProperty("Global Quote")]
        public AlphaVantageGlobalQuote? GlobalQuote { get; set; }
    }

    public class AlphaVantageGlobalQuote
    {
        [JsonProperty("01. symbol")]
        public string? Symbol { get; set; }

        [JsonProperty("02. open")]
        public string? Open { get; set; }

        [JsonProperty("03. high")]
        public string? High { get; set; }

        [JsonProperty("04. low")]
        public string? Low { get; set; }

        [JsonProperty("05. price")]
        public string? Price { get; set; }

        [JsonProperty("06. volume")]
        public string? Volume { get; set; }

        [JsonProperty("07. latest trading day")]
        public string? LatestTradingDay { get; set; }

        [JsonProperty("08. previous close")]
        public string? PreviousClose { get; set; }

        [JsonProperty("09. change")]
        public string? Change { get; set; }

        [JsonProperty("10. change percent")]
        public string? ChangePercent { get; set; }
    }

    // Modelo para dados históricos (Time Series Daily)
    public class AlphaVantageTimeSeriesResponse
    {
        [JsonProperty("Meta Data")]
        public AlphaVantageMetaData? MetaData { get; set; }

        [JsonProperty("Time Series (Daily)")]
        public Dictionary<string, AlphaVantageDailyData>? TimeSeriesDaily { get; set; }
    }

    public class AlphaVantageMetaData
    {
        [JsonProperty("1. Information")]
        public string? Information { get; set; }

        [JsonProperty("2. Symbol")]
        public string? Symbol { get; set; }

        [JsonProperty("3. Last Refreshed")]
        public string? LastRefreshed { get; set; }

        [JsonProperty("4. Output Size")]
        public string? OutputSize { get; set; }

        [JsonProperty("5. Time Zone")]
        public string? TimeZone { get; set; }
    }

    public class AlphaVantageDailyData
    {
        [JsonProperty("1. open")]
        public string? Open { get; set; }

        [JsonProperty("2. high")]
        public string? High { get; set; }

        [JsonProperty("3. low")]
        public string? Low { get; set; }

        [JsonProperty("4. close")]
        public string? Close { get; set; }

        [JsonProperty("5. volume")]
        public string? Volume { get; set; }
    }

    // Modelo para busca de símbolos
    public class AlphaVantageSearchResponse
    {
        [JsonProperty("bestMatches")]
        public List<AlphaVantageSearchMatch>? BestMatches { get; set; }
    }

    public class AlphaVantageSearchMatch
    {
        [JsonProperty("1. symbol")]
        public string? Symbol { get; set; }

        [JsonProperty("2. name")]
        public string? Name { get; set; }

        [JsonProperty("3. type")]
        public string? Type { get; set; }

        [JsonProperty("4. region")]
        public string? Region { get; set; }

        [JsonProperty("5. marketOpen")]
        public string? MarketOpen { get; set; }

        [JsonProperty("6. marketClose")]
        public string? MarketClose { get; set; }

        [JsonProperty("7. timezone")]
        public string? Timezone { get; set; }

        [JsonProperty("8. currency")]
        public string? Currency { get; set; }

        [JsonProperty("9. matchScore")]
        public string? MatchScore { get; set; }
    }

    // Modelo para indicadores técnicos (SMA, EMA, RSI, etc.)
    public class AlphaVantageTechnicalIndicatorResponse
    {
        [JsonProperty("Meta Data")]
        public Dictionary<string, string>? MetaData { get; set; }

        [JsonProperty("Technical Analysis: SMA")]
        public Dictionary<string, AlphaVantageTechnicalData>? SMA { get; set; }

        [JsonProperty("Technical Analysis: EMA")]
        public Dictionary<string, AlphaVantageTechnicalData>? EMA { get; set; }

        [JsonProperty("Technical Analysis: RSI")]
        public Dictionary<string, AlphaVantageTechnicalData>? RSI { get; set; }
    }

    public class AlphaVantageTechnicalData
    {
        [JsonProperty("SMA")]
        public string? SMA { get; set; }

        [JsonProperty("EMA")]
        public string? EMA { get; set; }

        [JsonProperty("RSI")]
        public string? RSI { get; set; }
    }

    // Modelo para resposta de erro
    public class AlphaVantageErrorResponse
    {
        [JsonProperty("Error Message")]
        public string? ErrorMessage { get; set; }

        [JsonProperty("Note")]
        public string? Note { get; set; }
    }

    // Modelo consolidado para retorno da API
    public class AlphaVantageStockData
    {
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal PreviousClose { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercent { get; set; }
        public long Volume { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Currency { get; set; } = "USD";
        public List<AlphaVantageHistoricalData>? HistoricalData { get; set; }
    }

    public class AlphaVantageHistoricalData
    {
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
    }
}