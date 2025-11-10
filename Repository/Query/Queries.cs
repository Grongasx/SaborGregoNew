using System.Data;

namespace saborGregoNew.Repository
{
    public static class Queries
    {
        // Endereço
        public const string EnderecoInsert = "INSERT INTO Enderecos (Apelido, Logradouro, Numero, Complemento, Bairro, UsuarioId) VALUES (@Apelido, @Logradouro, @Numero, @Complemento, @Bairro, @UsuarioId)";
        public const string EnderecoUpdate = "UPDATE Enderecos SET Id = @Id, Apelido = @Apelido, Logradouro = @Logradouro, Numero = @Numero, Complemento = @Complemento, Bairro = @Bairro, UsuarioId = @UsuarioId WHERE Id = @Id";
        public const string EnderecoSelectByUserId = "SELECT * FROM Enderecos WHERE UsuarioId = @UsuarioId";
        public const string EnderecoSelectById = "SELECT * FROM Enderecos WHERE Id = @Id";
        public const string EnderecoDeleteById = "DELETE FROM Enderecos WHERE Id = @Id";
        public const string EnderecoSelectByIdAndUserId = "SELECT Id, Apelido, Logradouro, Numero, Complemento, Bairro, UsuarioId FROM Enderecos WHERE Id = @Id AND UsuarioId = @UsuarioId";

        //Pedido
        public const string PedidoInsert = "INSERT INTO Pedidos (DataPedido, ValorTotal, Status, ClienteId, EnderecoId, MetodoPagamento) VALUES (@DataPedido, @ValorTotal, @Status, @ClienteId, @EnderecoId, @MetodoPagamento)";
        public const string PedidoUpdateStatus = "UPDATE Pedidos SET Status = @Status WHERE Id = @Id";
        public const string DetalhePedidoInsert = "INSERT INTO DetalhesPedido (PedidoId, ProdutoId, NomeProduto, PrecoUnitario, Quantidade) VALUES (@PedidoId, @ProdutoId, @NomeProduto, @PrecoUnitario, @Quantidade)";
        public const string PedidoSelectById = "SELECT * FROM Pedidos WHERE Id = @Id";

        //Produto
        public const string ProdutoInsert = "INSERT INTO Produtos (Id, Categoria, Descricao, Imagem, Nome, Preco) VALUES (@Id, @Categoria, @Descricao, @Imagem, @Nome, @Preco)";
        public const string ProdutoSelectAll = "SELECT Id, Categoria, Descricao, Imagem, Nome, Preco FROM Produtos";
        public const string ProdutoSelectById = "SELECT Id, Categoria, Descricao, Imagem, Nome, Preco FROM Produtos WHERE Id = @Id";
        public const string ProdutoUpdate = "UPDATE Produtos SET Nome = @Nome, Descricao = @Descricao, Preco = @Preco, Categoria = @Categoria, Imagem = @Imagem WHERE Id = @Id";
        public const string ProdutoDeleteById = "DELETE FROM Produtos WHERE Id = @Id";

        //Usuario
        public const string UsuarioLogin = "SELECT * FROM Usuarios WHERE Email = @Email";
        public const string UsuarioRegistrar = "INSERT INTO Usuarios (Nome, Telefone, Email, Senha, Role) VALUES (@Nome, @Telefone, @Email, @Senha, @Role)";
        public const string UsuarioListagem = "SELECT Id, Nome, Telefone, Email, Senha, Role FROM Usuarios";
        public const string UsuarioById = "SELECT Id, Nome, Telefone, Email, Senha, Role FROM Usuarios WHERE Id = @Id";
        public const string UsuarioUpdateById = "UPDATE Usuarios SET Nome = @Nome, Telefone = @Telefone, Email = @Email, Senha = @Senha, Role = @Role WHERE Id = @Id";
        public const string UsuarioDeleteById = "DELETE FROM Usuarios WHERE Id = @Id";







        //=================================================//
        //===========Coloca As Queries aqui Caue===========//
        //=================================================//
        //Dashboard
        public const string PedidosMensais = "";
        public const string PedidosDiarios = "";
        public const string ProdutosMensais = "";
        public const string ProdutosDiarios = "";
        public const string FuncionariosMensais = "";
        public const string FuncionariosDiarios = "";


        public const string GetVendasQuantidadePorCategoria = @" SELECT P.Categoria, SUM(DP.Quantidade) AS QuantidadeTotal FROM DetalhesPedidos AS DP JOIN Produtos AS P ON P.Id = DP.ProdutoId GROUP BY P.Categoria;";
        public const string GetVendasReceitaPorCategoria = @" SELECT P.Categoria, SUM(DP.PrecoUnitario) AS ReceitaTotal FROM DetalhesPedidos AS DP JOIN Produtos AS P ON P.Id = DP.ProdutoId GROUP BY P.Categoria;";
        public const string GetProdutoMaisRentavel = @" SELECT DP.NomeProduto, SUM(DP.PrecoUnitario) AS ReceitaTotal FROM DetalhesPedidos AS DP GROUP BY DP.NomeProduto ORDER BY ReceitaTotal DESC";
        public const string GetReceitaPorProdutoId = @"SELECT DP.ProdutoId, SUM(DP.PrecoUnitario * DP.Quantidade) AS ValorVendidoTotal, SUM(DP.Quantidade) AS QuantidadeTotalVendida FROM DetalhesPedidos AS DP GROUP BY  DP.ProdutoId ORDER BY QuantidadeTotalVendida DESC;";
    }
}