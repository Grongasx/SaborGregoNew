using System.Data;
using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Models;

namespace SaborGregoNew.Repository
{
    public interface IEnderecoRepository
    {
        Task Create(CadastroEnderecoDTO ModeloEndereco, int usuarioId);
        Task<List<Endereco>> SelectAllByUserIdAsync(int usuarioId);
        Task<Endereco?> SelectByIdAsync(int id);
        Task UpdateById(int Id, CadastroEnderecoDTO ModeloEndereco);
        Task<Endereco?> GetByIdAndUserIdAsync(int enderecoId, int usuarioId);
        Task DesativarAsync(int Id);
    }
}