
using SaborGregoNew.Models;

namespace SaborGregoNew.Repository
{
    public interface ICarrinhoRepository
    {
        List<CarrinhoItem> GetCarrinho();
        void SaveCarrinho(List<CarrinhoItem> carrinho);
        void ClearCarrinho();
        Task AdicionarAoCarrinhoAsync(Produto produto);
        void RemoverItem(int produtoId);
        decimal CalcularTotal();
        void AtualizarQuantidade(int produtoId, int novaQuantidade);
    }
}