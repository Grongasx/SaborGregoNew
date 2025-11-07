using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Models;
using SaborGregoNew.Services;

namespace SaborGregoNew.Pages
{
    public class CarrinhoModel : PageModel
    {
        private readonly CarrinhoService _carrinhoService;

        public CarrinhoModel(CarrinhoService carrinhoService)
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
        public IActionResult OnPostAtualizarQuantidade(int produtoId, int novaQuantidade)
        {
            if (novaQuantidade <= 0)
            {
                // Se a nova quantidade for <= 0, usa o método de remover item
                _carrinhoService.RemoverItem(produtoId);
            }
            else
            {
                _carrinhoService.AtualizarQuantidade(produtoId, novaQuantidade);
            }

            return RedirectToPage();
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