// using System.Data;
// using SaborGregoNew.DTOs.Pedido;
// using Microsoft.Data.SqlClient;
// using System.Threading.Tasks;
// using SaborGregoNew.Models;
// using SaborGregoNew.Enums;


// namespace saborGregoNew.Repository
// {
//     public class PedidoDatabaseRepository : IPedidoRepository
//     {
//         private readonly IDbConnection _connection;
//         public PedidoDatabaseRepository(IDbConnectionFactory connection)
//         {
//             _connection = connection.CreateConnection();
//         }
//         public async Task Create(PedidoDTO ModeloPedido)
//         {
//             using var conn = _connection;
//             conn.Open();
//             using var cmd = conn.CreateCommand();
//             cmd.CommandText = Queries.PedidoInsert;
//             cmd.Parameters.Add(new SqlParameter("@DataPedido", ModeloPedido.DataPedido));
//             cmd.Parameters.Add(new SqlParameter("@ValorTotal", ModeloPedido.ValorTotal));
//             cmd.Parameters.Add(new SqlParameter("@Status", ModeloPedido.Status));
//             cmd.Parameters.Add(new SqlParameter("@ClienteId", ModeloPedido.ClienteId));
//             cmd.Parameters.Add(new SqlParameter("@EnderecoId", ModeloPedido.EnderecoId));
//             cmd.Parameters.Add(new SqlParameter("@MetodoPagamento", ModeloPedido.MetodoPagamento));

//             cmd.ExecuteNonQuery();
//             conn.Close();
//         }

//         public async Task UpdateStatusById(int id, PedidoDTO ModeloPedido)
//         {
//             using var conn = _connection;
//             conn.Open();
//             using var cmd = conn.CreateCommand();
//             cmd.CommandText = Queries.PedidoUpdateStatus;
//             cmd.Parameters.Add(new SqlParameter("@Id", id));
//             cmd.Parameters.Add(new SqlParameter("@Status", ModeloPedido.Status));
//             cmd.ExecuteNonQuery();
//             conn.Close();
//         }

//         public async Task AddDetalhesAsync(IEnumerable<DetalhePedido> detalhes)
//         {
//             using var conn = _connection;
//             if (conn == null) throw new InvalidOperationException("Falha ao criar DbConnection.");

//             conn.Open();

//             // Inicia a transação para garantir que todos os detalhes sejam salvos
//             using var transaction = conn.BeginTransaction();

//             try
//             {
//                 foreach (var detalhe in detalhes)
//                 {
//                     using var cmd = conn.CreateCommand();
//                     cmd.Transaction = transaction; // Associa o comando à transação

//                     // Comando SQL de Inserção para DetalhePedido
//                     // Ex: INSERT INTO DetalhesPedido (PedidoId, ProdutoId, NomeProduto, PrecoUnitario, Quantidade) 
//                     //     VALUES (@PedidoId, @ProdutoId, @NomeProduto, @PrecoUnitario, @Quantidade)
//                     cmd.CommandText = Queries.DetalhePedidoInsert;

//                     // Parâmetros
//                     cmd.Parameters.Add(new SqlParameter("@PedidoId", detalhe.PedidoId));
//                     cmd.Parameters.Add(new SqlParameter("@ProdutoId", detalhe.ProdutoId));
//                     cmd.Parameters.Add(new SqlParameter("@NomeProduto", detalhe.NomeProduto));
//                     cmd.Parameters.Add(new SqlParameter("@PrecoUnitario", detalhe.PrecoUnitario));
//                     cmd.Parameters.Add(new SqlParameter("@Quantidade", detalhe.Quantidade));

//                     cmd.ExecuteNonQuery();
//                 }

//                 // Confirma a transação se todos os INSERTs forem bem-sucedidos
//                 transaction.Commit();
//             }
//             catch (Exception)
//             {
//                 // Reverte a transação se qualquer INSERT falhar
//                 transaction.Rollback();
//                 throw;
//             }
//         }
//         public async Task<Pedido?> SelectByIdAsync(int id)
//         {
//             using var conn = _connection;
//             conn.Open();

//             using var cmd = conn.CreateCommand();
//             cmd.CommandText = Queries.PedidoSelectById;
//             cmd.Parameters.Add(new SqlParameter("@Id", id));

//             using var reader = cmd.ExecuteReader();
//             if (reader.Read())
//             {
//                 var pedido = new Pedido
//                 {
//                     Id = reader.GetInt32(0),
//                     DataPedido = reader.GetDateTime(1),
//                     TotalPedido = reader.GetDecimal(2),
//                     Status = (StatusPedido)reader.GetInt32(3),
//                     ClienteId = reader.GetInt32(4),
//                     EnderecoId = reader.GetInt32(5),
//                     MetodoPagamento = (MetodoPagamento)reader.GetInt32(6),
//                     FuncionarioId = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7),
//                     EntregadorId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
//                     Itens = new List<DetalhePedido>()
//                 };
//                 return pedido;
//             }
//             return null;
//         }
//         public async Task<List<Pedido>> GetPedidosComItensPorStatusAsync(StatusPedido status)
//         {
//         }
//         public async Task<List<Pedido>> GetPedidosComItensPorStatusEFuncionarioAsync(StatusPedido status, int funcionarioId)
//         {
//         }
//         public async Task<bool> IniciarPreparoAsync(int pedidoId, int funcionarioId)
//         {
//         }
//         public async Task<bool> ConcluirPreparoAsync(int pedidoId, int funcionarioId)
//         {
//         }
//     }
// }
