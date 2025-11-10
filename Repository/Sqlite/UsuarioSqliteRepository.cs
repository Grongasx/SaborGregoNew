using System.Data;
using SaborGregoNew.DTOs.Usuario;
using Microsoft.Data.Sqlite;
using SaborGregoNew.Models;
using System.Security.Claims;
using System.Data.Common;

namespace saborGregoNew.Repository
{
    public class UsuarioSqliteRepository : IUsuarioRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UsuarioSqliteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task Create(RegisterDto ModeloUsuario)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.UsuarioRegistrar;
                cmd.Parameters.Add(new SqliteParameter("@Nome", ModeloUsuario.Nome));
                cmd.Parameters.Add(new SqliteParameter("@Telefone", ModeloUsuario.Telefone));
                cmd.Parameters.Add(new SqliteParameter("@Email", ModeloUsuario.Email));
                cmd.Parameters.Add(new SqliteParameter("@Senha", ModeloUsuario.Senha));
                cmd.Parameters.Add(new SqliteParameter("@Role", (int)ModeloUsuario.Role));

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<Usuario?> Login(LoginDTO ModeloUsuario)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");

            Usuario? usuario = null;

            using (conn)
            {
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();

                cmd.CommandText = Queries.UsuarioLogin;
                cmd.Parameters.Add(new SqliteParameter("@Email", ModeloUsuario.Email));

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    usuario = new Usuario
                    {
                        Id = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        Nome = reader.GetString(2),
                        Role = (SaborGregoNew.Enums.UserRole)reader.GetInt32(3),
                        Senha = reader.GetString(4),
                        Telefone = reader.GetString(5),
                    };
                }
            }

            if (usuario == null)
            { return null; }

            if (ModeloUsuario.Senha == usuario.Senha)
            { return usuario; }

            else
            { return null; }
        }
        public async Task<List<Usuario>> SelectAllAsync()
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.UsuarioListagem;
                using var reader = await cmd.ExecuteReaderAsync();
                var usuarios = new List<Usuario>();
                while (await reader.ReadAsync())
                {
                    usuarios.Add(new Usuario
                    {
                        Id = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        Nome = reader.GetString(2),
                        Role = (SaborGregoNew.Enums.UserRole)reader.GetInt32(3),
                        Senha = reader.GetString(4),
                        Telefone = reader.GetString(5),
                    });
                }
                return usuarios;
            }
        }
        public async Task<Usuario?> SelectByIdAsync(int id)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.UsuarioById;
                cmd.Parameters.Add(new SqliteParameter("@Id", id));
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Usuario
                    {
                        Id = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        Nome = reader.GetString(2),
                        Role = (SaborGregoNew.Enums.UserRole)reader.GetInt32(3),
                        Senha = reader.GetString(4),
                        Telefone = reader.GetString(5),
                    };
                }
                return null;
            }
        }
        public async Task UpdateById(int id, RegisterDto ModeloUsuario)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.UsuarioUpdateById;

                cmd.Parameters.Add(new SqliteParameter("@Id", id));
                cmd.Parameters.Add(new SqliteParameter("@Nome", ModeloUsuario.Nome));
                cmd.Parameters.Add(new SqliteParameter("@Telefone", ModeloUsuario.Telefone));
                cmd.Parameters.Add(new SqliteParameter("@Email", ModeloUsuario.Email));
                cmd.Parameters.Add(new SqliteParameter("@Senha", ModeloUsuario.Senha));
                cmd.Parameters.Add(new SqliteParameter("@Role", (int)ModeloUsuario.Role));

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task DeleteById(int id)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.UsuarioDeleteById;
                cmd.Parameters.Add(new SqliteParameter("@Id", id));

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}