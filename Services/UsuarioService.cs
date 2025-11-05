using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Models;
using SaborGregoNew.Repository;

namespace SaborGregoNew.Services;

public class UsuarioService
{
    private readonly UsuarioRepository _usuarioRepository;
    
    public UsuarioService(UsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<bool> Register(RegisterUserDto dto)
    {
        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha,
            Telefone = dto.Telefone,
        };
        
        await _usuarioRepository.Create(usuario);
        
        return true;
    }

    public async Task<List<Usuario>> ListarUsuarios()
    {
        return await _usuarioRepository.FindAll();
    }

    public async Task<Usuario?> Logar(LoginUserDTO dto)
    {
        // A checagem de null/vazio é feita pelo ModelState no PageModel.

        var usuario = await _usuarioRepository.GetByEmail(dto.Email);

        // 1. O Serviço retorna NULL se o usuário não existir OU se a senha não bater
        if (usuario != null && usuario.Senha == dto.Senha)
        {
            return usuario; // SUCESSO
        }
        else
        {
            return null; // FALHA (seja por email ou senha)
        }
    }
}