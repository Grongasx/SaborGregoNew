using System.Data;
namespace SaborGregoNew.Repository;

public class Queries
{
        public const string GetPedidosComItensPorStatus = "SELECT p.Id AS PedidoId, p.DataPedido, p.TotalPedido, p.Status, p.ClienteId, p.EnderecoId, p.MetodoPagamento, p.FuncionarioId, p.EntregadorId, dp.Id AS DetalhePedidoId, dp.ProdutoId, dp.NomeProduto, dp.PrecoUnitario, dp.Quantidade FROM Pedidos p LEFT JOIN DetalhePedido dp ON p.Id = dp.PedidoId WHERE p.Status = @Status";
        public const string GetPedidosComItensPorStatusEFuncionario = "SELECT p.Id AS PedidoId, p.DataPedido, p.TotalPedido, p.Status, p.ClienteId, p.EnderecoId, p.MetodoPagamento, p.FuncionarioId, p.EntregadorId, dp.Id AS DetalhePedidoId, dp.ProdutoId, dp.NomeProduto, dp.PrecoUnitario, dp.Quantidade FROM Pedidos p LEFT JOIN DetalhePedido dp ON p.Id = dp.PedidoId WHERE p.Status = @Status AND p.FuncionarioId = @FuncionarioId";
        public const string PedidoIniciarPreparo = "UPDATE Pedidos SET Status = @StatusEmPreparo, FuncionarioId = @FuncionarioId WHERE Id = @PedidoId AND Status = 0"; // 0 = Pendente
        public const string PedidoConcluirPreparo = "UPDATE Pedidos SET Status = @StatusPronto WHERE Id = @PedidoId AND FuncionarioId = @FuncionarioId AND Status = 1"; // 1 = Em Preparo

        //Produto
        public const string ProdutoInsert = "INSERT INTO Produtos (Categoria, Descricao, Imagem, Nome, Preco, Ativo) VALUES (@Categoria, @Descricao, @Imagem, @Nome, @Preco, 1)";
        public const string ProdutoSelectAll = "SELECT Id, Categoria, Descricao, Imagem, Nome, Preco, Ativo FROM Produtos";
        public const string ProdutoSelectById = "SELECT Id, Categoria, Descricao, Imagem, Nome, Preco, Ativo FROM Produtos WHERE Id = @Id";
        public const string ProdutoUpdate = "UPDATE Produtos SET Nome = @Nome, Descricao = @Descricao, Preco = @Preco, Categoria = @Categoria, Imagem = @Imagem WHERE Id = @Id";
        public const string ProdutoDeleteById = "DELETE FROM Produtos WHERE Id = @Id";
        public const string ProdutoDesativar = "UPDATE Produtos SET Ativo = 0 WHERE Id = @Id";
        public const string ProdutoAtivar = "UPDATE Produtos SET Ativo = 1 WHERE Id = @Id";

        //Usuario
        public const string UsuarioListagem = "SELECT Id, Nome, Telefone, Email, Senha, Role FROM Usuarios";
        public const string UsuarioById = "SELECT Id, Nome, Telefone, Email, Senha, Role FROM Usuarios WHERE Id = @Id";
        public const string UsuarioUpdateById = "UPDATE Usuarios SET Nome = @Nome, Telefone = @Telefone, Email = @Email, Senha = @Senha, Role = @Role WHERE Id = @Id";
        public const string UsuarioDeleteById = "DELETE FROM Usuarios WHERE Id = @Id";


        public const string DashboardVendasHoje = @"
            SELECT 
                COALESCE(SUM(TotalPedido), 0) AS VendasHoje,
                COUNT(Id) AS PedidosHoje
            FROM Pedidos
            WHERE DATE(DataPedido) = DATE('now', 'localtime');";

        public const string DashboardVendasMes = @"
            SELECT 
                COALESCE(SUM(TotalPedido), 0) AS VendasMes,
                COUNT(Id) AS PedidosMes
            FROM Pedidos
            WHERE STRFTIME('%Y-%m', DataPedido) = STRFTIME('%Y-%m', 'now', 'localtime');";

        public const string DashboardProdutoMaisVendidoHoje = @"
            SELECT 
                p.Nome, 
                SUM(dp.Quantidade) AS QtdTotal
            FROM DetalhePedido AS dp
            JOIN Produtos AS p ON dp.ProdutoId = p.Id
            JOIN Pedidos AS pe ON dp.PedidoId = pe.Id
            WHERE DATE(pe.DataPedido) = DATE('now', 'localtime')
            GROUP BY p.Nome
            ORDER BY QtdTotal DESC
            LIMIT 1;";

        public const string DashboardProdutoMaisVendidoMes = @"
            SELECT 
                p.Nome, 
                SUM(dp.Quantidade) AS QtdTotal
            FROM DetalhePedido AS dp
            JOIN Produtos AS p ON dp.ProdutoId = p.Id
            JOIN Pedidos AS pe ON dp.PedidoId = pe.Id
            WHERE STRFTIME('%Y-%m', pe.DataPedido) = STRFTIME('%Y-%m', 'now', 'localtime')
            GROUP BY p.Nome
            ORDER BY QtdTotal DESC
            LIMIT 1;";

        public const string DashboardVendasDiarias = @"
            SELECT 
                STRFTIME('%d/%m', DataPedido) AS Dia, 
                SUM(TotalPedido) AS Total
            FROM Pedidos
            WHERE STRFTIME('%Y-%m', DataPedido) = STRFTIME('%Y-%m', 'now', 'localtime')
            GROUP BY DATE(DataPedido)
            ORDER BY DATE(DataPedido);";

        public const string DashboardVendasPorProduto = @"
            SELECT 
                dp.NomeProduto, 
                SUM(dp.PrecoUnitario * dp.Quantidade) AS Total
            FROM DetalhePedido AS dp
            JOIN Pedidos AS pe ON dp.PedidoId = pe.Id
            WHERE STRFTIME('%Y-%m', pe.DataPedido) = STRFTIME('%Y-%m', 'now', 'localtime')
            GROUP BY dp.NomeProduto
            ORDER BY Total DESC
            LIMIT 10;";



        public const string GetVendasQuantidadePorCategoria = @" SELECT P.Categoria, SUM(DP.Quantidade) AS QuantidadeTotal FROM DetalhePedido AS DP JOIN Produtos AS P ON P.Id = DP.ProdutoId GROUP BY P.Categoria;";
        public const string GetVendasReceitaPorCategoria = @" SELECT P.Categoria, SUM(DP.PrecoUnitario) AS ReceitaTotal FROM DetalhePedido AS DP JOIN Produtos AS P ON P.Id = DP.ProdutoId GROUP BY P.Categoria;";
        public const string GetProdutoMaisRentavel = @"SELECT DP.NomeProduto, SUM(DP.PrecoUnitario) AS ReceitaTotal FROM DetalhePedido AS DP GROUP BY DP.NomeProduto ORDER BY ReceitaTotal DESC";
        public const string GetReceitaPorProdutoId = @"SELECT DP.ProdutoId, SUM(DP.PrecoUnitario * DP.Quantidade) AS ValorVendidoTotal, SUM(DP.Quantidade) AS QuantidadeTotalVendida FROM DetalhePedido AS DP GROUP BY  DP.ProdutoId ORDER BY QuantidadeTotalVendida DESC;";
        
        // Endereço
        public const string EnderecoInsert = "INSERT INTO Enderecos (Apelido, Logradouro, Numero, Complemento, Bairro, UsuarioId, Ativo) VALUES (@Apelido, @Logradouro, @Numero, @Complemento, @Bairro, @UsuarioId, 1)";
        public const string EnderecoUpdate = "UPDATE Enderecos SET Apelido = @Apelido, Logradouro = @Logradouro, Numero = @Numero, Complemento = @Complemento, Bairro = @Bairro WHERE Id = @Id";
        public const string EnderecoSelectByUserId = "SELECT * FROM Enderecos WHERE UsuarioId = @UsuarioId AND Ativo = 1";
        public const string EnderecoSelectById = "SELECT * FROM Enderecos WHERE Id = @Id AND Ativo = 1";
        public const string EnderecoSelectByIdAndUserId = "SELECT * FROM Enderecos WHERE Id = @Id AND UsuarioId = @UsuarioId AND Ativo = 1";
        public const string EnderecoDeleteById = "DELETE FROM Enderecos WHERE Id = @Id";
        public const string EnderecoDesativar = "UPDATE Enderecos SET Ativo = 0 WHERE Id = @Id";
}