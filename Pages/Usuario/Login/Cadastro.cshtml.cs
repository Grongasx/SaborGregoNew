using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using saborGregoNew.Repository;
using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Enums;

namespace SaborGregoNew.Pages.Usuario;

public class CadastroModel : PageModel
{
    private readonly IUsuarioRepository _usuarioService;
    
    public CadastroModel(IUsuarioRepository usuarioService)
    {
        _usuarioService =  usuarioService;
    }

    [BindProperty]
    public RegisterDto? Usuario  { get; set; }
    
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
        }
            
        return RedirectToPage("/Index");
    }
}