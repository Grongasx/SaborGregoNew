using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Repository;
using SaborGregoNew.Models;

namespace SaborGregoNew.Pages
{
    public class CarrinhoModel : PageModel
    {
        private readonly ICarrinhoRepository _carrinhoService;

        public CarrinhoModel(ICarrinhoRepository carrinhoService)
        {
            _carrinhoService = carrinhoService;
        }

        // Propriedade pública para ser acessada na View
        public List<CarrinhoItem> Carrinho { get; set; } = new List<CarrinhoItem>();
        
        // Propriedade calculada para o total
        public decimal CarrinhoTotal { get; set; }

        public void OnGet()
        {
            // Carrega o carrinho via Service
            Carrinho = _carrinhoService.GetCarrinho();
            CarrinhoTotal = _carrinhoService.CalcularTotal();
        }

        // Handler para Remover um Item
        public IActionResult OnPostRemoverItem(int produtoId)
        {
            _carrinhoService.RemoverItem(produtoId);
            
            // Redireciona para a mesma página para atualizar o estado
            return RedirectToPage();
        }

        // Handler para Atualizar a Quantidade
        // Novo método para AJAX
        public IActionResult OnPostAtualizarQuantidadeAjax(int produtoId, int novaQuantidade)
        {
            if (novaQuantidade <= 0)
            {
                _carrinhoService.RemoverItem(produtoId);
            }
            else
            {
                _carrinhoService.AtualizarQuantidade(produtoId, novaQuantidade);
            }

            // Recalcula tudo para devolver ao front-end
            var carrinho = _carrinhoService.GetCarrinho();
            var total = _carrinhoService.CalcularTotal();
            var itemAtualizado = carrinho.FirstOrDefault(x => x.ProdutoId == produtoId);
            var qtdTotalItens = carrinho.Sum(x => x.Quantidade);

            // Retorna JSON com os dados formatados
            return new JsonResult(new 
            { 
                deveRemover = novaQuantidade <= 0,
                novoSubtotal = itemAtualizado?.SubTotal.ToString("C"),
                novoTotal = total.ToString("C"),
                itensTotal = qtdTotalItens
            });
        }

        public IActionResult OnPostLimparCarrinho()
        {
            _carrinhoService.ClearCarrinho();
            return RedirectToPage();
        }

        public IActionResult OnPostCheckout()
        {
            return RedirectToPage("/Pedido/Carrinho/Checkout");
        }
    }
}