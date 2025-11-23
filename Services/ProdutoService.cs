using SaborGregoNew.DTOs.Produtos;
using SaborGregoNew.Models;
using saborGregoNew.Repository;
using SaborGregoNew.Repository;

namespace SaborGregoNew.Services;

public class ProdutoService
{
    private readonly ProdutoSqliteRepository _produtoRepository;

    public ProdutoService(ProdutoSqliteRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }
    
    public async Task Create(CreateProdutoDTO dto)
    {
        var produtoDTO = new ProdutoDTO 
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Preco = dto.Preco,
            Categoria = dto.Categoria,
            Imagem = dto.Imagem != null && dto.Imagem.Length > 0 ? Convert.ToBase64String(dto.Imagem) : string.Empty
        };
        await _produtoRepository.Create(produtoDTO);
    }

    public async Task<List<Produto>> GetAllAsync()
    {
        return await _produtoRepository.SelectAllAsync();
    }
}