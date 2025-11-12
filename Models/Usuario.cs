using System.ComponentModel.DataAnnotations;
using SaborGregoNew.Enums;

namespace SaborGregoNew.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; } = string.Empty;
        [Required]
        public string Telefone { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Senha { get; set; } = string.Empty;
        [Required]
        public UserRole Role { get; set; }
        public ICollection<Endereco>? Endereco { get; set; }
        public ICollection<Pedido>? Pedidos { get; set; }
        public bool Ativo { get; set; } = true;
    }
}