namespace SaborGregoNew.Repository.Query;
public static class PedidoQuery
{
    //Create do Pedido == Status - Solicitado
    public const string PedidoInsert = "INSERT INTO Pedidos (ClienteId, DataPedido, EnderecoId, MetodoPagamento, Status, TotalPedido) VALUES (@ClienteId, @DataPedido, @EnderecoId, @MetodoPagamento, @Status, @ValorTotal);";
    public const string DetalhePedidoInsert = "INSERT INTO DetalhePedido (PedidoId, ProdutoId, Imagem, NomeProduto, PrecoUnitario, Quantidade) VALUES (@PedidoId, @ProdutoId, @Imagem, @NomeProduto, @PrecoUnitario, @Quantidade)";
    //=================================//
    //===Fluxo de trabalho do Pedido===//
    //=================================//
    //Atualiza o Status do Pedido
    public const string PedidoUpdateStatus = "UPDATE Pedidos Set Status = @Status WHERE Id = @Id";
    //Atualiza o Status do Pedido e define o funcionario responsavel
    public const string PedidoUpdateStatusAndFuncionario = "UPDATE Pedidos Set Status = @Status, FuncionarioId = @UsuarioId WHERE Id = @Id";
    //Pegar Pedidos por Status
    public const string GetPedidosStatus = "SELECT * FROM Pedidos WHERE Status = @Status";
    //Pegar Pedidos por Status e Funcionario(cozinheiro)
    public const string GetPedidoFuncionario = "SELECT * FROM Pedidos WHERE Status = @Status AND FuncionarioId = @UsuarioId";
    //Pegar Pedidos por Status e Entregador
    public const string GetPedidosEntregador = "SELECT * FROM Pedidos WHERE Status = @Status AND EntregadorId = @UsuarioId";
    
    //pegar pedidos com status para acompanhamento
    public const string GetPedidosPendentes = "SELECT * FROM Pedidos WHERE Status NOT IN (0, 5) AND ClienteId = @UsuarioId";
    //Pegar todos os pedidos de um cliente (Histórico)
    public const string GetPedidosPorCliente = "SELECT * FROM Pedidos WHERE ClienteId = @UsuarioId ORDER BY DataPedido DESC";
    //Pegar detalhes do pedido
    public const string GetDetalhesByPedidoId = "SELECT * FROM DetalhePedido WHERE PedidoId = @PedidoId";
}