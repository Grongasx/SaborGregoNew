using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Services;

namespace SaborGregoNew.Pages.Usuario
{
    public class LoginModel : PageModel
    {
        private readonly UsuarioService _usuarioService;

        [BindProperty]
        public LoginUserDTO? LoginDto { get; set; } // Renomeado para seguir o padrão da View

        public LoginModel(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public void OnGet() { }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || LoginDto == null)
            {
                return Page();
            }

            // 1. Chama o método de login
            var usuarioLogado = await _usuarioService.Logar(LoginDto);

            if (usuarioLogado != null)
            {
                // 2. Criação das Claims (Identidades do usuário)
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
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Expiração
                };

                // 5. Efetua o Login (Assina o usuário e cria o Cookie de Autenticação)
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                
                // 6. Redireciona para uma página protegida após o sucesso
                return RedirectToPage("/Index"); 
            }
            else
            {
                // 7. Falha: Credenciais Inválidas
                ModelState.AddModelError(string.Empty, "Credenciais inválidas. Verifique seu e-mail e sua senha.");
                return Page();
            }
        }
    }
}