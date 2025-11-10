using SaborGregoNew.DTOs.Produtos;
using SaborGregoNew.Models;

namespace saborGregoNew.Repository
{
    public interface IProdutoRepository
    {
        Task Create(ProdutoDTO ModeloProduto);

        Task<List<Produto>> SelectAllAsync();
        
        Task<Produto?> SelectByIdAsync(int id);

        Task UpdateById(int id, ProdutoDTO ModeloProduto);

        Task DeleteById(int id);
    }
}