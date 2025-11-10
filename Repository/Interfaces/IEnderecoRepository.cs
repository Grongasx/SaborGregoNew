using System.Data;
using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Models;

namespace saborGregoNew.Repository
{
    public interface IEnderecoRepository
    {
        Task Create(EnderecoDTO ModeloEndereco, int usuarioId);

        Task<List<Endereco>> SelectAllByUserIdAsync(int usuarioId);

        Task<Endereco?> SelectByIdAsync(int id);

        Task<Endereco?> SelectByUserIdAsync(int usuarioId);

        Task UpdateById(int id, EnderecoDTO ModeloEndereco);

        Task DeleteById(int id);
        
        Task<Endereco?> GetByIdAndUserIdAsync(int enderecoId, int usuarioId);
    
    }
}