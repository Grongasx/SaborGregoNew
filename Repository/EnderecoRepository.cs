using Microsoft.EntityFrameworkCore;
using SaborGregoNew.Data;
using SaborGregoNew.Models;

namespace SaborGregoNew.Repository;

public class EnderecoRepository
{
    private readonly ApplicationDbContext _context;

    public EnderecoRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Create(Endereco endereco)
    {
        await _context.Enderecos.AddAsync(endereco);

        await _context.SaveChangesAsync();
    }

    public async Task<List<Endereco>> FindAllById(int? Id)
    {
        return await _context.Enderecos.ToListAsync();
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
    public List<Endereco> GetAllByUserId(int userId)
    {
        return _context.Enderecos.Where(e => e.UsuarioId == userId).ToList();
    }
    public Endereco GetById(int id)
    {
        return _context.Enderecos.FirstOrDefault(e => e.Id == id);
    }
    public async Task UpdateAsync(Endereco endereco)
    {
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var endereco = GetById(id);
        if (endereco != null)
        {
            _context.Enderecos.Remove(endereco);
            _context.SaveChanges();
        }
    }
}

    

        

    