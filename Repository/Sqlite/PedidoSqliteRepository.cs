using System.Data.Common;
using Microsoft.Data.Sqlite;
using SaborGregoNew.DTOs.Pedido;
using SaborGregoNew.Models;
using SaborGregoNew.Enums;
using SaborGregoNew.Repository.Query;

namespace SaborGregoNew.Repository
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
            cmd.Parameters.Add(new SqliteParameter("@Status", (int)ModeloPedido.Status));
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
                cmd.Parameters.Add(new SqliteParameter("@Status", (int)status));
                await cmd.ExecuteNonQueryAsync();
            }
        }

        //Pegar Pedidos para o fluxo
        // public async Task<List<Pedido>> GetPedidosFluxoTrabalhoAsync(StatusPedido status, int usuarioId)
        // {
        //     //Verifica a conexão com o banco de dados
        //     if (_connectionFactory.CreateConnection() is not DbConnection conn)
        //         throw new InvalidOperationException("Falha ao obter conexão");

        //     //abre a conexão com o banco de dados, Configura a query e executa
        //     using (conn)
        //     {
        //         await conn.OpenAsync();
        //         var cmd = conn.CreateCommand();

        //         //verificação de Status e Usuario
        //         if (status == StatusPedido.Solicitado || status == StatusPedido.ProntoParaRetirada) //Estados onde todos podem ver
        //         {
        //             cmd.CommandText = PedidoQuery.GetPedidosStatus;
        //         }
        //         else if (status == StatusPedido.EmPreparacao || status == StatusPedido.EmRotaDeEntrega) //estado onde só quem pegou o pedido pode ver
        //         {
        //             if (status == StatusPedido.EmPreparacao) //apenas para cozinheiro
        //             {
        //                 cmd.CommandText = PedidoQuery.GetPedidoFuncionario;
        //             }
        //             else if (status == StatusPedido.EmRotaDeEntrega) //apenas para entregador
        //             {
        //                 cmd.CommandText = PedidoQuery.GetPedidosEntregador;
        //             }
        //             cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId)); //seta o tipo de usuario
        //         }
        //         else
        //         {
        //             return new List<Pedido>(); //caso algo esteja errado, retorna uma lista vazia
        //         }

        //         cmd.Parameters.Add(new SqliteParameter("@Status", (int)status)); //seta o status para pesquisa

        //         await cmd.ExecuteNonQueryAsync(); //executa a query
        //     }
        //     throw new InvalidCastException("a conexão não conseguiu realizar a operação");
        // }
        //Pegar Pedidos para o fluxo
// Pegar Pedidos para o fluxo (Cozinha/Entrega)
        public async Task<List<Pedido>> GetPedidosFluxoTrabalhoAsync(StatusPedido status, int usuarioId)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão");

            using (conn)
            {
                await conn.OpenAsync();
                
                var pedidos = new List<Pedido>();

                // 1. Busca os Cabeçalhos dos Pedidos
                using (var cmd = conn.CreateCommand())
                {
                    // LÓGICA CRÍTICA: Definir qual Query usar
                    if (status == StatusPedido.Solicitado || status == StatusPedido.ProntoParaRetirada)
                    {
                        // Solicitado = Todos veem (sem filtro de funcionário)
                        cmd.CommandText = PedidoQuery.GetPedidosStatus;
                    }
                    else if (status == StatusPedido.EmPreparacao || status == StatusPedido.EmRotaDeEntrega)
                    {
                        // Em Preparação = Apenas o cozinheiro responsável vê
                        if (status == StatusPedido.EmPreparacao)
                            cmd.CommandText = PedidoQuery.GetPedidoFuncionario;
                        else 
                            cmd.CommandText = PedidoQuery.GetPedidosEntregador;

                        // Só adicionamos este parâmetro se a query filtrar por funcionário
                        cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId));
                    }
                    else
                    {
                        return new List<Pedido>();
                    }

                    cmd.Parameters.Add(new SqliteParameter("@Status", (int)status));

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var pedido = new Pedido
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ClienteId = reader.GetInt32(reader.GetOrdinal("ClienteId")),
                                DataPedido = reader.GetDateTime(reader.GetOrdinal("DataPedido")),
                                EnderecoId = reader.GetInt32(reader.GetOrdinal("EnderecoId")),
                                MetodoPagamento = (MetodoPagamento)reader.GetInt32(reader.GetOrdinal("MetodoPagamento")),
                                Status = (StatusPedido)reader.GetInt32(reader.GetOrdinal("Status")),
                                TotalPedido = reader.GetDecimal(reader.GetOrdinal("TotalPedido"))
                            };
                            
                            // Verifica se as colunas opcionais não são nulas antes de ler
                            int ordFunc = reader.GetOrdinal("FuncionarioId");
                            if (!reader.IsDBNull(ordFunc)) pedido.FuncionarioId = reader.GetInt32(ordFunc);

                            int ordEnt = reader.GetOrdinal("EntregadorId");
                            if (!reader.IsDBNull(ordEnt)) pedido.EntregadorId = reader.GetInt32(ordEnt);

                            pedidos.Add(pedido);
                        }
                    }
                }
                foreach (var pedido in pedidos)
                {
                    using (var cmdItens = conn.CreateCommand())
                    {
                        cmdItens.CommandText = PedidoQuery.GetDetalhesByPedidoId;
                        cmdItens.Parameters.Add(new SqliteParameter("@PedidoId", pedido.Id));

                        using (var readerItens = await cmdItens.ExecuteReaderAsync())
                        {
                            while (await readerItens.ReadAsync())
                            {
                                pedido.Itens.Add(new DetalhePedido
                                {
                                    PedidoId = readerItens.GetInt32(readerItens.GetOrdinal("PedidoId")),
                                    ProdutoId = readerItens.GetInt32(readerItens.GetOrdinal("ProdutoId")),
                                    // Tratamento de nulo para imagem
                                    Imagem = readerItens.IsDBNull(readerItens.GetOrdinal("Imagem")) ? null : readerItens.GetString(readerItens.GetOrdinal("Imagem")),
                                    NomeProduto = readerItens.GetString(readerItens.GetOrdinal("NomeProduto")),
                                    PrecoUnitario = readerItens.GetDecimal(readerItens.GetOrdinal("PrecoUnitario")),
                                    Quantidade = readerItens.GetInt32(readerItens.GetOrdinal("Quantidade"))
                                });
                            }
                        }
                    }
                }
                return pedidos;
            }
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
                throw new Exception("Erro ao buscar pedidos pendentes", ex);
            }
        }
        
        public async Task<List<Pedido>> GetPedidosPorClienteAsync(int usuarioId)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new Exception("Falha ao obter conexão");
            try
            {
                using (conn)
                {
                    await conn.OpenAsync();
                    
                    // 1. Busca os Pedidos (Cabeçalho)
                    var pedidos = new List<Pedido>();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = PedidoQuery.GetPedidosPorCliente;
                        cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId));
                        
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var pedido = new Pedido
                                {
                                    // CORREÇÃO: Usamos GetOrdinal para achar a coluna pelo nome correto
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    ClienteId = reader.GetInt32(reader.GetOrdinal("ClienteId")),
                                    DataPedido = reader.GetDateTime(reader.GetOrdinal("DataPedido")),
                                    EnderecoId = reader.GetInt32(reader.GetOrdinal("EnderecoId")),
                                    MetodoPagamento = (MetodoPagamento)reader.GetInt32(reader.GetOrdinal("MetodoPagamento")),
                                    Status = (StatusPedido)reader.GetInt32(reader.GetOrdinal("Status")),
                                    TotalPedido = reader.GetDecimal(reader.GetOrdinal("TotalPedido"))
                                };

                                // Tratamento seguro para colunas que podem ser NULL (Funcionário/Entregador)
                                int ordFunc = reader.GetOrdinal("FuncionarioId");
                                if (!reader.IsDBNull(ordFunc)) 
                                    pedido.FuncionarioId = reader.GetInt32(ordFunc);

                                int ordEnt = reader.GetOrdinal("EntregadorId");
                                if (!reader.IsDBNull(ordEnt)) 
                                    pedido.EntregadorId = reader.GetInt32(ordEnt);

                                pedidos.Add(pedido);
                            }
                        }
                    } // O reader fecha aqui, liberando a conexão para o próximo comando

                    // 2. Popula os Itens de cada pedido
                    foreach (var pedido in pedidos)
                    {
                        using (var cmdItens = conn.CreateCommand())
                        {
                            cmdItens.CommandText = PedidoQuery.GetDetalhesByPedidoId;
                            cmdItens.Parameters.Add(new SqliteParameter("@PedidoId", pedido.Id));
                            
                            using (var readerItens = await cmdItens.ExecuteReaderAsync())
                            {
                                while (await readerItens.ReadAsync())
                                {
                                    pedido.Itens.Add(new DetalhePedido
                                    {
                                        PedidoId = readerItens.GetInt32(readerItens.GetOrdinal("PedidoId")),
                                        ProdutoId = readerItens.GetInt32(readerItens.GetOrdinal("ProdutoId")),
                                        // Verifica se imagem é nula antes de ler
                                        Imagem = readerItens.IsDBNull(readerItens.GetOrdinal("Imagem")) ? null : readerItens.GetString(readerItens.GetOrdinal("Imagem")),
                                        NomeProduto = readerItens.GetString(readerItens.GetOrdinal("NomeProduto")),
                                        PrecoUnitario = readerItens.GetDecimal(readerItens.GetOrdinal("PrecoUnitario")),
                                        Quantidade = readerItens.GetInt32(readerItens.GetOrdinal("Quantidade"))
                                    });
                                }
                            }
                        }
                    }
                    return pedidos;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Problema ao listar histórico de pedidos", ex);
            }
        }
        
        public async Task UpdateStatusAndAssignAsync(int id, StatusPedido status, int usuarioId)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão");
            using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = PedidoQuery.PedidoUpdateStatusAndFuncionario;
                cmd.Parameters.Add(new SqliteParameter("@Id", id));
                cmd.Parameters.Add(new SqliteParameter("@Status", (int)status));
                cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId));
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
