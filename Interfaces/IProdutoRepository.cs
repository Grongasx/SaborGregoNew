using SaborGregoNew.DTOs.Produtos;
using SaborGregoNew.Models;

namespace SaborGregoNew.Repository
{
    public interface IProdutoRepository
    {
        Task Create(ProdutoDTO ModeloProduto);

        Task<List<Produto>> SelectAllAsync();
        
        Task<Produto?> SelectByIdAsync(int id);

        Task UpdateById(int id, ProdutoDTO ModeloProduto);
        Task DesativarAsync(int id);
        Task AtivarAsync(int id);

        Task DeleteById(int id);
    }
}