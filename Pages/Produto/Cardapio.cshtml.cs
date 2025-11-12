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
        public async Task<IActionResult> OnPostAsync(int produtoId)
        {
            if (produtoId <= 0)
            {
                TempData["MensagemErro"] = "ID de produto inválido.";
                return RedirectToPage();
            }

            // 2. Busque o produto completo no banco de dados
            var produto = await _produtoService.SelectByIdAsync(produtoId); 

            if (produto == null)
            {
                TempData["MensagemErro"] = "Produto não encontrado.";
                return RedirectToPage();
            }
            
            try
            {
                // 3. Adicione o produto completo ao carrinho
                await _carrinhoService.AdicionarAoCarrinhoAsync(produto);

                TempData["MensagemSucesso"] = "Produto adicionado ao carrinho com sucesso!";
                return RedirectToPage("/Pedido/Carrinho/Carrinho");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao adicionar produto ao carrinho: " + ex.Message;
                await OnGetAsync(); 
                return RedirectToPage("Cardapio"); 
            }
        }
    }
}