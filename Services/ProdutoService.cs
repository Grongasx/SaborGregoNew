using SaborGregoNew.DTOs.Produtos;
using SaborGregoNew.Models;
using SaborGregoNew.Repository;

namespace SaborGregoNew.Services;

public class ProdutoService
{
    private readonly ProdutoRepository _produtoRepository;

    public ProdutoService(ProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }
    public async Task Create(CreateProdutoDTO dto)
    {
        var produto = new Produto
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Preco = dto.Preco,
            Categoria = dto.Categoria
        };

        await _produtoRepository.Create(produto);
    }

    public async Task<List<Produto>> GetAllAsync()
    {
        return await _produtoRepository.FindAll();
    }
}