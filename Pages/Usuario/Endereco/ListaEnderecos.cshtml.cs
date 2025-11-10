using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using saborGregoNew.Repository;
using SaborGregoNew.Models;
using System.Security.Claims;

namespace SaborGregoNew.Pages.Usuario
{
    public class ListaEnderecosModel : PageModel
    {
        private readonly IEnderecoRepository _enderecoService;

        public ListaEnderecosModel(IEnderecoRepository enderecoService) 
        {
            _enderecoService = enderecoService;
        }

        public List<Endereco> enderecos { get; set; } = new List<Endereco>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
            {
                TempData["MensagemErro"] = "Para acessar esta página, você precisa estar logado.";
                return RedirectToPage("/Usuario/Login/Login");
            }

            var userId = int.Parse(userIdString);

            enderecos = await _enderecoService.SelectAllByUserIdAsync(userId);
            if (enderecos.Count == 0)
            {
                TempData["MensagemErro"] = "Nenhum endereço encontrado.";
                return Page();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int Id)
        {
            await _enderecoService.DeleteById(Id);

            return RedirectToPage("/Usuario/Endereco/ListaEnderecos");
        }
    }
}
