namespace SaborGregoNew.Repository;

public interface ICarrinhoServices
{
    public void AtualizarQuantidade(int produtoId, int novaQuantidade);
}