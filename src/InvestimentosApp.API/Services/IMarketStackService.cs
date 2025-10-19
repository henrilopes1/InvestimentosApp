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

        // Tickers
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