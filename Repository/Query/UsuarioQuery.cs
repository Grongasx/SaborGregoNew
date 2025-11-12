namespace SaborGregoNew.Repository.Query;

public static class UsuarioQuery
{
    public const string UsuarioLogin = "SELECT * FROM Usuarios WHERE Email = @Email";
    public const string UsuarioRegistrar = "INSERT INTO Usuarios (Nome, Telefone, Email, Senha, Role) VALUES (@Nome, @Telefone, @Email, @Senha, @Role)";
    public const string UsuarioListagem = "SELECT Id, Nome, Telefone, Email, Senha, Role FROM Usuarios";
    public const string UsuarioById = "SELECT Id, Nome, Telefone, Email, Senha, Role FROM Usuarios WHERE Id = @Id";
    public const string UsuarioUpdateById = "UPDATE Usuarios SET Nome = @Nome, Telefone = @Telefone, Email = @Email, Senha = @Senha, Role = @Role WHERE Id = @Id";
    public const string UsuarioDeleteById = "DELETE FROM Usuarios WHERE Id = @Id";
}