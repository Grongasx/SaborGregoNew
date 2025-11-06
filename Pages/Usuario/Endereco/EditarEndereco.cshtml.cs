using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Models;
using SaborGregoNew.Services;
using System.Security.Claims;

namespace SaborGregoNew.Pages.Usuario
{
    public class EditarEnderecoModel : PageModel
    {
        private readonly EnderecoService _enderecoService;

        public EditarEnderecoModel(EnderecoService enderecoService)
        {
            _enderecoService = enderecoService;
        }

        [BindProperty]
        public Endereco Endereco { get; set; }

        public IActionResult OnGet(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return RedirectToPage("/Usuario/Login");
            }

            Endereco = _enderecoService.GetById(id);

            if (Endereco == null || Endereco.UsuarioId != userId)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return RedirectToPage("/Usuario/Login");
            }

            var enderecoFromDb = _enderecoService.GetById(Endereco.Id);
            if (enderecoFromDb == null || enderecoFromDb.UsuarioId != userId)
            {
                return Forbid();
            }

            // Update the properties of the entity that is already being tracked.
            enderecoFromDb.Apelido = Endereco.Apelido;
            enderecoFromDb.Logradouro = Endereco.Logradouro;
            enderecoFromDb.Numero = Endereco.Numero;
            enderecoFromDb.Complemento = Endereco.Complemento;
            enderecoFromDb.Bairro = Endereco.Bairro;

            await _enderecoService.UpdateAsync(enderecoFromDb);

            return RedirectToPage("/Usuario/ListaEnderecos");
        }
    }
}
