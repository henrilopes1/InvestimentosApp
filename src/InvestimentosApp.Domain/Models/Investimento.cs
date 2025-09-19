using System.ComponentModel.DataAnnotations;

namespace InvestimentosApp.Domain.Models
{
    public class Investimento
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Tipo é obrigatório")]
        [StringLength(50, ErrorMessage = "Tipo deve ter no máximo 50 caracteres")]
        public string Tipo { get; set; } = string.Empty;
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor inicial deve ser maior que zero")]
        public decimal ValorInicial { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Valor atual deve ser maior ou igual a zero")]
        public decimal ValorAtual { get; set; }
        
        public decimal Rentabilidade { get; set; }
        
        [Required(ErrorMessage = "Data de início é obrigatória")]
        public DateTime DataInicio { get; set; }
        
        public DateTime? DataVencimento { get; set; }
        
        [Required(ErrorMessage = "Investidor é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do investidor deve ser válido")]
        public int InvestidorId { get; set; }
        
        [Required(ErrorMessage = "Status é obrigatório")]
        [StringLength(20, ErrorMessage = "Status deve ter no máximo 20 caracteres")]
        public string Status { get; set; } = string.Empty;
    }
}