using System.ComponentModel.DataAnnotations;
using SaborGregoNew.Enums;

namespace SaborGregoNew.Models
{
    public class Usuario
    {
        public Usuario()
        {

        }

        public Usuario(int id, string nome, string telefone, string email, string senha, UserRole role, Endereco? endereco)
        {
            Id = id;
            Nome = nome;
            Telefone = telefone;
            Email = email;
            Senha = senha;
            Role = role;
            Endereco = new List<Endereco>();
        }

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
    }
}