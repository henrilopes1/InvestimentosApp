using InvestimentosApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosApp.Data.Context;

public class AppDbContext : DbContext
{
    // NOVO CONSTRUTOR: Aceita as opções de conexão injetadas pelo Program.cs
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSets para as entidades
    public DbSet<Investidor> Investidores => Set<Investidor>();
    public DbSet<Investimento> Investimentos => Set<Investimento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração do modelo Investidor
        modelBuilder.Entity<Investidor>(entity =>
        {
            entity.ToTable("INVESTIDORES");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nome).HasColumnName("NOME").IsRequired().HasMaxLength(100);
            entity.Property(e => e.CPF).HasColumnName("CPF").IsRequired().HasMaxLength(14);
            entity.Property(e => e.Email).HasColumnName("EMAIL").IsRequired().HasMaxLength(100);
            entity.Property(e => e.DataNascimento).HasColumnName("DATANASCIMENTO");
            entity.Property(e => e.SaldoTotal).HasColumnName("SALDOTOTAL").HasPrecision(18, 2);
            entity.Property(e => e.PerfilRisco).HasColumnName("PERFILRISCO").HasMaxLength(50);
        });

        // Configuração do modelo Investimento
        modelBuilder.Entity<Investimento>(entity =>
        {
            entity.ToTable("INVESTIMENTOS");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nome).HasColumnName("NOME").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Tipo).HasColumnName("TIPO").IsRequired().HasMaxLength(50);
            entity.Property(e => e.ValorInicial).HasColumnName("VALORINICIAL").HasPrecision(18, 2);
            entity.Property(e => e.ValorAtual).HasColumnName("VALORATUAL").HasPrecision(18, 2);
            entity.Property(e => e.Rentabilidade).HasColumnName("RENTABILIDADE").HasPrecision(10, 2);
            entity.Property(e => e.DataInicio).HasColumnName("DATAINICIO");
            entity.Property(e => e.DataVencimento).HasColumnName("DATAVENCIMENTO");
            entity.Property(e => e.InvestidorId).HasColumnName("INVESTIDORID");
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20);
        });

        base.OnModelCreating(modelBuilder);
    }
}