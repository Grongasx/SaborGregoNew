using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaborGregoNew.DTOs.Usuario;

public class CadastroEnderecoDTO
{
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
    [ForeignKey("UsuarioId")]
    public int? UsuarioId { get; set; }
}