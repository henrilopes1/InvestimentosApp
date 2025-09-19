using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InvestimentosApp.Domain.Interfaces;
using InvestimentosApp.Domain.Models;
using InvestimentosApp.Data.Context;

namespace InvestimentosApp.Data.Repositories
{
    public class InvestimentoRepository : IInvestimentoRepository
    {
        private readonly AppDbContext _context;

        public InvestimentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Investimento>> GetAllAsync()
        {
            try
            {
                return await _context.Investimentos.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar investimentos: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        public async Task<Investimento?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Investimentos.FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar investimento por ID {id}: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        public async Task<IEnumerable<Investimento>> GetByInvestidorIdAsync(int investidorId)
        {
            try
            {
                return await _context.Investimentos
                    .Where(i => i.InvestidorId == investidorId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar investimentos do investidor {investidorId}: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        public async Task<bool> AddAsync(Investimento investimento)
        {
            try
            {
                // Verificar se o investidor existe
                var investidorExiste = await _context.Investidores.AnyAsync(i => i.Id == investimento.InvestidorId);
                if (!investidorExiste)
                {
                    Console.WriteLine($"Investidor com ID {investimento.InvestidorId} não encontrado");
                    return false;
                }

                _context.Investimentos.Add(investimento);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar investimento: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Investimento investimento)
        {
            _context.Entry(investimento).State = EntityState.Modified;
            
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
            var investimento = await _context.Investimentos.FindAsync(id);
            if (investimento == null)
                return false;

            _context.Investimentos.Remove(investimento);
            
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