using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Models;
using SaborGregoNew.Repository;

namespace SaborGregoNew.Pages.Usuario.Endereco
{
    public class IndexModel : PageModel
    {
        private readonly IEnderecoRepository _enderecoRepository;

        public IndexModel(IEnderecoRepository enderecoRepository)
        {
            _enderecoRepository = enderecoRepository;
        }

        public List<SaborGregoNew.Models.Endereco> Enderecos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToPage("/Usuario/Login");
            }

            if (int.TryParse(userIdString, out int userId))
            {
                Enderecos = await _enderecoRepository.SelectAllByUserIdAsync(userId);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _enderecoRepository.DesativarAsync(id);
                TempData["MensagemSucesso"] = "Endereço excluído com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao excluir endereço: " + ex.Message;
            }

            return RedirectToPage();
        }
    }
}
