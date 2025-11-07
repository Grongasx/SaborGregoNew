using Microsoft.EntityFrameworkCore;
using SaborGregoNew.Data;
using SaborGregoNew.Models;

namespace SaborGregoNew.Repository
{
    public class EnderecoRepository
    {
        private readonly ApplicationDbContext _context;

        public EnderecoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Endereco>> GetEnderecosByUsuarioId(int usuarioId)
        {
            return await _context.Enderecos
                                 .Where(e => e.UsuarioId == usuarioId)
                                 .ToListAsync();
        }

        public async Task AddEndereco(Endereco endereco)
        {
            await _context.Enderecos.AddAsync(endereco);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEndereco(Endereco endereco)
        {
            _context.Enderecos.Update(endereco);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEndereco(Endereco endereco)
        {
            _context.Enderecos.Remove(endereco);
            await _context.SaveChangesAsync();
        }

        public async Task<Endereco?> GetEnderecoById(int id)
        {
            return await _context.Enderecos.FindAsync(id);
        }
    }
}