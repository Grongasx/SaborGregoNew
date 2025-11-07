
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Services;

namespace SaborGregoNew.Pages
{
    public class CardapioModel : PageModel
    {
        private readonly ProdutoService _produtoService;
        private readonly CarrinhoService _carrinhoService;


        public CardapioModel(ProdutoService produtoService, CarrinhoService carrinhoService)
        {
            _produtoService = produtoService;
            _carrinhoService = carrinhoService;
        }

        public List<Models.Produto> Produtos { get; set; }

        public async Task OnGetAsync()
        {
            Produtos = await _produtoService.GetAllAsync();
        }

        public IActionResult OnPostAdicionarAoCarrinho(int produtoId)
        {
            if (produtoId <= 0)
            {
                // Tratamento de erro básico
                return RedirectToPage(); 
            }

            try
            {
                // 1. Chama o Serviço para adicionar. 
                // O Serviço irá buscar o produto no banco e salvar na Sessão.
                _carrinhoService.AdicionarAoCarrinho(produtoId);

                // 2. Redireciona o usuário para a página do carrinho para ver o item
                return RedirectToPage("/Pedido/Carrinho/Carrinho"); 
            }
            catch (Exception ex)
            {
                // Lidar com exceções (ex: produtoId não encontrado)
                ModelState.AddModelError(string.Empty, "Erro ao adicionar produto ao carrinho: " + ex.Message);
                
                // Recarrega os produtos para a tela atual
                // Produtos = _produtoService.GetAllProdutos(); 
                return Page(); 
            }
        }

    }
}
