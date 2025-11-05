using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaborGregoNew.Models
{
    public class Endereco
    {
        public Endereco()
        {
            
        }

        [Key]
        public int Id { get; set; }
        public string Apelido { get; set; } = string.Empty;
        [Required]
        public string Logradouro { get; set; } = string.Empty;
        [Required]
        public string Numero { get; set; } = string.Empty;
        [Required]
        public string Complemento { get; set; } = string.Empty;
        [Required]
        public string Bairro { get; set; } = string.Empty;
        [Required]
        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
        public int? UsuarioId { get; set; }
    }
}