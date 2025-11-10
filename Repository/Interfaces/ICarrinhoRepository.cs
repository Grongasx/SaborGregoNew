
using SaborGregoNew.Models;

namespace saborGregoNew.Repository
{
    public interface ICarrinhoRepository
    {
        List<CarrinhoItem> GetCarrinho();
        void SaveCarrinho(List<CarrinhoItem> carrinho);
        void ClearCarrinho();
        Task AdicionarAoCarrinhoAsync(int produtoId, int quantidade = 1);
        void RemoverItem(int produtoId);
        decimal CalcularTotal();
        void AtualizarQuantidade(int produtoId, int novaQuantidade);
    }
}