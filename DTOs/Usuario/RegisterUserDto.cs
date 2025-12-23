using System.ComponentModel.DataAnnotations;
using SaborGregoNew.Enums;

namespace SaborGregoNew.DTOs.Usuario;

public class RegisterUserDto
{
    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Tamanho do nome inválido")]
    public string Nome { get; set; } = string.Empty;
    [Required]
    public string Telefone { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Senha { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}