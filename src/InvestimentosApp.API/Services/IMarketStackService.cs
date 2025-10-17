using InvestimentosApp.API.Models.ExternalAPIs;

namespace InvestimentosApp.API.Services
{
    public interface IMarketStackService
    {
        // End-of-Day Data
        Task<MarketStackEodResponse?> GetEndOfDayDataAsync(string symbols, string? exchange = null, 
            DateTime? dateFrom = null, DateTime? dateTo = null, int limit = 100, int offset = 0);
        
        Task<MarketStackEodResponse?> GetLatestEndOfDayDataAsync(string symbols, string? exchange = null);
        
        Task<MarketStackEodResponse?> GetEndOfDayDataByDateAsync(string symbols, DateTime date, string? exchange = null);

        // Intraday Data
        Task<MarketStackIntradayResponse?> GetIntradayDataAsync(string symbols, string? exchange = null, 
            string interval = "1hour", DateTime? dateFrom = null, DateTime? dateTo = null, int limit = 100, int offset = 0);
        
        Task<MarketStackIntradayResponse?> GetLatestIntradayDataAsync(string symbols, string? exchange = null, string interval = "1hour");

        // Tickers
        Task<MarketStackTickersResponse?> GetTickersAsync(string? exchange = null, string? search = null, 
            int limit = 100, int offset = 0);
        
        Task<MarketStackTickerData?> GetTickerInfoAsync(string symbol);

        // Exchanges
        Task<MarketStackExchangesResponse?> GetExchangesAsync(string? search = null, int limit = 100, int offset = 0);
        
        Task<MarketStackExchangeData?> GetExchangeInfoAsync(string mic);

        // Dividends
        Task<MarketStackDividendsResponse?> GetDividendsAsync(string symbols, DateTime? dateFrom = null, 
            DateTime? dateTo = null, int limit = 100, int offset = 0);

        // Splits
        Task<MarketStackSplitsResponse?> GetSplitsAsync(string symbols, DateTime? dateFrom = null, 
            DateTime? dateTo = null, int limit = 100, int offset = 0);
    }
}