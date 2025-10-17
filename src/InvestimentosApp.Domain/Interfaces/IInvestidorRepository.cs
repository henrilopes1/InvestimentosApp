using InvestimentosApp.Domain.Models;

namespace InvestimentosApp.Domain.Interfaces
{
    public interface IInvestidorRepository
    {
        // CRUD básico
        Task<IEnumerable<Investidor>> GetAllAsync();
        Task<Investidor?> GetByIdAsync(int id);
        Task<Investidor?> GetByEmailAsync(string email);
        Task<bool> AddAsync(Investidor investidor);
        Task<bool> UpdateAsync(Investidor investidor);
        Task<bool> DeleteAsync(int id);
        
        // Pesquisas avançadas com LINQ
        Task<IEnumerable<Investidor>> SearchByNameAsync(string nome);
        Task<IEnumerable<Investidor>> GetByPerfilRiscoAsync(string perfilRisco);
        Task<IEnumerable<Investidor>> GetBySaldoRangeAsync(decimal saldoMinimo, decimal saldoMaximo);
        Task<IEnumerable<Investidor>> GetByIdadeRangeAsync(int idadeMinima, int idadeMaxima);
        Task<IEnumerable<Investidor>> SearchMultipleFiltersAsync(string? nome, string? perfilRisco, decimal? saldoMinimo, decimal? saldoMaximo);
        Task<int> CountByPerfilRiscoAsync(string perfilRisco);
        Task<decimal> GetTotalSaldoAsync();
        Task<decimal> GetMediaSaldoByPerfilAsync(string perfilRisco);
    }
}