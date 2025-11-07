using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Models;
using SaborGregoNew.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SaborGregoNew.Pages.Usuario
{
    public class ExcluirEnderecoModel : PageModel
    {
        private readonly EnderecoService _enderecoService;

        public ExcluirEnderecoModel(EnderecoService enderecoService)
        {
            _enderecoService = enderecoService;
        }

        [BindProperty]
        public Endereco Endereco { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return RedirectToPage("/Usuario/Login");
            }

            Endereco = await _enderecoService.GetById(id);

            if (Endereco == null || Endereco.UsuarioId != userId)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return RedirectToPage("/Usuario/Login");
            }

            var enderecoFromDb = await _enderecoService.GetById(id);
            if (enderecoFromDb == null || enderecoFromDb.UsuarioId != userId)
            {
                return Forbid();
            }

            await _enderecoService.DeleteAsync(id);

            return RedirectToPage("/Usuario/ListaEnderecos");
        }
    }
}
