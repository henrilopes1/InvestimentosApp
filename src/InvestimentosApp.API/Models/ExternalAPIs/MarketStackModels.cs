using Newtonsoft.Json;

namespace InvestimentosApp.API.Models.ExternalAPIs
{
    // Modelo para End-of-Day Data
    public class MarketStackEodResponse
    {
        [JsonProperty("pagination")]
        public MarketStackPagination? Pagination { get; set; }

        [JsonProperty("data")]
        public List<MarketStackEodData>? Data { get; set; }
    }

    public class MarketStackEodData
    {
        [JsonProperty("open")]
        public decimal? Open { get; set; }

        [JsonProperty("high")]
        public decimal? High { get; set; }

        [JsonProperty("low")]
        public decimal? Low { get; set; }

        [JsonProperty("close")]
        public decimal? Close { get; set; }

        [JsonProperty("volume")]
        public decimal? Volume { get; set; }

        [JsonProperty("adj_high")]
        public decimal? AdjHigh { get; set; }

        [JsonProperty("adj_low")]
        public decimal? AdjLow { get; set; }

        [JsonProperty("adj_close")]
        public decimal? AdjClose { get; set; }

        [JsonProperty("adj_open")]
        public decimal? AdjOpen { get; set; }

        [JsonProperty("adj_volume")]
        public decimal? AdjVolume { get; set; }

        [JsonProperty("split_factor")]
        public decimal? SplitFactor { get; set; }

        [JsonProperty("dividend")]
        public decimal? Dividend { get; set; }

        [JsonProperty("symbol")]
        public string? Symbol { get; set; }

        [JsonProperty("exchange")]
        public string? Exchange { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }
    }

    // Modelo para Intraday Data
    public class MarketStackIntradayResponse
    {
        [JsonProperty("pagination")]
        public MarketStackPagination? Pagination { get; set; }

        [JsonProperty("data")]
        public List<MarketStackIntradayData>? Data { get; set; }
    }

    public class MarketStackIntradayData
    {
        [JsonProperty("open")]
        public decimal? Open { get; set; }

        [JsonProperty("high")]
        public decimal? High { get; set; }

        [JsonProperty("low")]
        public decimal? Low { get; set; }

        [JsonProperty("close")]
        public decimal? Close { get; set; }

        [JsonProperty("last")]
        public decimal? Last { get; set; }

        [JsonProperty("volume")]
        public decimal? Volume { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("symbol")]
        public string? Symbol { get; set; }

        [JsonProperty("exchange")]
        public string? Exchange { get; set; }
    }

    // Modelo para Tickers
    public class MarketStackTickersResponse
    {
        [JsonProperty("pagination")]
        public MarketStackPagination? Pagination { get; set; }

        [JsonProperty("data")]
        public List<MarketStackTickerData>? Data { get; set; }
    }

    public class MarketStackTickerData
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("symbol")]
        public string? Symbol { get; set; }

        [JsonProperty("stock_exchange")]
        public MarketStackStockExchange? StockExchange { get; set; }
    }

    public class MarketStackStockExchange
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("acronym")]
        public string? Acronym { get; set; }

        [JsonProperty("mic")]
        public string? Mic { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("country_code")]
        public string? CountryCode { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("website")]
        public string? Website { get; set; }
    }

    // Modelo para Exchanges
    public class MarketStackExchangesResponse
    {
        [JsonProperty("pagination")]
        public MarketStackPagination? Pagination { get; set; }

        [JsonProperty("data")]
        public List<MarketStackExchangeData>? Data { get; set; }
    }

    public class MarketStackExchangeData
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("acronym")]
        public string? Acronym { get; set; }

        [JsonProperty("mic")]
        public string? Mic { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("country_code")]
        public string? CountryCode { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("website")]
        public string? Website { get; set; }

        [JsonProperty("timezone")]
        public MarketStackTimezone? Timezone { get; set; }
    }

    public class MarketStackTimezone
    {
        [JsonProperty("timezone")]
        public string? TimezoneValue { get; set; }

        [JsonProperty("abbr")]
        public string? Abbr { get; set; }

        [JsonProperty("abbr_dst")]
        public string? AbbrDst { get; set; }
    }

    // Modelo para Dividends
    public class MarketStackDividendsResponse
    {
        [JsonProperty("pagination")]
        public MarketStackPagination? Pagination { get; set; }

        [JsonProperty("data")]
        public List<MarketStackDividendData>? Data { get; set; }
    }

    public class MarketStackDividendData
    {
        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("dividend")]
        public decimal? Dividend { get; set; }

        [JsonProperty("symbol")]
        public string? Symbol { get; set; }
    }

    // Modelo para Splits
    public class MarketStackSplitsResponse
    {
        [JsonProperty("pagination")]
        public MarketStackPagination? Pagination { get; set; }

        [JsonProperty("data")]
        public List<MarketStackSplitData>? Data { get; set; }
    }

    public class MarketStackSplitData
    {
        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("split_factor")]
        public decimal? SplitFactor { get; set; }

        [JsonProperty("symbol")]
        public string? Symbol { get; set; }
    }

    // Modelo de Paginação (comum a todas as respostas)
    public class MarketStackPagination
    {
        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }

    // Modelo para Erro da API
    public class MarketStackErrorResponse
    {
        [JsonProperty("error")]
        public MarketStackError? Error { get; set; }
    }

    public class MarketStackError
    {
        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("context")]
        public object? Context { get; set; }
    }
}