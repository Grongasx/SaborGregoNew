using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using saborGregoNew.Repository;
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

        // Propriedade pública para carregar o carrinho na exibição (OnGet)
        public List<CarrinhoItem> Carrinho { get; set; } = new List<CarrinhoItem>();
        
        // Propriedade calculada para o total
        public decimal CarrinhoTotal { get; set; }
        
        // ⭐️ NOVO: Propriedade para RECEBER a lista atualizada do formulário no POST
        [BindProperty]
        public List<CarrinhoItem> CarrinhoForm { get; set; } = new List<CarrinhoItem>();

        public void OnGet()
        {
            // Carrega o carrinho via Service para EXIBIR
            Carrinho = _carrinhoService.GetCarrinho();
            CarrinhoTotal = _carrinhoService.CalcularTotal();

            // ⭐️ IMPORTANTE: Popula CarrinhoForm para que os valores dos inputs sejam exibidos
            // Isso é um padrão comum em Razor Pages para formulários complexos.
            CarrinhoForm = Carrinho;
        }

        // Handler para Remover um Item (Mantido, funciona com POST separado)
        public IActionResult OnPostRemoverItem(int produtoId)
        {
            _carrinhoService.RemoverItem(produtoId);
            return RedirectToPage();
        }

        // Handler para Atualizar a Quantidade (Mantido, mas não será mais usado pelos botões +/-, 
        // a menos que você queira que ele seja chamado via AJAX, o que você descartou)
        public IActionResult OnPostAtualizarQuantidade(int produtoId, int novaQuantidade)
        {
             // ... Lógica de atualização (mantida) ...
             if (novaQuantidade <= 0)
             {
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

        // ⭐️ HANDLER DE CHECKOUT CORRIGIDO
        public IActionResult OnPostCheckout()
        {
            // ⭐️ O CarrinhoForm agora contém os dados alterados nos inputs.
            // Aqui, você deve atualizar o carrinho na sessão/DB com os novos valores
            // (após validação de estoque, etc.) ANTES de prosseguir para o pagamento.
            if (CarrinhoForm != null && CarrinhoForm.Any())
            {
                // 1. Atualiza o serviço do carrinho com os valores do formulário
                _carrinhoService.AtualizarCarrinhoCompleto(CarrinhoForm); // **Você precisa implementar este método no seu ICarrinhoRepository**
                
                // 2. Redireciona para o checkout
                return RedirectToPage("/Pedido/Carrinho/Checkout");
            }
            
            TempData["MensagemErro"] = "O carrinho está vazio ou ocorreu um erro na submissão.";
            return RedirectToPage();
        }
    }
}