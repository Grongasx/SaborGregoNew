using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using saborGregoNew.Repository;
using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace SaborGregoNew.Pages.Usuario;

public class CadastroModel : PageModel
{
    private readonly IUsuarioRepository _usuarioService;
    
    public CadastroModel(IUsuarioRepository usuarioService)
    {
        _usuarioService =  usuarioService;
    }

    [BindProperty]
    public RegisterDto? Usuario { get; set; }
    public LoginDTO? LoginDto { get; set; }
    
    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Usuario != null)
        {
            Usuario.Role = UserRole.Cliente;

            await _usuarioService.Create(Usuario);
            var login = new LoginDTO
            {
                Email = Usuario.Email,
                Senha = Usuario.Senha
            };
            await _usuarioService.Login(login);
            await claimar(await _usuarioService.Login(login));
            TempData["SucessMessage"] = "Cadastro efetuado com sucesso!";
        }

        return RedirectToPage("/Index");
    }

    private async Task claimar(Models.Usuario usuarioLogado)
    {
        var claims = new List<Claim>
                {
                    // A ClaimTypes.NameIdentifier armazena o ID único do usuário
                    new Claim(ClaimTypes.NameIdentifier, usuarioLogado.Id.ToString()),
                    
                    // A ClaimTypes.Name armazena o nome (útil para exibição)
                    new Claim(ClaimTypes.Name, usuarioLogado.Nome),
                    
                    // A ClaimTypes.Role armazena a função/papel (IMPORTANTE para autorização)
                    new Claim(ClaimTypes.Role, usuarioLogado.Role.ToString()) 
                    
                    // Você pode adicionar mais Claims aqui, como o Email, Telefone, etc.
                };

                // 3. Criação da identidade principal
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // 4. Criação das propriedades de autenticação (opcional, mas útil)
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Manter o usuário logado entre sessões (Remember Me)
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                // 5. Efetua o Login (Assina o usuário e cria o Cookie de Autenticação)
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
    }
}