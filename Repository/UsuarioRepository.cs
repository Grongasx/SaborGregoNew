using Microsoft.EntityFrameworkCore;
using SaborGregoNew.Data;
using SaborGregoNew.Models;

namespace SaborGregoNew.Repository;

public class UsuarioRepository
{
    private readonly ApplicationDbContext _context;

    public UsuarioRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Create(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);

        await _context.SaveChangesAsync();
    }

    public async Task<List<Usuario>> FindAll()
    {
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<Usuario?> GetByEmail(string? Email)
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            return null;
        }
        else
        {
            return await _context.Usuarios.
                FirstOrDefaultAsync(u => u.Email == Email);
        }
    }
}