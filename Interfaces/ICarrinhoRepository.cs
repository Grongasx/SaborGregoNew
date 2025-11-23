
using SaborGregoNew.Models;

namespace saborGregoNew.Repository
{
    public interface ICarrinhoRepository
    {
        List<CarrinhoItem> GetCarrinho();
        void SaveCarrinho(List<CarrinhoItem> carrinho);
        void ClearCarrinho();
        void AtualizarQuantidade(int produtoId, int novaQuantidade);
        Task AdicionarAoCarrinhoAsync(Produto produto);
        void RemoverItem(int produtoId);
        decimal CalcularTotal();
        void AtualizarCarrinhoCompleto(List<CarrinhoItem> novosItensCarrinho);
    }
}