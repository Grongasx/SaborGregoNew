using System.Data;
using SaborGregoNew.DTOs.Pedido;
using Microsoft.Data.Sqlite;
using SaborGregoNew.Models;
using SaborGregoNew.Enums;
using System.Data.Common;



namespace saborGregoNew.Repository
{
    public class PedidoSqliteRepository : IPedidoRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PedidoSqliteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task Create(PedidoDTO ModeloPedido)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão");
            using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.PedidoInsert;
                cmd.Parameters.Add(new SqliteParameter("@DataPedido", ModeloPedido.DataPedido));
                cmd.Parameters.Add(new SqliteParameter("@ValorTotal", ModeloPedido.ValorTotal));
                cmd.Parameters.Add(new SqliteParameter("@Status", ModeloPedido.Status));
                cmd.Parameters.Add(new SqliteParameter("@ClienteId", ModeloPedido.ClienteId));
                cmd.Parameters.Add(new SqliteParameter("@EnderecoId", ModeloPedido.EnderecoId));
                cmd.Parameters.Add(new SqliteParameter("@MetodoPagamento", ModeloPedido.MetodoPagamento));

                await cmd.ExecuteNonQueryAsync();
            }
            
        }

        public async Task UpdateStatusById(int id, PedidoDTO ModeloPedido)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão");
            using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.PedidoUpdateStatus;
                cmd.Parameters.Add(new SqliteParameter("@Id", id));
                cmd.Parameters.Add(new SqliteParameter("@Status", ModeloPedido.Status));
                await cmd.ExecuteNonQueryAsync();
            }
            
        }

        public async Task AddDetalhesAsync(IEnumerable<DetalhePedido> detalhes)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão");
            using (conn)
            {
                await conn.OpenAsync();

                using var transaction = conn.BeginTransaction();
                try
                {
                    foreach (var detalhe in detalhes)
                    {
                        using var cmd = conn.CreateCommand();
                        cmd.Transaction = transaction; // Associa o comando à transação

                        // Comando SQL de Inserção para DetalhePedido
                        // Ex: INSERT INTO DetalhesPedido (PedidoId, ProdutoId, NomeProduto, PrecoUnitario, Quantidade) 
                        //     VALUES (@PedidoId, @ProdutoId, @NomeProduto, @PrecoUnitario, @Quantidade)
                        cmd.CommandText = Queries.DetalhePedidoInsert;

                        // Parâmetros
                        cmd.Parameters.Add(new SqliteParameter("@PedidoId", detalhe.PedidoId));
                        cmd.Parameters.Add(new SqliteParameter("@ProdutoId", detalhe.ProdutoId));
                        cmd.Parameters.Add(new SqliteParameter("@NomeProduto", detalhe.NomeProduto));
                        cmd.Parameters.Add(new SqliteParameter("@PrecoUnitario", detalhe.PrecoUnitario));
                        cmd.Parameters.Add(new SqliteParameter("@Quantidade", detalhe.Quantidade));

                        cmd.ExecuteNonQuery();
                    }

                    // Confirma a transação se todos os INSERTs forem bem-sucedidos
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // Reverte a transação se qualquer INSERT falhar
                    transaction.Rollback();
                    throw;
                }
            }
            
            
        }
        public async Task<Pedido?> SelectByIdAsync(int id)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.PedidoSelectById;
                cmd.Parameters.Add(new SqliteParameter("@Id", id));

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var pedido = new Pedido
                    {
                        Id = reader.GetInt32(0),
                        ClienteId = reader.GetInt32(4),
                        DataPedido = reader.GetDateTime(1),
                        EnderecoId = reader.GetInt32(5),
                        EntregadorId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                        FuncionarioId = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7),
                        MetodoPagamento = (MetodoPagamento)reader.GetInt32(6),
                        Status = (StatusPedido)reader.GetInt32(3),
                        TotalPedido = reader.GetDecimal(2),
                        Itens = new List<DetalhePedido>()
                    };
                    return pedido;
                }
                return null;
            }
        }
    }
}
