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

        // =================== PESQUISAS AVANÇADAS COM LINQ ===================

        // Busca por tipo de investimento
        public async Task<IEnumerable<Investimento>> GetByTipoAsync(string tipo)
        {
            return await _context.Investimentos
                .Where(i => i.Tipo.ToLower() == tipo.ToLower())
                .OrderByDescending(i => i.Rentabilidade)
                .ToListAsync();
        }

        // Busca por status
        public async Task<IEnumerable<Investimento>> GetByStatusAsync(string status)
        {
            return await _context.Investimentos
                .Where(i => i.Status.ToLower() == status.ToLower())
                .OrderBy(i => i.DataInicio)
                .ToListAsync();
        }

        // Busca por faixa de rentabilidade
        public async Task<IEnumerable<Investimento>> GetByRentabilidadeRangeAsync(decimal rentabilidadeMinima, decimal rentabilidadeMaxima)
        {
            return await _context.Investimentos
                .Where(i => i.Rentabilidade >= rentabilidadeMinima && i.Rentabilidade <= rentabilidadeMaxima)
                .OrderByDescending(i => i.Rentabilidade)
                .ToListAsync();
        }

        // Busca por faixa de valor
        public async Task<IEnumerable<Investimento>> GetByValorRangeAsync(decimal valorMinimo, decimal valorMaximo)
        {
            return await _context.Investimentos
                .Where(i => i.ValorAtual >= valorMinimo && i.ValorAtual <= valorMaximo)
                .OrderByDescending(i => i.ValorAtual)
                .ToListAsync();
        }

        // Busca por período de investimento
        public async Task<IEnumerable<Investimento>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _context.Investimentos
                .Where(i => i.DataInicio >= dataInicio && i.DataInicio <= dataFim)
                .OrderBy(i => i.DataInicio)
                .ToListAsync();
        }

        // Busca com múltiplos filtros
        public async Task<IEnumerable<Investimento>> SearchMultipleFiltersAsync(
            string? nome, 
            string? tipo, 
            string? status, 
            decimal? rentabilidadeMinima)
        {
            var query = _context.Investimentos.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
                query = query.Where(i => i.Nome.ToLower().Contains(nome.ToLower()));

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(i => i.Tipo.ToLower() == tipo.ToLower());

            if (!string.IsNullOrEmpty(status))
                query = query.Where(i => i.Status.ToLower() == status.ToLower());

            if (rentabilidadeMinima.HasValue)
                query = query.Where(i => i.Rentabilidade >= rentabilidadeMinima.Value);

            return await query.OrderByDescending(i => i.Rentabilidade).ToListAsync();
        }

        // Top investimentos mais rentáveis
        public async Task<IEnumerable<Investimento>> GetTopRentaveisAsync(int quantidade)
        {
            return await _context.Investimentos
                .OrderByDescending(i => i.Rentabilidade)
                .Take(quantidade)
                .ToListAsync();
        }

        // Agregações e estatísticas
        public async Task<decimal> GetTotalInvestidoAsync()
        {
            return await _context.Investimentos.SumAsync(i => i.ValorInicial);
        }

        public async Task<decimal> GetTotalAtualAsync()
        {
            return await _context.Investimentos.SumAsync(i => i.ValorAtual);
        }

        public async Task<decimal> GetMediaRentabilidadeByTipoAsync(string tipo)
        {
            var investimentos = await _context.Investimentos
                .Where(i => i.Tipo.ToLower() == tipo.ToLower())
                .ToListAsync();

            return investimentos.Any() ? investimentos.Average(i => i.Rentabilidade) : 0;
        }

        public async Task<int> CountByTipoAsync(string tipo)
        {
            return await _context.Investimentos
                .CountAsync(i => i.Tipo.ToLower() == tipo.ToLower());
        }
    }
}