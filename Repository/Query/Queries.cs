namespace SaborGregoNew.Repository;

public class Queries
{
    // --- PEDIDOS ---
    // SQL Server usa SCOPE_IDENTITY() para pegar o ID inserido, não last_insert_rowid()
    // A query de Insert não muda muito, exceto pelo ID que trataremos no Repositório.
    public const string GetPedidosComItensPorStatus = "SELECT p.Id AS PedidoId, p.DataPedido, p.TotalPedido, p.Status, p.ClienteId, p.EnderecoId, p.MetodoPagamento, p.FuncionarioId, p.EntregadorId, dp.Id AS DetalhePedidoId, dp.ProdutoId, dp.NomeProduto, dp.PrecoUnitario, dp.Quantidade FROM Pedidos p LEFT JOIN DetalhePedido dp ON p.Id = dp.PedidoId WHERE p.Status = @Status";
    public const string GetPedidosComItensPorStatusEFuncionario = "SELECT p.Id AS PedidoId, p.DataPedido, p.TotalPedido, p.Status, p.ClienteId, p.EnderecoId, p.MetodoPagamento, p.FuncionarioId, p.EntregadorId, dp.Id AS DetalhePedidoId, dp.ProdutoId, dp.NomeProduto, dp.PrecoUnitario, dp.Quantidade FROM Pedidos p LEFT JOIN DetalhePedido dp ON p.Id = dp.PedidoId WHERE p.Status = @Status AND p.FuncionarioId = @FuncionarioId";
    
    // --- PRODUTOS ---
    // 'true'/'false' no SQL Server são BIT (0 ou 1), igual ao SQLite, então a lógica mantém-se.
    public const string ProdutoInsert = "INSERT INTO Produtos (Categoria, Descricao, Imagem, Nome, Preco, Ativo) VALUES (@Categoria, @Descricao, @Imagem, @Nome, @Preco, 1)";
    public const string ProdutoSelectAll = "SELECT Id, Categoria, Descricao, Imagem, Nome, Preco, Ativo FROM Produtos";
    public const string ProdutoSelectById = "SELECT Id, Categoria, Descricao, Imagem, Nome, Preco, Ativo FROM Produtos WHERE Id = @Id";
    public const string ProdutoUpdate = "UPDATE Produtos SET Nome = @Nome, Descricao = @Descricao, Preco = @Preco, Categoria = @Categoria, Imagem = @Imagem WHERE Id = @Id";
    public const string ProdutoDeleteById = "DELETE FROM Produtos WHERE Id = @Id";
    public const string ProdutoDesativar = "UPDATE Produtos SET Ativo = 0 WHERE Id = @Id";
    public const string ProdutoAtivar = "UPDATE Produtos SET Ativo = 1 WHERE Id = @Id";

    // --- USUÁRIOS ---
    public const string UsuarioListagem = "SELECT Id, Nome, Telefone, Email, Senha, Role FROM Usuarios";
    public const string UsuarioById = "SELECT Id, Nome, Telefone, Email, Senha, Role FROM Usuarios WHERE Id = @Id";
    public const string UsuarioUpdateById = "UPDATE Usuarios SET Nome = @Nome, Telefone = @Telefone, Email = @Email, Senha = @Senha, Role = @Role WHERE Id = @Id";
    public const string UsuarioDeleteById = "DELETE FROM Usuarios WHERE Id = @Id";

    // --- DASHBOARD (TRADUÇÃO PESADA) ---
    
    // SQLite: DATE('now') -> SQL Server: CAST(GETDATE() AS DATE)
    public const string DashboardVendasHoje = @"
        SELECT 
            COALESCE(SUM(TotalPedido), 0) AS VendasHoje,
            COUNT(Id) AS PedidosHoje
        FROM Pedidos
        WHERE CAST(DataPedido AS DATE) = CAST(GETDATE() AS DATE);";

    // SQLite: STRFTIME -> SQL Server: MONTH() e YEAR()
    public const string DashboardVendasMes = @"
        SELECT 
            COALESCE(SUM(TotalPedido), 0) AS VendasMes,
            COUNT(Id) AS PedidosMes
        FROM Pedidos
        WHERE MONTH(DataPedido) = MONTH(GETDATE()) AND YEAR(DataPedido) = YEAR(GETDATE());";

    // SQLite: LIMIT 1 no fim -> SQL Server: TOP 1 logo após SELECT
    public const string DashboardProdutoMaisVendidoHoje = @"
        SELECT TOP 1
            p.Nome, 
            SUM(dp.Quantidade) AS QtdTotal
        FROM DetalhePedido AS dp
        JOIN Produtos AS p ON dp.ProdutoId = p.Id
        JOIN Pedidos AS pe ON dp.PedidoId = pe.Id
        WHERE CAST(pe.DataPedido AS DATE) = CAST(GETDATE() AS DATE)
        GROUP BY p.Nome
        ORDER BY QtdTotal DESC;";

    public const string DashboardProdutoMaisVendidoMes = @"
        SELECT TOP 1
            p.Nome, 
            SUM(dp.Quantidade) AS QtdTotal
        FROM DetalhePedido AS dp
        JOIN Produtos AS p ON dp.ProdutoId = p.Id
        JOIN Pedidos AS pe ON dp.PedidoId = pe.Id
        WHERE MONTH(pe.DataPedido) = MONTH(GETDATE()) AND YEAR(pe.DataPedido) = YEAR(GETDATE())
        GROUP BY p.Nome
        ORDER BY QtdTotal DESC;";

    // SQLite: STRFTIME('%d/%m') -> SQL Server: FORMAT(Data, 'dd/MM')
    public const string DashboardVendasDiarias = @"
        SELECT 
            FORMAT(DataPedido, 'dd/MM') AS Dia, 
            SUM(TotalPedido) AS Total
        FROM Pedidos
        WHERE MONTH(DataPedido) = MONTH(GETDATE()) AND YEAR(DataPedido) = YEAR(GETDATE())
        GROUP BY CAST(DataPedido AS DATE), DataPedido
        ORDER BY CAST(DataPedido AS DATE);";

    // SQLite: LIMIT 10 -> SQL Server: TOP 10
    public const string DashboardVendasPorProduto = @"
        SELECT TOP 10
            dp.NomeProduto, 
            SUM(dp.PrecoUnitario * dp.Quantidade) AS Total
        FROM DetalhePedido AS dp
        JOIN Pedidos AS pe ON dp.PedidoId = pe.Id
        WHERE MONTH(pe.DataPedido) = MONTH(GETDATE()) AND YEAR(pe.DataPedido) = YEAR(GETDATE())
        GROUP BY dp.NomeProduto
        ORDER BY Total DESC;";

    // --- ENDEREÇOS (Mantidos do nosso ajuste anterior) ---
    public const string EnderecoInsert = "INSERT INTO Enderecos (Apelido, Logradouro, Numero, Complemento, Bairro, UsuarioId, Ativo) VALUES (@Apelido, @Logradouro, @Numero, @Complemento, @Bairro, @UsuarioId, 1)";
    public const string EnderecoUpdate = "UPDATE Enderecos SET Apelido = @Apelido, Logradouro = @Logradouro, Numero = @Numero, Complemento = @Complemento, Bairro = @Bairro WHERE Id = @Id";
    public const string EnderecoSelectByUserId = "SELECT * FROM Enderecos WHERE UsuarioId = @UsuarioId AND Ativo = 1";
    public const string EnderecoSelectById = "SELECT * FROM Enderecos WHERE Id = @Id AND Ativo = 1";
    public const string EnderecoDeleteById = "DELETE FROM Enderecos WHERE Id = @Id"; 
    public const string EnderecoSelectByIdAndUserId = "SELECT * FROM Enderecos WHERE Id = @Id AND UsuarioId = @UsuarioId AND Ativo = 1";
    public const string EnderecoDesativar = "UPDATE Enderecos SET Ativo = 0 WHERE Id = @Id";
}