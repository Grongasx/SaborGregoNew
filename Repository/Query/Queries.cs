using System.Data;

namespace saborGregoNew.Repository
{
    public static class Queries
    {
        // Endereço
        public const string EnderecoInsert = "INSERT INTO Enderecos (Apelido, Logradouro, Numero, Complemento, Bairro, UsuarioId) VALUES (@Apelido, @Logradouro, @Numero, @Complemento, @Bairro, @UsuarioId)";
        public const string EnderecoUpdate = "UPDATE Enderecos SET Apelido = @Apelido, Logradouro = @Logradouro, Numero = @Numero, Complemento = @Complemento, Bairro = @Bairro, UsuarioId = @UsuarioId WHERE Id = @Id";
        public const string EnderecoSelectByUserId = "SELECT * FROM Enderecos WHERE UsuarioId = @UsuarioId";
        public const string EnderecoSelectById = "SELECT * FROM Enderecos WHERE Id = @Id";
        public const string EnderecoDeleteById = "DELETE FROM Enderecos WHERE Id = @Id";
        public const string EnderecoSelectByIdAndUserId = "SELECT Id, Apelido, Logradouro, Numero, Complemento, Bairro, UsuarioId FROM Enderecos WHERE Id = @Id AND UsuarioId = @UsuarioId";

        //Pedido
        public const string PedidoUpdateStatus = "UPDATE Pedidos SET Status = @Status WHERE Id = @Id";
        public const string DetalhePedidoInsert = "INSERT INTO DetalhesPedido (PedidoId, ProdutoId, Imagem, NomeProduto, PrecoUnitario, Quantidade) VALUES (@PedidoId, @ProdutoId, @Imagem, @NomeProduto, @PrecoUnitario, @Quantidade)";
        public const string PedidoSelectById = "SELECT * FROM Pedidos WHERE Id = @Id";
        public const string PedidosComItensPorStatus = "SELECT p.*, d.* FROM Pedidos p INNER JOIN DetalhesPedido d ON p.Id = d.PedidoId WHERE p.Status = @Status";
        public const string GetPedidosComItensPorStatus = "SELECT p.Id AS PedidoId, p.DataPedido, p.TotalPedido, p.Status, p.ClienteId, p.EnderecoId, p.MetodoPagamento, p.FuncionarioId, p.EntregadorId, dp.Id AS DetalhePedidoId, dp.ProdutoId, dp.NomeProduto, dp.PrecoUnitario, dp.Quantidade FROM Pedidos p LEFT JOIN DetalhesPedido dp ON p.Id = dp.PedidoId WHERE p.Status = @Status";
        public const string GetPedidosComItensPorStatusEFuncionario = "SELECT p.Id AS PedidoId, p.DataPedido, p.TotalPedido, p.Status, p.ClienteId, p.EnderecoId, p.MetodoPagamento, p.FuncionarioId, p.EntregadorId, dp.Id AS DetalhePedidoId, dp.ProdutoId, dp.NomeProduto, dp.PrecoUnitario, dp.Quantidade FROM Pedidos p LEFT JOIN DetalhesPedido dp ON p.Id = dp.PedidoId WHERE p.Status = @Status AND p.FuncionarioId = @FuncionarioId";
        public const string PedidoIniciarPreparo = "UPDATE Pedidos SET Status = @StatusEmPreparo, FuncionarioId = @FuncionarioId WHERE Id = @PedidoId AND Status = 0"; // 0 = Pendente
        public const string PedidoConcluirPreparo = "UPDATE Pedidos SET Status = @StatusPronto WHERE Id = @PedidoId AND FuncionarioId = @FuncionarioId AND Status = 1"; // 1 = Em Preparo
        public const string PedidoEntregar = "UPDATE Pedidos SET Status = @StatusEntregue, EntregadorId = @EntregadorId WHERE Id = @PedidoId";
        public const string PedidoEntregue = "UPDATE Pedidos SET Status = @StatusEntregue WHERE Id = @PedidoId AND EntregadorId = @EntregadorId";


        //Produto
        public const string ProdutoInsert = "INSERT INTO Produtos (Categoria, Descricao, Imagem, Nome, Preco) VALUES (@Categoria, @Descricao, @Imagem, @Nome, @Preco)";
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
        public const string GetProdutoMaisRentavel = @"SELECT DP.NomeProduto, SUM(DP.PrecoUnitario) AS ReceitaTotal FROM DetalhesPedidos AS DP GROUP BY DP.NomeProduto ORDER BY ReceitaTotal DESC";
        public const string GetReceitaPorProdutoId = @"SELECT DP.ProdutoId, SUM(DP.PrecoUnitario * DP.Quantidade) AS ValorVendidoTotal, SUM(DP.Quantidade) AS QuantidadeTotalVendida FROM DetalhesPedidos AS DP GROUP BY  DP.ProdutoId ORDER BY QuantidadeTotalVendida DESC;";
    }
}