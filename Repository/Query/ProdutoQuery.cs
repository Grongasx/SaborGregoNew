namespace saborGregoNew.Repository.Query;

public static class ProdutoQuery
{
    public const string ProdutoInsert = "INSERT INTO Produtos (Id, Categoria, Descricao, Imagem, Nome, Preco) VALUES (@Id, @Categoria, @Descricao, @Imagem, @Nome, @Preco)";
    public const string ProdutoSelectAll = "SELECT Id, Categoria, Descricao, Imagem, Nome, Preco FROM Produtos";
    public const string ProdutoSelectById = "SELECT Id, Categoria, Descricao, Imagem, Nome, Preco FROM Produtos WHERE Id = @Id";
    public const string ProdutoUpdate = "UPDATE Produtos SET Nome = @Nome, Descricao = @Descricao, Preco = @Preco, Categoria = @Categoria, Imagem = @Imagem WHERE Id = @Id";
    public const string ProdutoDeleteById = "DELETE FROM Produtos WHERE Id = @Id";
    public const string DesativarProduto = "UPDATE Produtos SET Ativo = 0 WHERE Id = @Id";
}