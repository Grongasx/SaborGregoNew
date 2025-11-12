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
        Task UpdateById(int Id, EnderecoDTO ModeloEndereco);
        Task<Endereco?> GetByIdAndUserIdAsync(int enderecoId, int usuarioId);
        Task DesativarAsync(int Id);
    }
}