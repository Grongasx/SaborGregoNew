using saborGregoNew.Repository;
using Microsoft.Data.Sqlite;
using SaborGregoNew.Models;
using SaborGregoNew.DTOs.Usuario;
using System.Data.Common;
using SaborGregoNew.Repository.Query;
using SaborGregoNew.Enumerate;

namespace SaborGregoNew.Repository
{
    public class EnderecoSqliteRepository : IEnderecoRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public EnderecoSqliteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // Método Auxiliar: Cria e abre a conexão, tratando falhas iniciais
        private async Task<DbConnection> GetOpenConnection()
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão com o banco de dados.");
            
            await conn.OpenAsync();
            return conn;
        }

        // Cria um novo endereço
        public async Task Create(EnderecoDTO ModeloEndereco, int usuarioId)
        {
            try
            {
                using var conn = await GetOpenConnection();
                using var cmd = conn.CreateCommand();
                
                cmd.CommandText = EnderecoQuery.EnderecoInsert;
                
                cmd.Parameters.Add(new SqliteParameter("@Apelido", ModeloEndereco.Apelido));
                cmd.Parameters.Add(new SqliteParameter("@Logradouro", ModeloEndereco.Logradouro));
                cmd.Parameters.Add(new SqliteParameter("@Numero", ModeloEndereco.Numero));
                cmd.Parameters.Add(new SqliteParameter("@Complemento", ModeloEndereco.Complemento));
                cmd.Parameters.Add(new SqliteParameter("@Bairro", ModeloEndereco.Bairro));
                cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId));
                
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                // Lança uma exceção mais específica para o repositório.
                throw new Exception("Falha ao criar endereço no banco de dados.", ex);
            }
        }

        // Seleciona todos os endereços de um usuário
        public async Task<List<Endereco>> SelectAllByUserIdAsync(int usuarioId)
        {
            try
            {
                using var conn = await GetOpenConnection();
                using var cmd = conn.CreateCommand();
                
                cmd.CommandText = EnderecoQuery.EnderecoSelectByUserId;
                cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId));
                
                using var reader = await cmd.ExecuteReaderAsync();
                
                var enderecos = new List<Endereco>();
                while (await reader.ReadAsync())
                {
                    enderecos.Add(MapEnderecoFromReader(reader));
                }
                return enderecos;
            }
            catch(Exception ex)
            {
                throw new Exception("Houve um problema ao selecionar os endereços no banco de dados.", ex);
            }
        }

        // Seleciona um endereço pelo seu ID
        public async Task<Endereco?> SelectByIdAsync(int id)
        {
            try
            {
                using var conn = await GetOpenConnection();
                using var cmd = conn.CreateCommand();
                
                cmd.CommandText = EnderecoQuery.EnderecoSelectById;
                cmd.Parameters.Add(new SqliteParameter("@Id", id));
                
                using var reader = await cmd.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    return MapEnderecoFromReader(reader);
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception("Houve um problema ao obter o endereço pelo ID.", ex);
            }
        }

        // Atualiza um endereço
        public async Task UpdateById(int Id, EnderecoDTO ModeloEndereco)
        {
            try
            {
                using var conn = await GetOpenConnection();
                using var cmd = conn.CreateCommand();
                
                cmd.CommandText = EnderecoQuery.EnderecoUpdate;
                
                cmd.Parameters.Add(new SqliteParameter("@Id", Id));
                cmd.Parameters.Add(new SqliteParameter("@Apelido", ModeloEndereco.Apelido));
                cmd.Parameters.Add(new SqliteParameter("@Logradouro", ModeloEndereco.Logradouro));
                cmd.Parameters.Add(new SqliteParameter("@Numero", ModeloEndereco.Numero));
                cmd.Parameters.Add(new SqliteParameter("@Complemento", ModeloEndereco.Complemento));
                cmd.Parameters.Add(new SqliteParameter("@Bairro", ModeloEndereco.Bairro));
                cmd.Parameters.Add(new SqliteParameter("@UsuarioId", ModeloEndereco.UsuarioId));

                await cmd.ExecuteNonQueryAsync();
            }
            catch(Exception ex)
            {
                throw new Exception("Erro ao atualizar o endereço no banco de dados.", ex);
            }
        }

        // Seleciona um endereço pelo seu ID e UsuarioId
        // Assinatura de retorno alterada para 'Endereco?' para indicar que pode ser nulo.
        public async Task<Endereco?> GetByIdAndUserIdAsync(int enderecoId, int usuarioId)
        {
            try
            {
                using var conn = await GetOpenConnection();
                using var cmd = conn.CreateCommand();
                
                cmd.CommandText = EnderecoQuery.EnderecoSelectByIdAndUserId;
                
                cmd.Parameters.Add(new SqliteParameter("@Id", enderecoId));
                cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId));
                
                using var reader = await cmd.ExecuteReaderAsync();
                
                if (await reader.ReadAsync())
                {
                    // Mapeamento manual dos dados da coluna para o objeto Endereco
                    return MapEnderecoFromReader(reader);
                }
                
                return null; // Retorna null se não encontrar
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter o endereço do usuário (ID e Usuário ID).", ex);
            }
        }

        // Metodo para "desativar" o Endereço (Soft Delete)
        public async Task DesativarAsync(int id)
        {
            // Corrigido: Usando CreateSqliteConnection
            try
            {
                using var conn = await GetOpenConnection();
                using var cmd = conn.CreateCommand();
                
                cmd.CommandText = EnderecoQuery.EnderecoDesativar;
                cmd.Parameters.Add(new SqliteParameter("@Id", id));
                // Corrigido: Passando Ativo.Inativo (assumindo que 0 é inativo)
                cmd.Parameters.Add(new SqliteParameter("@ativo", Ativo.Inativo)); 

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Algo deu errado ao desativar endereço no banco de dados.", ex);
            }
        }

        // Realmente deleta o endereço pelo id
        // Mantido o aviso de produção
        public async Task DeleteById(int id)
        {
             //===============================================================//
             //=============Não usar esse metodo em produção==================//
             //===============================================================//
            
            try
            {
                using var conn = await GetOpenConnection();
                using var cmd = conn.CreateCommand();
                
                cmd.CommandText = Queries.EnderecoDeleteById;
                cmd.Parameters.Add(new SqliteParameter("@Id", id));
                
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                 throw new Exception("Falha ao deletar endereço fisicamente.", ex);
            }
        }
        
        // Mapeador Privado para evitar repetição e garantir a ordem correta das colunas
        private Endereco MapEnderecoFromReader(DbDataReader reader)
        {
            return new Endereco
            {
                Id = reader.GetInt32(0),
                Apelido = reader.GetString(1),
                // ✅ Mapeamento corrigido conforme o Model:
                Logradouro = reader.GetString(2), // Índice 2
                Numero = reader.GetString(3),     // Índice 3
                Complemento = reader.GetString(4),// Índice 4
                Bairro = reader.GetString(5),     // Índice 5
                UsuarioId = reader.GetInt32(6),
                Ativo = reader.GetBoolean(7)      // ✅ NOVO: Mapeia o campo Ativo (Índice 7)
            };
        }
    }
}