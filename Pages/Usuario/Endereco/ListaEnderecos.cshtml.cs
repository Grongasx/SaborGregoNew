using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Models;
using SaborGregoNew.Services;
using System.Security.Claims;

namespace SaborGregoNew.Pages.Usuario
{
    public class ListaEnderecosModel : PageModel
    {
        private readonly SaborGregoNew.Repository.IEnderecoRepository _enderecoRepository;

        public ListaEnderecosModel(SaborGregoNew.Repository.IEnderecoRepository enderecoRepository)
        {
            _enderecoRepository = enderecoRepository;
        }

        public List<SaborGregoNew.Models.Endereco> Enderecos { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return RedirectToPage("/Usuario/Login");
            }

            List<SaborGregoNew.Models.Endereco> Enderecos = await _enderecoRepository.SelectAllByUserIdAsync(userId);

            return Page();
        }
    }
}
