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

        // CRUD Básico
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

        // =================== PESQUISAS AVANÇADAS COM LINQ ===================

        // Busca por nome (contém texto)
        public async Task<IEnumerable<Investidor>> SearchByNameAsync(string nome)
        {
            return await _context.Investidores
                .Where(i => i.Nome.ToLower().Contains(nome.ToLower()))
                .OrderBy(i => i.Nome)
                .ToListAsync();
        }

        // Busca por perfil de risco
        public async Task<IEnumerable<Investidor>> GetByPerfilRiscoAsync(string perfilRisco)
        {
            return await _context.Investidores
                .Where(i => i.PerfilRisco.ToLower() == perfilRisco.ToLower())
                .OrderBy(i => i.SaldoTotal)
                .ToListAsync();
        }

        // Busca por faixa de saldo
        public async Task<IEnumerable<Investidor>> GetBySaldoRangeAsync(decimal saldoMinimo, decimal saldoMaximo)
        {
            return await _context.Investidores
                .Where(i => i.SaldoTotal >= saldoMinimo && i.SaldoTotal <= saldoMaximo)
                .OrderByDescending(i => i.SaldoTotal)
                .ToListAsync();
        }

        // Busca por faixa de idade
        public async Task<IEnumerable<Investidor>> GetByIdadeRangeAsync(int idadeMinima, int idadeMaxima)
        {
            var dataLimiteMax = DateTime.Now.AddYears(-idadeMinima);
            var dataLimiteMin = DateTime.Now.AddYears(-idadeMaxima);
            
            return await _context.Investidores
                .Where(i => i.DataNascimento <= dataLimiteMax && i.DataNascimento >= dataLimiteMin)
                .OrderBy(i => i.DataNascimento)
                .ToListAsync();
        }

        // Busca com múltiplos filtros
        public async Task<IEnumerable<Investidor>> SearchMultipleFiltersAsync(
            string? nome, 
            string? perfilRisco, 
            decimal? saldoMinimo, 
            decimal? saldoMaximo)
        {
            var query = _context.Investidores.AsQueryable();

            // Aplica filtros condicionalmente
            if (!string.IsNullOrEmpty(nome))
                query = query.Where(i => i.Nome.ToLower().Contains(nome.ToLower()));

            if (!string.IsNullOrEmpty(perfilRisco))
                query = query.Where(i => i.PerfilRisco.ToLower() == perfilRisco.ToLower());

            if (saldoMinimo.HasValue)
                query = query.Where(i => i.SaldoTotal >= saldoMinimo.Value);

            if (saldoMaximo.HasValue)
                query = query.Where(i => i.SaldoTotal <= saldoMaximo.Value);

            return await query.OrderBy(i => i.Nome).ToListAsync();
        }

        // Contadores e agregações
        public async Task<int> CountByPerfilRiscoAsync(string perfilRisco)
        {
            return await _context.Investidores
                .CountAsync(i => i.PerfilRisco.ToLower() == perfilRisco.ToLower());
        }

        public async Task<decimal> GetTotalSaldoAsync()
        {
            return await _context.Investidores.SumAsync(i => i.SaldoTotal);
        }

        public async Task<decimal> GetMediaSaldoByPerfilAsync(string perfilRisco)
        {
            var investidores = await _context.Investidores
                .Where(i => i.PerfilRisco.ToLower() == perfilRisco.ToLower())
                .ToListAsync();

            return investidores.Any() ? investidores.Average(i => i.SaldoTotal) : 0;
        }
    }
}