using InvestimentosApp.Domain.Models;

namespace InvestimentosApp.Domain.Interfaces
{
    public interface IInvestidorRepository
    {
        Task<IEnumerable<Investidor>> GetAllAsync();
        Task<Investidor?> GetByIdAsync(int id);
        Task<Investidor?> GetByEmailAsync(string email);
        Task<bool> AddAsync(Investidor investidor);
        Task<bool> UpdateAsync(Investidor investidor);
        Task<bool> DeleteAsync(int id);
    }
}