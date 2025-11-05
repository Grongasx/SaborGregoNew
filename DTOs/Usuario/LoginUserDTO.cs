using System.ComponentModel.DataAnnotations;

namespace SaborGregoNew.DTOs.Usuario;

public class LoginUserDTO
{
    [Required(ErrorMessage = "O e-mail é obrigatório para o login.")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "A senha deve ser informada.")]
    public string Senha { get; set; } = string.Empty;
}
