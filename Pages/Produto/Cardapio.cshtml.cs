using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Repository;
using SaborGregoNew.Models;
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

        public List<SaborGregoNew.Models.Produto> Produtos { get; set; } = new List<SaborGregoNew.Models.Produto>(); 
        public async Task OnGetAsync()
        {
            var todosProdutos = await _produtoService.SelectAllAsync();
            Produtos = todosProdutos.Where(p => p.Ativo).ToList();
        }
        public async Task<IActionResult> OnPostAsync(int produtoId)
        {
            if (produtoId <= 0)
            {
                TempData["MensagemErro"] = "ID de produto inválido.";
                return RedirectToPage();
            }
            var produto = await _produtoService.SelectByIdAsync(produtoId); 
            if (produto == null)
            {
                TempData["MensagemErro"] = "Produto não encontrado.";
                return RedirectToPage();
            }
            
            try
            {
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