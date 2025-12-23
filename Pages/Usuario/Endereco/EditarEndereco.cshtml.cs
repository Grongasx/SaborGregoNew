using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Models;
using SaborGregoNew.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SaborGregoNew.Pages.Usuario
{
    public class EditarEnderecoModel : PageModel
    {
        private readonly SaborGregoNew.Repository.IEnderecoRepository _enderecoRepository;

        public EditarEnderecoModel(SaborGregoNew.Repository.IEnderecoRepository enderecoRepository)
        {
            _enderecoRepository = enderecoRepository;
        }

        [BindProperty]
        public SaborGregoNew.Models.Endereco Endereco { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return RedirectToPage("/Usuario/Login");
            }

            Endereco = await _enderecoRepository.SelectByIdAsync(id);

            // Garante que o endereço existe e pertence ao usuário logado
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

            // Verificação de segurança duplicada para garantir que ninguém alterou o ID no HTML
            var enderecoFromDb = await _enderecoRepository.SelectByIdAsync(Endereco.Id);
            if (enderecoFromDb == null || enderecoFromDb.UsuarioId != userId)
            {
                return Forbid();
            }

            var dto = new DTOs.Usuario.CadastroEnderecoDTO
            {
                Apelido = Endereco.Apelido,
                Logradouro = Endereco.Logradouro,
                Numero = Endereco.Numero,
                Bairro = Endereco.Bairro,
                Complemento = Endereco.Complemento,
                UsuarioId = userId
            };

            await _enderecoRepository.UpdateById(Endereco.Id, dto);

            return RedirectToPage("/Usuario/Endereco/ListaEnderecos");
        }
    }
}