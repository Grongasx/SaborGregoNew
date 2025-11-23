using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Repository;

namespace SaborGregoNew.Pages.Usuario.Admin.Funcionarios
{
    public class EditModel : PageModel
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public EditModel(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [BindProperty]
        public RegisterUserDto Input { get; set; } = new();

        public int CurrentId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _usuarioRepository.SelectByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            CurrentId = user.Id;
            Input = new RegisterUserDto
            {
                Nome = user.Nome,
                Email = user.Email,
                Telefone = user.Telefone,
                Role = user.Role,
                Senha = "PLACEHOLDER_PASSWORD"
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            // Remove password validation if it's empty or placeholder
            if (Input.Senha == "PLACEHOLDER_PASSWORD" || string.IsNullOrEmpty(Input.Senha))
            {
                ModelState.Remove("Input.Senha");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Fetch existing user to get the old password if we are not updating it
                var existingUser = await _usuarioRepository.SelectByIdAsync(id);
                if (existingUser == null) return NotFound();

                if (Input.Senha == "PLACEHOLDER_PASSWORD" || string.IsNullOrEmpty(Input.Senha))
                {
                    Input.Senha = existingUser.Senha;
                }
                // Else: Input.Senha is the new password.

                await _usuarioRepository.UpdateById(id, Input);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Erro ao atualizar funcionário: " + ex.Message);
                return Page();
            }
        }
    }
}
