using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using saborGregoNew.Repository;

namespace SaborGregoNew.Pages
{
    public class CardapioModel : PageModel
    {
        private readonly IProdutoRepository _produtoService;
        private readonly ICarrinhoRepository _carrinhoService;

        public CardapioModel(IProdutoRepository produtoService, ICarrinhoRepository carrinhoService)
        {
            _produtoService = produtoService;
            _carrinhoService = carrinhoService;
        }

        // Inicializado para evitar CS8618 e NRE na View
        public List<Models.Produto> Produtos { get; set; } = new List<Models.Produto>(); 

        public async Task OnGetAsync()
        {
            Produtos = await _produtoService.SelectAllAsync();
        }

        // ⭐️ CORREÇÃO: Método assíncrono para usar 'await'
        public async Task<IActionResult> OnPost(int produtoId)
        {
            if (produtoId <= 0)
            {
                TempData["MensagemErro"] = "ID de produto inválido.";
                ModelState.AddModelError(string.Empty, "ID de produto inválido.");
                // Recarrega os produtos para a View ter os dados necessários
                await OnGetAsync(); 
                return Page(); 
            }

            try
            {
                // ⭐️ Chama o serviço de forma assíncrona (await)
                await _carrinhoService.AdicionarAoCarrinhoAsync(produtoId);

                // ⭐️ Padrão PRG: Redireciona para evitar re-submissão
                TempData["MensagemSucesso"] = "Produto adicionado ao carrinho com sucesso!";
                return RedirectToPage("Pedido/Carrinho/Carrinho"); 
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao adicionar produto ao carrinho: " + ex.Message;
                ModelState.AddModelError(string.Empty, "Erro ao adicionar produto ao carrinho: " + ex.Message);
                
                // Em caso de erro, recarrega os dados antes de retornar a Page()
                await OnGetAsync(); 
                return RedirectToPage("Cardapio"); 
            }
        }
    }
}