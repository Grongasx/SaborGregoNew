using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Repository;

namespace SaborGregoNew.Pages.Usuario.Admin.Funcionarios
{
    public class CreateModel : PageModel
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public CreateModel(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [BindProperty]
        public RegisterUserDto Input { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _usuarioRepository.Create(Input);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Erro ao cadastrar funcionário: " + ex.Message);
                return Page();
            }
        }
    }
}
