using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Enums;
using SaborGregoNew.Repository;

namespace SaborGregoNew.Pages.Usuario.Admin.Funcionarios
{
    public class IndexModel : PageModel
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public IndexModel(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public List<Models.Usuario> Funcionarios { get; set; } = new();

        [BindProperty]
        public SaborGregoNew.DTOs.Usuario.RegisterUserDto Input { get; set; } = new();

        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Roles { get; set; }

        public async Task OnGetAsync()
        {
            Roles = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "Cozinheiro", Value = ((int)UserRole.Cozinheiro).ToString() },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "Entregador", Value = ((int)UserRole.Entregador).ToString() }
            };

            try{
                var allUsers = await _usuarioRepository.SelectAllAsync();
                if(allUsers != null){
                    Funcionarios = allUsers
                    .Where(u => u.Role == UserRole.Cozinheiro || u.Role == UserRole.Entregador || u.Role == UserRole.Admin)
                    .OrderBy(u => u.Nome)
                    .ToList();
                }
            }
            catch(Exception){
                ModelState.AddModelError(string.Empty, "Erro ao carregar a lista de funcionários.");
            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }
            try
            {
                await _usuarioRepository.Create(Input);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Erro ao cadastrar funcionário: " + ex.Message);
                await OnGetAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _usuarioRepository.DeleteById(id);
            return RedirectToPage();
        }
    }
}
