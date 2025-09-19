using System.ComponentModel.DataAnnotations;

namespace InvestimentosApp.Domain.Models
{
    public class Investidor
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(14, ErrorMessage = "CPF deve ter no máximo 14 caracteres")]
        public string CPF { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Saldo total deve ser maior ou igual a zero")]
        public decimal SaldoTotal { get; set; }
        
        [Required(ErrorMessage = "Perfil de risco é obrigatório")]
        [StringLength(20, ErrorMessage = "Perfil de risco deve ter no máximo 20 caracteres")]
        public string PerfilRisco { get; set; } = string.Empty;
    }
}