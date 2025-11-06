using Microsoft.EntityFrameworkCore;
using SaborGregoNew.Data;
using SaborGregoNew.Models;

namespace SaborGregoNew.Repository;

public class ProdutoRepository
{
    private readonly ApplicationDbContext _context;

    public ProdutoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Create(Produto produto)
    {
        await _context.Produtos.AddAsync(produto);

        await _context.SaveChangesAsync();
    }

    public async Task<List<Produto>> FindAll()
    {
        return await _context.Produtos.ToListAsync();
    }
}