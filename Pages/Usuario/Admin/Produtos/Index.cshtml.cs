using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Repository;
using SaborGregoNew.Models;

namespace SaborGregoNew.Pages.Usuario.Admin.Produtos
{
    public class IndexModel : PageModel
    {
        private readonly IProdutoRepository _produtoRepository;

        public List<SaborGregoNew.Models.Produto> Produtos { get; set; } = new();

        public IndexModel(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task OnGetAsync()
        {
            Produtos = await _produtoRepository.SelectAllAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _produtoRepository.DesativarAsync(id);
                TempData["MensagemSucesso"] = "Produto desativado com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao desativar produto: " + ex.Message;
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAtivarAsync(int id)
        {
            try
            {
                await _produtoRepository.AtivarAsync(id);
                TempData["MensagemSucesso"] = "Produto ativado com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao ativar produto: " + ex.Message;
            }
            return RedirectToPage();
        }
    }
}
