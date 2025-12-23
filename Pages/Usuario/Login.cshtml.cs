using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Repository;
using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Enums;

namespace SaborGregoNew.Pages.Usuario
{
    public class LoginModel : PageModel
    {
        private readonly IUsuarioRepository _usuarioService;
        [BindProperty]
        public LoginDTO? LoginDto { get; set; }
        public LoginModel(IUsuarioRepository usuarioService)
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

            // 1. Chama o método de login (agora retorna null se falhar, sem travar)
            var usuarioLogado = await _usuarioService.Login(LoginDto);

            if (usuarioLogado != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuarioLogado.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuarioLogado.Nome),
                    new Claim(ClaimTypes.Role, usuarioLogado.Role.ToString()) 
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false,
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                
                if (usuarioLogado.Role == UserRole.Cozinheiro) return RedirectToPage("/Usuario/Cozinheiro/Pedidos");
                if (usuarioLogado.Role == UserRole.Entregador) return RedirectToPage("/Usuario/Entregador/Entregas");
                if (usuarioLogado.Role == UserRole.Admin) return RedirectToPage("/Usuario/Admin/Dashboard");
                
                return RedirectToPage("/Index"); 
            }
            else
            {
                ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos. Tente novamente.");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }
    }
}