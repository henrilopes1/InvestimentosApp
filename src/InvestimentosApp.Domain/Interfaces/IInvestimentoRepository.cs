using InvestimentosApp.Domain.Models;

namespace InvestimentosApp.Domain.Interfaces
{
    public interface IInvestimentoRepository
    {
        // CRUD básico
        Task<IEnumerable<Investimento>> GetAllAsync();
        Task<Investimento?> GetByIdAsync(int id);
        Task<bool> AddAsync(Investimento investimento);
        Task<bool> UpdateAsync(Investimento investimento);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Investimento>> GetByInvestidorIdAsync(int investidorId);
        
        // Pesquisas avançadas com LINQ
        Task<IEnumerable<Investimento>> GetByTipoAsync(string tipo);
        Task<IEnumerable<Investimento>> GetByStatusAsync(string status);
        Task<IEnumerable<Investimento>> GetByRentabilidadeRangeAsync(decimal rentabilidadeMinima, decimal rentabilidadeMaxima);
        Task<IEnumerable<Investimento>> GetByValorRangeAsync(decimal valorMinimo, decimal valorMaximo);
        Task<IEnumerable<Investimento>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Investimento>> SearchMultipleFiltersAsync(string? nome, string? tipo, string? status, decimal? rentabilidadeMinima);
        Task<IEnumerable<Investimento>> GetTopRentaveisAsync(int quantidade);
        Task<decimal> GetTotalInvestidoAsync();
        Task<decimal> GetTotalAtualAsync();
        Task<decimal> GetMediaRentabilidadeByTipoAsync(string tipo);
        Task<int> CountByTipoAsync(string tipo);
    }
}