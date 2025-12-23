namespace SaborGregoNew.Repository.Query;

public static class EnderecoQuery
{
    public const string EnderecoInsert = "INSERT INTO Enderecos (Apelido, Logradouro, Numero, Complemento, Bairro, UsuarioId) VALUES (@Apelido, @Logradouro, @Numero, @Complemento, @Bairro, @UsuarioId)";
    public const string EnderecoUpdate = "UPDATE Enderecos SET Id = @Id, Apelido = @Apelido, Logradouro = @Logradouro, Numero = @Numero, Complemento = @Complemento, Bairro = @Bairro, UsuarioId = @UsuarioId WHERE Id = @Id";
    public const string EnderecoSelectByUserId = "SELECT * FROM Enderecos WHERE UsuarioId = @UsuarioId";
    public const string EnderecoSelectById = "SELECT * FROM Enderecos WHERE Id = @Id";
    public const string EnderecoDeleteById = "DELETE FROM Enderecos WHERE Id = @Id";
    public const string EnderecoSelectByIdAndUserId = "SELECT Id, Apelido, Logradouro, Numero, Complemento, Bairro, UsuarioId FROM Enderecos WHERE Id = @Id AND UsuarioId = @UsuarioId";
    public const string EnderecoDesativar = "UPDATE Enderecos SET Ativo = 0 WHERE Id = @Id";
}