using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Models;

namespace SaborGregoNew.Repository
{
    public interface IUsuarioRepository
    {
        Task Create(RegisterUserDto ModeloUsuario);
        Task<Usuario?> Login(LoginDTO ModeloUsuario);
        Task<List<Usuario>> SelectAllAsync();
        Task<Usuario?> SelectByIdAsync(int id);
        Task UpdateById(int id, RegisterUserDto ModeloUsuario);
        Task DeleteById(int id);
    }
}