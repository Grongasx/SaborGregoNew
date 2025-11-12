using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using saborGregoNew.Repository;
using SaborGregoNew.Extensions;
using SaborGregoNew.Models;
using System.Security.Claims;

namespace SaborGregoNew.Pages.Usuario
{
    public class ListaEnderecosModel : PageModel
    {
        private readonly IEnderecoRepository _enderecoRepository;

        public ListaEnderecosModel(IEnderecoRepository enderecoRepository) 
        {
            _enderecoRepository = enderecoRepository;
        }

        public List<Endereco> enderecos { get; set; } = new List<Endereco>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = 0;
            try
            {
                userId = User.GetUserId();
            }
            catch
            {
                TempData["MensagemErro"] = "Usuário não autenticado, Porfavor faça Login para continuar!";
                RedirectToPage("/Usuario/Login/Login");
            }
            enderecos = await _enderecoRepository.SelectAllByUserIdAsync(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int Id)
        {

            try
            {
                await _enderecoRepository.DesativarAsync(Id);
                TempData["MensagemSucesso"] = "Endereço desativado com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao desativar endereço: " + ex.Message;
                return Page();
            }
            return RedirectToPage();
        }
    }
}
