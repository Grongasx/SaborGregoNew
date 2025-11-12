using SaborGregoNew.DTOs.Usuario;
using Microsoft.Data.Sqlite;
using SaborGregoNew.Models;
using System.Data.Common;
using SaborGregoNew.Repository.Query;

namespace saborGregoNew.Repository
{
    public class UsuarioSqliteRepository : IUsuarioRepository
    {

        //conexão com o banco de dados//
        private readonly IDbConnectionFactory _connectionFactory;
        public UsuarioSqliteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        //==============================================//
        //=========Corrigir problema nas roles==========//
        //==============================================//

        //Cria um Cliente no banco de dados 
        public async Task Create(RegisterDto ModeloUsuario) //recebe um objeto de registro e retorna um check
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)//cria conexão c banco
                throw new InvalidOperationException("Falha ao obter conexão.");
            try
            {
                using (conn)
                {
                    await conn.OpenAsync();//abre conexão ocm o banco

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = UsuarioQuery.UsuarioRegistrar;//query para o banco de dados

                    //parametros para criação do usuario (seta o Usuario como cliente)
                    cmd.Parameters.Add(new SqliteParameter("@Nome", ModeloUsuario.Nome));//insere o dado na coluna dele...
                    cmd.Parameters.Add(new SqliteParameter("@Telefone", ModeloUsuario.Telefone));
                    cmd.Parameters.Add(new SqliteParameter("@Email", ModeloUsuario.Email));
                    cmd.Parameters.Add(new SqliteParameter("@Senha", ModeloUsuario.Senha));
                    cmd.Parameters.Add(new SqliteParameter("@Role", (int)ModeloUsuario.Role)); //------------------------------------arrumar aqui ele deve setar no repository apenas------------------//

                    await cmd.ExecuteNonQueryAsync();//executa a query
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro na criação do usuario no banco de dados", ex);//caso de merda ele pega aqui
            }
        }

        //realiza o login de qualquer usuario
        public async Task<Usuario?> Login(LoginDTO ModeloUsuario)//recebe um objeto de login e retorna Usuario completo
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)//cria conexão com o banco
                throw new InvalidOperationException("Falha ao obter conexão.");

            try
            {
                using (conn)
                {
                    await conn.OpenAsync();//abre conexão com o banco de dados

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = UsuarioQuery.UsuarioLogin;//query para realizar o login

                    cmd.Parameters.Add(new SqliteParameter("@Email", ModeloUsuario.Email));//parametro do email

                    using var reader = await cmd.ExecuteReaderAsync();
                    var usuario = new Usuario();
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
                    if (usuario == null)
                    {
                        throw new KeyNotFoundException("Usuario não encontrado.");
                    }
                    else if (usuario.Senha == ModeloUsuario.Senha)
                    {
                        return usuario;
                    }
                    else if (usuario.Senha == ModeloUsuario.Senha)
                    {
                        throw new UnauthorizedAccessException("Senha incorreta.");
                    }
                    else
                    {
                        throw new Exception("deus lá sabe oq aconteceu");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("erro no banco de dados ao efetuar login", ex);
            }
        }

        
        //===============================//
        //==implementar mais pra frente==//
        //===============================//
        
        //seleciona todos os usuario ------ ainda não implementado em nada, porém util
        public async Task<List<Usuario>> SelectAllAsync()
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");
            try
            {
                using (conn)
                {
                    await conn.OpenAsync();

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = UsuarioQuery.UsuarioListagem;
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
            catch (Exception ex)
            {
                throw new Exception("erro ao selecionar todos os usuarios no banco de dados", ex);
            }
        }

        //Selecionar usuario pelo id --- ainda não implementado, porém util
        public async Task<Usuario?> SelectByIdAsync(int id)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");
            try
            {
                using (conn)
                {
                    await conn.OpenAsync();

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = UsuarioQuery.UsuarioById;
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
            catch (Exception ex)
            {
                throw new Exception("erro ao selecionar usuario no banco de dados", ex);
            }
        }

        //Atualizar dados do usuario --- ainda não implementado
        public async Task UpdateById(int id, RegisterDto ModeloUsuario)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = UsuarioQuery.UsuarioUpdateById;

                cmd.Parameters.Add(new SqliteParameter("@Id", id));
                cmd.Parameters.Add(new SqliteParameter("@Nome", ModeloUsuario.Nome));
                cmd.Parameters.Add(new SqliteParameter("@Telefone", ModeloUsuario.Telefone));
                cmd.Parameters.Add(new SqliteParameter("@Email", ModeloUsuario.Email));
                cmd.Parameters.Add(new SqliteParameter("@Senha", ModeloUsuario.Senha));
                cmd.Parameters.Add(new SqliteParameter("@Role", (int)ModeloUsuario.Role));

                await cmd.ExecuteNonQueryAsync();
            }
        }
        
        
        
        //==============================================//
        //===========Não usar em produção===============//
        //==============================================//
        //deleta o usuario do banco de dados
        public async Task DeleteById(int id)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = UsuarioQuery.UsuarioDeleteById;
                cmd.Parameters.Add(new SqliteParameter("@Id", id));

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}