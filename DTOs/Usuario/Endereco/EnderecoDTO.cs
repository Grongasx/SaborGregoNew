using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaborGregoNew.DTOs.Usuario;

public class EnderecoDTO
{
    [Key]
    public int Id { get; set; }
    [StringLength(50, ErrorMessage = "O apelido deve ter no máximo 50 caracteres")]
    public string Apelido { get; set; } = string.Empty;
    [Required(ErrorMessage = "O logradouro é obrigatório")]
    [StringLength(100, ErrorMessage = "O logradouro deve ter no máximo 100 caracteres")]
    public string Logradouro { get; set; } = string.Empty;
    [Required(ErrorMessage = "O número é obrigatório")]
    public string Numero { get; set; } = string.Empty;
    [StringLength(50, ErrorMessage = "O complemento deve ter no máximo 50 caracteres")]
    public string Complemento { get; set; } = string.Empty;
    [Required(ErrorMessage = "O bairro é obrigatório")]
    [StringLength(50, ErrorMessage = "O bairro deve ter no máximo 50 caracteres")]
    public string Bairro { get; set; } = string.Empty;
    [ForeignKey("UsuarioId")]
    public int? UsuarioId { get; set; }
}