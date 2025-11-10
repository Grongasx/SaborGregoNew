using saborGregoNew.Repository;
using Microsoft.Data.Sqlite;
using SaborGregoNew.Models;
using System.Data;
using SaborGregoNew.DTOs.Usuario;
using System.Data.Common;

namespace SaborGregoNew.Repository
{
    public class EnderecoSqliteRepository : IEnderecoRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public EnderecoSqliteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task Create(EnderecoDTO ModeloEndereco, int usuarioId)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.EnderecoInsert;
                cmd.Parameters.Add(new SqliteParameter("@Apelido", ModeloEndereco.Apelido));
                cmd.Parameters.Add(new SqliteParameter("@Logradouro", ModeloEndereco.Logradouro));
                cmd.Parameters.Add(new SqliteParameter("@Numero", ModeloEndereco.Numero));
                cmd.Parameters.Add(new SqliteParameter("@Complemento", ModeloEndereco.Complemento));
                cmd.Parameters.Add(new SqliteParameter("@Bairro", ModeloEndereco.Bairro));
                cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId));

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<List<Endereco>> SelectAllByUserIdAsync(int usuarioId)
        {
             if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.EnderecoSelectByUserId;
                cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId));
                using var reader = await cmd.ExecuteReaderAsync();
                var enderecos = new List<Endereco>();
                while (await reader.ReadAsync())
                {
                    enderecos.Add(new Endereco
                    {
                        Id = reader.GetInt32(0),
                        Apelido = reader.GetString(1),
                        Bairro = reader.GetString(2),
                        Complemento = reader.GetString(3),
                        Logradouro = reader.GetString(4),
                        Numero = reader.GetString(5),
                        UsuarioId = reader.GetInt32(6)
                    });

                }
                return enderecos;
            }
        }
        public async Task<Endereco?> SelectByIdAsync(int id)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.EnderecoSelectById;
                cmd.Parameters.Add(new SqliteParameter("@Id", id));
                using var reader = await cmd.ExecuteReaderAsync();
                var endereco = new Endereco();
                if (await reader.ReadAsync())
                {
                    endereco = new Endereco
                    {
                        Id = reader.GetInt32(0),
                        Apelido = reader.GetString(1),
                        Bairro = reader.GetString(2),
                        Complemento = reader.GetString(3),
                        Logradouro = reader.GetString(4),
                        Numero = reader.GetString(5),
                        UsuarioId = reader.GetInt32(6)
                    };
                    return endereco;
                }
                return null;
            }
        }
        public async Task<Endereco?> SelectByUserIdAsync(int usuarioId)
        {
             if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.EnderecoSelectByUserId;
                cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId));
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Endereco
                    {
                        Id = reader.GetInt32(0),
                        Apelido = reader.GetString(1),
                        Bairro = reader.GetString(2),
                        Complemento = reader.GetString(3),
                        Logradouro = reader.GetString(4),
                        Numero = reader.GetString(5),
                        UsuarioId = reader.GetInt32(6)
                    };
                }
                return null;
            }
        }
        public async Task UpdateById(int Id, EnderecoDTO ModeloEndereco)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.EnderecoUpdate;
                cmd.Parameters.Add(new SqliteParameter("@Id", Id));
                cmd.Parameters.Add(new SqliteParameter("@Apelido", ModeloEndereco.Apelido));
                cmd.Parameters.Add(new SqliteParameter("@Logradouro", ModeloEndereco.Logradouro));
                cmd.Parameters.Add(new SqliteParameter("@Numero", ModeloEndereco.Numero));
                cmd.Parameters.Add(new SqliteParameter("@Complemento", ModeloEndereco.Complemento));
                cmd.Parameters.Add(new SqliteParameter("@Bairro", ModeloEndereco.Bairro));
                cmd.Parameters.Add(new SqliteParameter("@UsuarioId", ModeloEndereco.UsuarioId));

                await cmd.ExecuteNonQueryAsync();
            }

        }
        public async Task DeleteById(int id)
        {
             if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.EnderecoDeleteById;
                cmd.Parameters.Add(new SqliteParameter("@Id", id));

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<Endereco?> GetByIdAndUserIdAsync(int enderecoId, int usuarioId)
        {
             if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.EnderecoSelectByIdAndUserId;
                cmd.Parameters.Add(new SqliteParameter("@Id", enderecoId));
                cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId));

                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    // Mapeamento manual dos dados da coluna para o objeto Endereco
                    return new Endereco
                    {
                        Id = reader.GetInt32(0),
                        Apelido = reader.GetString(1),
                        Bairro = reader.GetString(2),
                        Complemento = reader.GetString(3),
                        Logradouro = reader.GetString(4),
                        Numero = reader.GetString(5),
                        UsuarioId = reader.GetInt32(6)
                    };
                }
                return null;
            }
        }
    }
}