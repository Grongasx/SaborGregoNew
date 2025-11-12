using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Models;

namespace saborGregoNew.Repository
{
    public interface IUsuarioRepository
    {
        Task Create(RegisterDto ModeloUsuario);
        Task<Usuario?> Login(LoginDTO ModeloUsuario);
        Task<List<Usuario>> SelectAllAsync();
        Task<Usuario?> SelectByIdAsync(int id);
        Task UpdateById(int id, RegisterDto ModeloUsuario);
        Task DeleteById(int id);
    }
}