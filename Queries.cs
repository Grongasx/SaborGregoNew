namespace saborGregoNew.Repository
{
    public static class Queries
    {
        // Endereço
        public const string EnderecoInsert = "INSERT INTO Enderecos (Apelido, Logradouro, Numero, Complemento, Bairro, UsuarioId) VALUES (@Apelido, @Logradouro, @Numero, @Complemento, @Bairro, @UsuarioId)";
        public const string EnderecoUpdate = "UPDATE Enderecos SET Apelido = @Apelido, Logradouro = @Logradouro, Numero = @Numero, Complemento = @Complemento, Bairro = @Bairro, UsuarioId = @UsuarioId WHERE Id = @Id";
        public const string SelectByUserId = "SELECT * FROM Enderecos WHERE UsuarioId = @UsuarioId";
        public const string SelectById = "SELECT * FROM Enderecos WHERE Id = @Id";
        public const string Delete = "DELETE FROM Enderecos WHERE Id = @Id";
        
        
        //Admin Dashboard
        public const string SalesQuantityByCategory = "SELECT COUNT(*) FROM Categorias WHERE Categoria = @Categoria";
        public const string SalesSumByCategory =  "SELECT COUNT(*) FROM Categorias WHERE Categoria = @Categoria";
    }
}