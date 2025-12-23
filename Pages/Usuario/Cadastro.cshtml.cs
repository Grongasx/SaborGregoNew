using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Enums;
using SaborGregoNew.Services;
using SaborGregoNew.Repository;

namespace SaborGregoNew.Pages.Usuario;

public class CadastroModel : PageModel
{
    private readonly IUsuarioRepository _usuarioRepository;
    
    public CadastroModel(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
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
            
            await _usuarioRepository.Create(Usuario);
        }
            
        return RedirectToPage("/Index");
    }
}