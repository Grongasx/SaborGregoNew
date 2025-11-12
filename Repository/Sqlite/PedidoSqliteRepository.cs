using System.Data.Common;
using Microsoft.Data.Sqlite;
using SaborGregoNew.DTOs.Pedido;
using SaborGregoNew.Models;
using SaborGregoNew.Enums;
using SaborGregoNew.Repository.Query;
using saborGregoNew.Repository.Interfaces;



namespace saborGregoNew.Repository
{
    public class PedidoSqliteRepository : IPedidoRepository
    {
        //Conexão com o banco//
        private readonly IDbConnectionFactory _connectionFactory;
        public PedidoSqliteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        //===========================//
        //===Solicitação do Pedido===//
        //===========================//

        //uma transaction que cria o pedido, insere os dados do pedido e insere todos os itens um por um no pedido
        public async Task CriarPedidoCompletoAsync(PedidoDTO ModeloPedido, IEnumerable<DetalhePedido> detalhes)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão");

            int novoPedidoId;

            using (conn) //usando a conexão
            {
                await conn.OpenAsync(); //abrindo nova conexão
                using var transaction = conn.BeginTransaction(); //abrindo uma transaction para caso de errado, ele desfaça tudo

                try //caso de certo
                {
                    novoPedidoId = await CriarPedidoAsync(conn, transaction, ModeloPedido); //Cria o Pedido e Guarda o Id do pedido
                    foreach (var detalhe in detalhes)// para cada item no carrinho
                    {
                        detalhe.PedidoId = novoPedidoId; //pega o Id 
                    }

                    await AddDetalhesAsync(conn, transaction, detalhes); //coloca os itens do carrinho na tabela DetalhesProduto

                    await transaction.CommitAsync(); //confirma a transação

                }
                catch (Exception)
                {
                    transaction.Rollback(); //Desfaz tudo caso de errado
                    throw;
                }
            }
        }
        private async Task<int> CriarPedidoAsync(DbConnection conn, DbTransaction transaction, PedidoDTO ModeloPedido)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = PedidoQuery.PedidoInsert;

            cmd.Parameters.Add(new SqliteParameter("@ClienteId", ModeloPedido.ClienteId));
            cmd.Parameters.Add(new SqliteParameter("@DataPedido", ModeloPedido.DataPedido));
            cmd.Parameters.Add(new SqliteParameter("@EnderecoId", ModeloPedido.EnderecoId));
            cmd.Parameters.Add(new SqliteParameter("@MetodoPagamento", ModeloPedido.MetodoPagamento.ToString()));
            cmd.Parameters.Add(new SqliteParameter("@Status", ModeloPedido.Status.ToString()));
            cmd.Parameters.Add(new SqliteParameter("@ValorTotal", ModeloPedido.ValorTotal));

            await cmd.ExecuteNonQueryAsync();
            cmd.CommandText = "select last_insert_rowid();";
            long newId = (long)await cmd.ExecuteScalarAsync();

            return (int)newId;
        }
        private async Task AddDetalhesAsync(DbConnection conn, DbTransaction transaction, IEnumerable<DetalhePedido> detalhes)
        {
            foreach (var detalhe in detalhes)
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = PedidoQuery.DetalhePedidoInsert;

                // Parâmetros
                cmd.Parameters.Add(new SqliteParameter("@PedidoId", detalhe.PedidoId));
                cmd.Parameters.Add(new SqliteParameter("@ProdutoId", detalhe.ProdutoId));
                cmd.Parameters.Add(new SqliteParameter("@Imagem", detalhe.Imagem));
                cmd.Parameters.Add(new SqliteParameter("@NomeProduto", detalhe.NomeProduto));
                cmd.Parameters.Add(new SqliteParameter("@PrecoUnitario", detalhe.PrecoUnitario));
                cmd.Parameters.Add(new SqliteParameter("@Quantidade", detalhe.Quantidade));

                await cmd.ExecuteNonQueryAsync();
            }
        }


        //====================//
        //==Fluxos de Pedido==//
        //====================//

        //atualizar status conforme o fluxo de trabalho
        public async Task UpdateStatusByIdAsync(int id, StatusPedido status)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão");
            using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = PedidoQuery.PedidoUpdateStatus;
                cmd.Parameters.Add(new SqliteParameter("@Id", id));
                cmd.Parameters.Add(new SqliteParameter("@Status", status));
                await cmd.ExecuteNonQueryAsync();
            }
        }

        //Pegar Pedidos para o fluxo
        public async Task<List<Pedido>> GetPedidosFluxoTrabalhoAsync(StatusPedido status, int usuarioId)
        {

            //Verifica a conexão com o banco de dados
            if (_connectionFactory.CreateConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão");

            //abre a conexão com o banco de dados, Configura a query e executa
            using (conn)
            {
                await conn.OpenAsync();

                var cmd = conn.CreateCommand();

                //verificação de Status e Usuario
                if (status == StatusPedido.Solicitado || status == StatusPedido.ProntoParaRetirada) //Estados onde todos podem ver
                {
                    cmd.CommandText = PedidoQuery.GetPedidosStatus;
                }
                else if (status == StatusPedido.EmPreparacao || status == StatusPedido.EmRotaDeEntrega) //estado onde só quem pegou o pedido pode ver
                {
                    if (status == StatusPedido.EmPreparacao) //apenas para cozinheiro
                    {
                        cmd.CommandText = PedidoQuery.GetPedidoFuncionario;
                    }
                    else if (status == StatusPedido.EmRotaDeEntrega) //apenas para entregador
                    {
                        cmd.CommandText = PedidoQuery.GetPedidosEntregador;
                    }
                    cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId)); //seta o tipo de usuario
                }
                else
                {
                    return new List<Pedido>(); //caso algo esteja errado, retorna uma lista vazia
                }

                cmd.Parameters.Add(new SqliteParameter("@Status", (int)status)); //seta o status para pesquisa

                await cmd.ExecuteNonQueryAsync(); //executa a query
            }
            throw new InvalidCastException("a conexão não conseguiu realizar a operação");
        }

        //==========================//
        //===Metodos para Usuario===//
        //==========================//
        //Pegar pedidos pendentes do usuario
        public async Task<List<Pedido>> GetPedidosPendentesAsync(int usuarioId)
        {
            if (_connectionFactory.CreateConnection() is not DbConnection conn)
                throw new Exception("Falha ao obter conexão");

            try
            {
                using (conn)
                {
                    await conn.OpenAsync();

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = PedidoQuery.GetPedidosPendentes;

                    cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId));

                    using var reader = await cmd.ExecuteReaderAsync();
                    var pedidos = new List<Pedido>();
                    while (await reader.ReadAsync())
                    {
                        pedidos.Add(new Pedido
                        {
                            Id = reader.GetInt32(0),
                            ClienteId = reader.GetInt32(1),
                            DataPedido = reader.GetDateTime(2),
                            EnderecoId = reader.GetInt32(3),
                            MetodoPagamento = (MetodoPagamento)reader.GetInt32(4),
                            Status = (StatusPedido)reader.GetInt32(5),
                            TotalPedido = reader.GetDecimal(6)
                        });
                    }
                    return pedidos;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Problema ao listar pedidos pendentes", ex);
            }
        }
    }
}
