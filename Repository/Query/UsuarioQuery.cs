namespace SaborGregoNew.Repository.Query;

public static class UsuarioQuery
{
    public const string UsuarioLogin = "SELECT Id, Email, Nome, Role, Senha, Telefone FROM Usuarios WHERE Email = @Email";
    public const string UsuarioRegistrar = "INSERT INTO Usuarios (Nome, Telefone, Email, Senha, Role) VALUES (@Nome, @Telefone, @Email, @Senha, @Role)";
    public const string UsuarioListagem = "SELECT Id, Email, Nome, Role, Senha, Telefone FROM Usuarios";
    public const string UsuarioById = "SELECT Id, Email, Nome, Role, Senha, Telefone FROM Usuarios WHERE Id = @Id";
    public const string UsuarioUpdateById = "UPDATE Usuarios SET Nome = @Nome, Telefone = @Telefone, Email = @Email, Senha = @Senha, Role = @Role WHERE Id = @Id";
    public const string UsuarioDeleteById = "DELETE FROM Usuarios WHERE Id = @Id";
}