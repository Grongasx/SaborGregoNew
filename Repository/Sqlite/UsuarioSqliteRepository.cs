using SaborGregoNew.DTOs.Usuario;
using Microsoft.Data.SqlClient;
using SaborGregoNew.Models;
using System.Data.Common;
using SaborGregoNew.Repository.Query; // Se UsuarioQuery estiver aqui
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaborGregoNew.Repository
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
        public async Task Create(RegisterUserDto ModeloUsuario) //recebe um objeto de registro e retorna um check
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
                    cmd.Parameters.Add(new SqlParameter("@Nome", ModeloUsuario.Nome));//insere o dado na coluna dele...
                    cmd.Parameters.Add(new SqlParameter("@Telefone", ModeloUsuario.Telefone));
                    cmd.Parameters.Add(new SqlParameter("@Email", ModeloUsuario.Email));
                    cmd.Parameters.Add(new SqlParameter("@Senha", ModeloUsuario.Senha));
                    cmd.Parameters.Add(new SqlParameter("@Role", (int)ModeloUsuario.Role)); //------------------------------------arrumar aqui ele deve setar no repository apenas------------------//

                    await cmd.ExecuteNonQueryAsync();//executa a query
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro na criação do usuario no banco de dados", ex);//caso de merda ele pega aqui
            }
        }

        //realiza o login de qualquer usuario
        public async Task<Usuario?> Login(LoginDTO ModeloUsuario)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");

            try
            {
                using (conn)
                {
                    await conn.OpenAsync();

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = UsuarioQuery.UsuarioLogin;
                    cmd.Parameters.Add(new SqlParameter("@Email", ModeloUsuario.Email));

                    using var reader = await cmd.ExecuteReaderAsync();
                    
                    // Verifica se encontrou algum registro
                    if (await reader.ReadAsync())
                    {
                        var usuario = new Usuario
                        {
                            Id = reader.GetInt32(0),
                            Email = reader.GetString(1),
                            Nome = reader.GetString(2),
                            Role = (SaborGregoNew.Enums.UserRole)reader.GetInt32(3),
                            Senha = reader.GetString(4),
                            Telefone = reader.GetString(5),
                        };

                        // Verifica a senha
                        if (usuario.Senha == ModeloUsuario.Senha)
                        {
                            return usuario; // Sucesso!
                        }
                    }
                    
                    // Se chegou aqui, ou não achou usuário ou a senha estava errada.
                    return null; 
                }
            }
            catch (Exception ex)
            {
                // Log do erro real no servidor (opcional, mas recomendado)
                Console.WriteLine(ex.Message); 
                return null; // Retorna null para a tela tratar como "falha no login"
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
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
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
        public async Task UpdateById(int id, RegisterUserDto ModeloUsuario)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = UsuarioQuery.UsuarioUpdateById;

                cmd.Parameters.Add(new SqlParameter("@Id", id));
                cmd.Parameters.Add(new SqlParameter("@Nome", ModeloUsuario.Nome));
                cmd.Parameters.Add(new SqlParameter("@Telefone", ModeloUsuario.Telefone));
                cmd.Parameters.Add(new SqlParameter("@Email", ModeloUsuario.Email));
                cmd.Parameters.Add(new SqlParameter("@Senha", ModeloUsuario.Senha));
                cmd.Parameters.Add(new SqlParameter("@Role", (int)ModeloUsuario.Role));

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
                cmd.CommandText = UsuarioQuery.UsuarioDeleteById;
                cmd.Parameters.Add(new SqlParameter("@Id", id));

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}