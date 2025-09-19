using InvestimentosApp.Domain.Models;

namespace InvestimentosApp.Domain.Interfaces
{
    public interface IInvestimentoRepository
    {
        Task<IEnumerable<Investimento>> GetAllAsync();
        Task<Investimento?> GetByIdAsync(int id);
        Task<bool> AddAsync(Investimento investimento);
        Task<bool> UpdateAsync(Investimento investimento);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Investimento>> GetByInvestidorIdAsync(int investidorId);
    }
}