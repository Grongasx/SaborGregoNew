namespace SaborGregoNew.Repository.Query;

public static class EnderecoQuery
{
    // INSERT e UPDATE permanecem iguais
    public const string EnderecoInsert = "INSERT INTO Enderecos (Apelido, Logradouro, Numero, Complemento, Bairro, UsuarioId, Ativo) VALUES (@Apelido, @Logradouro, @Numero, @Complemento, @Bairro, @UsuarioId, 0)";
    public const string EnderecoUpdate = "UPDATE Enderecos SET Apelido = @Apelido, Logradouro = @Logradouro, Numero = @Numero, Complemento = @Complemento, Bairro = @Bairro, UsuarioId = @UsuarioId WHERE Id = @Id";
    
    // --- QUERIES DE SELEÇÃO (GET) REFATORADAS PARA INCLUIR ATIVO = 1 ---
    
    // Seleciona todos os endereços ATIVOS de um usuário.
    public const string EnderecoSelectByUserId = "SELECT * FROM Enderecos WHERE UsuarioId = @UsuarioId AND Ativo = 0";
    
    // Seleciona um endereço ATIVO pelo seu ID.
    public const string EnderecoSelectById = "SELECT * FROM Enderecos WHERE Id = @Id AND Ativo = 0";
    
    // Seleciona um endereço ATIVO pelo seu ID e UsuarioId.
    public const string EnderecoSelectByIdAndUserId = "SELECT Id, Apelido, Logradouro, Numero, Complemento, Bairro, UsuarioId, Ativo FROM Enderecos WHERE Id = @Id AND UsuarioId = @UsuarioId AND Ativo = 0";

    // --- QUERIES DE EXCLUSÃO/DESATIVAÇÃO E DELEÇÃO PERMANECEM IGUAIS ---

    // Desativa (Soft Delete): A query EnderecoDesativar define Ativo = @ativo (onde @ativo deve ser 0/Inativo).
    public const string EnderecoDesativar = "UPDATE Enderecos SET Ativo = @ativo WHERE Id = @Id";
    
    // Deletar (Hard Delete):
    public const string EnderecoDeleteById = "DELETE FROM Enderecos WHERE Id = @Id";
}