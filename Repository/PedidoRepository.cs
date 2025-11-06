using Microsoft.EntityFrameworkCore;
using SaborGregoNew.Data;
using SaborGregoNew.Models;

namespace SaborGregoNew.Repository;

public class PedidoRepository
{
    private readonly ApplicationDbContext _context;

    public PedidoRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Create(Pedido Pedido)
    {
        await _context.Pedidos.AddAsync(Pedido);

        await _context.SaveChangesAsync();
    }

    public async Task<List<Pedido>> FindAllById(int? Id)
    {
        return await _context.Pedidos.ToListAsync();
    }

    public async Task<Usuario?> GetByUserId(int? Id)
    {
        if (Id== null)
        {
            Console.WriteLine("O Usuario não foi Encontrado");
            return null;
        }
        else
        {
            return await _context.Usuarios.
                FirstOrDefaultAsync(u => u.Id == Id);
        }
    }
    public Pedido GetById(int id)
    {
        return _context.Pedidos.FirstOrDefault(e => e.Id == id);
    }
    public async Task UpdateAsync(Pedido Pedido)
    {
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var Pedido = GetById(id);
        if (Pedido != null)
        {
            _context.Pedidos.Remove(Pedido);
            _context.SaveChanges();
        }
    }
}

    

        

    