using SaborGregoNew.Repository;
using SaborGregoNew.Models;

namespace SaborGregoNew.Services;


public class CarrinhoService : ICarrinhoServices
{
    private readonly ICarrinhoRepository _carrinhoRepository;

    public CarrinhoService(ICarrinhoRepository carrinhoRepository)
    {
        _carrinhoRepository = carrinhoRepository;
    }

    public void AtualizarQuantidade(int produtoId, int novaQuantidade)
    {
        var carrinho = _carrinhoRepository.GetCarrinho();
        var item = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);

        if (item == null)
        {
            throw new Exception("O item não existe");
        }
        if (novaQuantidade <= 0)
        {
            throw new Exception("O valor tem que ser valido");
        }
        else
        {
            item.Quantidade = novaQuantidade;
        }
    }
}