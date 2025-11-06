using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Models;
using SaborGregoNew.Services;

namespace SaborGregoNew.Pages.Usuario
{
    public class CadastroEnderecoModel : PageModel
    {
        private readonly EnderecoService _EnderecoService;
        public CadastroEnderecoModel(EnderecoService enderecoService)
        {
            _EnderecoService = enderecoService;
        }

        [BindProperty]
        public CadastroEnderecoDTO? enderecoDTO { get; set; }

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
            var UserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(UserIdString))
            {
                Console.WriteLine("O Usuario não está logado");
                return Unauthorized();
            }

            var UserId = int.Parse(UserIdString);

            if (enderecoDTO != null)
            {
                enderecoDTO.UsuarioId = UserId;
                await _EnderecoService.CadastrarEndereco(enderecoDTO);
            }

            return RedirectToPage("/Index");
        }
    }
}
        