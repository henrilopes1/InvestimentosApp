using InvestimentosApp.Domain.Interfaces;
using InvestimentosApp.Domain.Models;
using InvestimentosApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvestimentosApp.Data.Repositories
{
    public class InvestidorRepository : IInvestidorRepository
    {
        private readonly AppDbContext _context;

        public InvestidorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Investidor>> GetAllAsync()
        {
            return await _context.Investidores.ToListAsync();
        }

        public async Task<Investidor?> GetByIdAsync(int id)
        {
            return await _context.Investidores.FindAsync(id);
        }

        public async Task<Investidor?> GetByEmailAsync(string email)
        {
            return await _context.Investidores.FirstOrDefaultAsync(i => i.Email == email);
        }

        public async Task<bool> AddAsync(Investidor investidor)
        {
            try
            {
                _context.Investidores.Add(investidor);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log do erro para debug
                Console.WriteLine($"Erro ao adicionar investidor: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Investidor investidor)
        {
            _context.Entry(investidor).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var investidor = await _context.Investidores.FindAsync(id);
            if (investidor == null)
                return false;

            _context.Investidores.Remove(investidor);
            
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}