using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Enums;
using SaborGregoNew.Services;

namespace SaborGregoNew.Pages.Usuario;

public class CadastroModel : PageModel
{
    private readonly UsuarioService _usuarioService;
    
    public CadastroModel(UsuarioService usuarioService)
    {
        _usuarioService =  usuarioService;
    }

    [BindProperty]
    public RegisterUserDto? Usuario  { get; set; }
    
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
            
            await _usuarioService.Register(Usuario);
        }
            
        return RedirectToPage("/Index");
    }
}