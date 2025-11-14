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

        //pegar pedidos por status
        private Pedido MapPedidoFromReader(DbDataReader reader)
        {
            // Nota: A função Enum.Parse é case-sensitive. Garanta que o status no DB seja exato.
            return new Pedido
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                ClienteId = reader.GetInt32(reader.GetOrdinal("ClienteId")),
                DataPedido = reader.GetDateTime(reader.GetOrdinal("DataPedido")),
                EnderecoId = reader.GetInt32(reader.GetOrdinal("EnderecoId")),

                EntregadorId = reader.IsDBNull(reader.GetOrdinal("EntregadorId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("EntregadorId")),
                FuncionarioId = reader.IsDBNull(reader.GetOrdinal("FuncionarioId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("FuncionarioId")),

                MetodoPagamento = (MetodoPagamento)Enum.Parse(typeof(MetodoPagamento), reader.GetString(reader.GetOrdinal("MetodoPagamento"))),
                Status = (StatusPedido)Enum.Parse(typeof(StatusPedido), reader.GetString(reader.GetOrdinal("Status"))),
                TotalPedido = reader.GetDecimal(reader.GetOrdinal("TotalPedido"))
            };
        }

        // ----------------------------------------------------------------------

        public async Task<List<Pedido>> GetPedidosPublicosPorStatusAsync(StatusPedido status)
        {
            if (status != StatusPedido.Solicitado && status != StatusPedido.ProntoParaRetirada)
            {
                throw new ArgumentException("Status inválido para acesso público.");
            }

            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão com o banco de dados.");

            using (conn)
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = PedidoQuery.GetPedidosStatus;
                
                // Padronizado para STRING, alinhado com o mapeamento
                cmd.Parameters.Add(new SqliteParameter("@Status", status.ToString())); 

                using var reader = await cmd.ExecuteReaderAsync();

                var listaPedidos = new List<Pedido>();
                while (await reader.ReadAsync())
                {
                    listaPedidos.Add(MapPedidoFromReader(reader));
                }
                return listaPedidos;
            }
        }

        // ----------------------------------------------------------------------

        public async Task<List<Pedido>> GetPedidosFuncionarioPorStatusAsync(StatusPedido status, int usuarioId)
        {
            if (status != StatusPedido.EmPreparacao && status != StatusPedido.EmRotaDeEntrega)
            {
                throw new ArgumentException("Status inválido para acesso de funcionário.");
            }

            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão com o banco de dados.");

            using (conn)
            {
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();

                // 1. Define a Query com base no status/tipo de funcionário
                if (status == StatusPedido.EmPreparacao)
                {
                    cmd.CommandText = PedidoQuery.GetPedidoFuncionario;
                }
                else if (status == StatusPedido.EmRotaDeEntrega)
                {
                    cmd.CommandText = PedidoQuery.GetPedidosEntregador;
                }

                // 2. Seta os parâmetros
                // CORRIGIDO: Padronizado para STRING, alinhado com o mapeamento e o outro método
                cmd.Parameters.Add(new SqliteParameter("@Status", status.ToString())); 
                
                cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId));

                using var reader = await cmd.ExecuteReaderAsync();

                var listaPedidos = new List<Pedido>();
                while (await reader.ReadAsync())
                {
                    listaPedidos.Add(MapPedidoFromReader(reader));
                }

                return listaPedidos;
            }
        }

        //atualizar status do pedido
        public async Task UpdateStatusByIdAsync(int id, int funcionarioId, StatusPedido status)
        {
            // 1. Cria a conexão UMA ÚNICA vez no método principal
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão com o banco de dados.");
            
            // Gerencia a conexão em todo o método principal
            using (conn)
            {
                await conn.OpenAsync(); // Abre a conexão uma vez
                
                // NOVO: Verifica se o status exige atribuição de funcionário (Cozinheiro ou Entregador)
                if (status == StatusPedido.EmPreparacao || status == StatusPedido.EmRotaDeEntrega)
                {
                    if (funcionarioId <= 0)
                    {
                        // Mensagem de erro unificada, pois o ID é obrigatório para ambas as atribuições
                        throw new ArgumentException($"O ID do funcionário (cozinheiro ou entregador) deve ser fornecido para a transição para o status {status}.");
                    }

                    // Chama o método auxiliar, que decide qual coluna (FuncionarioId ou EntregadorId)
                    // atualizar com base no 'status' fornecido.
                    await UpdateAssignmentAsync(id, funcionarioId, conn, status);
                    return; // Sai da função, a conexão será fechada pelo 'using' acima
                }
                else // Lógica Padrão para Outros Status (ProntoParaRetirada, Entregue, Cancelado, etc.)
                {
                    // Nota: Esta seção lida com mudanças de status que não exigem a atribuição de um ID
                    // de funcionário ou entregador (pois já foram atribuídos ou não se aplicam).
                    var cmd = conn.CreateCommand();

                    cmd.CommandText = PedidoQuery.PedidoUpdateStatus;
                    
                    // Usando status.ToString(), assumindo que o campo Status no banco é TEXT.
                    cmd.Parameters.Add(new SqliteParameter("@Status", status.ToString())); 
                    cmd.Parameters.Add(new SqliteParameter("@Id", id));

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException($"Nenhum pedido encontrado com o ID {id} para atualizar o status.");
                    }
                }
            }
        }

        // ----------------------------------------------------------------------------------

        // Método Auxiliar Refatorado para trabalhar com uma conexão existente
        private async Task UpdateAssignmentAsync(int id, int userId, DbConnection conn, StatusPedido status)
        {
            // A coluna a ser atualizada e o status final dependem do que está sendo feito
            string columnToUpdate = "";
            string statusString = status.ToString();
            
            // Define qual coluna será atualizada com base no status
            if (status == StatusPedido.EmPreparacao)
            {
                // Se estiver em preparação, o userId é o ID do Cozinheiro
                columnToUpdate = "FuncionarioId";
            }
            else if (status == StatusPedido.EmRotaDeEntrega)
            {
                // Se estiver em rota de entrega, o userId é o ID do Entregador
                columnToUpdate = "EntregadorId";
            }
            else
            {
                // Este método só deve ser chamado para atribuições diretas de ID
                throw new ArgumentException($"O Status '{statusString}' não exige atribuição de funcionário (FuncionarioId/EntregadorId).");
            }
            
            // O método principal (UpdateStatusByIdAsync) é responsável por garantir que conn está aberto.
            var cmd = conn.CreateCommand();

            // 1. Define a Query SQL com o nome da coluna variável
            // Nota: É necessário usar interpolação de strings (ou métodos mais avançados) 
            // para o NOME da coluna, pois ele não pode ser um parâmetro SQL (@...).
            // Isso é seguro neste contexto, pois 'columnToUpdate' é validada.
            cmd.CommandText = $@"
                UPDATE Pedidos
                SET 
                    {columnToUpdate} = @UserId,
                    Status = @Status
                WHERE 
                    Id = @Id;
            ";

            // 2. Adiciona os parâmetros com segurança
            cmd.Parameters.Add(new SqliteParameter("@UserId", userId));
            cmd.Parameters.Add(new SqliteParameter("@Status", statusString)); 
            cmd.Parameters.Add(new SqliteParameter("@Id", id));

            // 3. Executa o comando
            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"Nenhum pedido encontrado com o ID {id} para atribuição de funcionário.");
            }
        }

        //==========================//
        //===Metodos para Usuario===//
        //==========================//
        //Pegar pedidos pendentes do usuario
        public async Task<List<Pedido>> GetPedidosPendentesAsync(int usuarioId)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
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
