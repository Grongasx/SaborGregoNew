using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Models;
using SaborGregoNew.Services;
using System.Security.Claims;

namespace SaborGregoNew.Pages.Usuario
{
    public class ListaEnderecosModel : PageModel
    {
        private readonly EnderecoService _enderecoService;

        public ListaEnderecosModel(EnderecoService enderecoService)
        {
            _enderecoService = enderecoService;
        }

        public List<Endereco> Enderecos { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return RedirectToPage("/Usuario/Login");
            }

            Enderecos = await _enderecoService.GetAllByUserId(userId);

            return Page();
        }
    }
}
