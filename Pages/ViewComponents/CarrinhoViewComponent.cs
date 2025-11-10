using Microsoft.AspNetCore.Mvc;
using saborGregoNew.Repository;

namespace SaborGregoNew.ViewComponents
{
    // O nome da classe deve terminar com "ViewComponent"
    public class CarrinhoViewComponent : ViewComponent
    {
        private readonly ICarrinhoRepository _carrinhoService;

        // Injeta o Serviço do Carrinho
        public CarrinhoViewComponent(ICarrinhoRepository carrinhoService)
        {
            _carrinhoService = carrinhoService;
        }

        // O método InvokeAsync é chamado quando o componente é renderizado
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Usa o serviço para obter a lista de itens
            var carrinho = _carrinhoService.GetCarrinho();
            
            // Retorna a contagem total de itens (útil para badges)
            int totalItens = carrinho.Sum(i => i.Quantidade);
            
            // Passa o dado para o template do componente
            return View(totalItens); 
        }
    }
}