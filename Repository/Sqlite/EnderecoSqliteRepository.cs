using saborGregoNew.Repository;
using Microsoft.Data.Sqlite;
using SaborGregoNew.Models;
using SaborGregoNew.DTOs.Usuario;
using System.Data.Common;

namespace SaborGregoNew.Repository
{
    public class EnderecoSqliteRepository : IEnderecoRepository
    {

        //Conexão com o banco//
        private readonly IDbConnectionFactory _connectionFactory;
        public EnderecoSqliteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        //Cria um novo endereço//
        public async Task Create(CadastroEnderecoDTO ModeloEndereco, int usuarioId)//recebe um endereço e o id do usuário.
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)//abrir conexão com o banco de dados
                throw new InvalidOperationException("Falha ao obter conexão");
            try
            {
                using (conn)
                {
                    await conn.OpenAsync();//abre conexão com o banco de dados

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = Queries.EnderecoInsert;//query para o banco de dados.

                    // Parametros dos dados enviado do frontend//
                    cmd.Parameters.Add(new SqliteParameter("@Apelido", ModeloEndereco.Apelido));
                    cmd.Parameters.Add(new SqliteParameter("@Logradouro", ModeloEndereco.Logradouro));
                    cmd.Parameters.Add(new SqliteParameter("@Numero", ModeloEndereco.Numero));
                    cmd.Parameters.Add(new SqliteParameter("@Complemento", ModeloEndereco.Complemento));
                    cmd.Parameters.Add(new SqliteParameter("@Bairro", ModeloEndereco.Bairro));
                    cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId));

                    await cmd.ExecuteNonQueryAsync(); //executa a query com os parametros fornecidos
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao Criar Endereço no banco de dados.", ex);//caso algo de erro, ele acusa onde foi.
            }
        }

        //Seleciona todos os endereços de um usuário//
        public async Task<List<Endereco>> SelectAllByUserIdAsync(int usuarioId)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)//abrir conexão com o banco de dados
                throw new InvalidOperationException("Falha ao obter conexão");
            try
            {
                using (conn)
                {
                    await conn.OpenAsync();//abre conexão com o banco de dados

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = Queries.EnderecoSelectByUserId;//query para o banco de dados.

                    cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId));//cria um parametro para buscar o produto pelo id

                    using var reader = await cmd.ExecuteReaderAsync();//executa um reader para ler cada coluna no banco de dados

                    var enderecos = new List<Endereco>();//cria uma lista de produtos para enviar ao frontend
                    while (await reader.ReadAsync())//loop para inserir todos os endereços na lista
                    {
                        enderecos.Add(new Endereco
                        {
                            Id = reader.GetInt32(0),//le a primeira coluna e coloca no model como Id.
                            Apelido = reader.GetString(1),//le a segunda coluna e coloca no model como Categoria...
                            Logradouro = reader.GetString(2),
                            Numero = reader.GetString(3),
                            Complemento = reader.GetString(4),
                            Bairro = reader.GetString(5),
                            UsuarioId = reader.GetInt32(6)
                        });

                    }
                    return enderecos;//retorna a lista para o frontend.
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Houve um problema ao selecionar os endereços no banco de dados", ex);
            }
        }

        //Seleciona um endereço pelo seu ID//
        public async Task<Endereco?> SelectByIdAsync(int id)//recene um id e retorna um endereço ou null
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)//abre uma conexão com o banco de dados
                throw new InvalidOperationException("Falha ao obter conexão");
            try
            {
                using (conn)
                {
                    await conn.OpenAsync();//abre uma nova conexão com o banco de dados;

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = Queries.EnderecoSelectById; //query para o banco.

                    cmd.Parameters.Add(new SqliteParameter("@Id", id));//parametro do id do endereço
                    using var reader = await cmd.ExecuteReaderAsync();//executa um reader para buscar os dados no anco de dados.

                    if (await reader.ReadAsync())//inicia a leitura do banco de dados.
                    {
                        return new Endereco
                        {
                            Id = reader.GetInt32(0),//seleciona o dado q tiver na primeira coluna da tabela
                            Apelido = reader.GetString(1),//seleciona o dado q tiver na segunda coluna da tabela...
                            Logradouro = reader.GetString(2),
                            Numero = reader.GetString(3),
                            Complemento = reader.GetString(4),
                            Bairro = reader.GetString(5),
                            UsuarioId = reader.GetInt32(6)
                        };
                    }
                    return null;//se n, retorna nulo
                }
            }
            catch(Exception ex)
            {
                throw new Exception("House um problema ao obter o endereço", ex);//caso de erro ele acusa oq foi
            }
        }

        //Atualiza um endereço//
        public async Task UpdateById(int Id, CadastroEnderecoDTO ModeloEndereco)//recebe o id e um objeto endereço
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)//cria a conexão com o banco
                throw new InvalidOperationException("Falha ao obter conexão");
            try
            {
                using (conn)
                {
                    await conn.OpenAsync();//abre conexão

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = Queries.EnderecoUpdate;//query para o banco

                    //parametros para o update
                    cmd.Parameters.Add(new SqliteParameter("@Id", Id));//recebe o id do endereço para encontra-lo
                    cmd.Parameters.Add(new SqliteParameter("@Apelido", ModeloEndereco.Apelido));//cria o dado na tabela...
                    cmd.Parameters.Add(new SqliteParameter("@Logradouro", ModeloEndereco.Logradouro));
                    cmd.Parameters.Add(new SqliteParameter("@Numero", ModeloEndereco.Numero));
                    cmd.Parameters.Add(new SqliteParameter("@Complemento", ModeloEndereco.Complemento));
                    cmd.Parameters.Add(new SqliteParameter("@Bairro", ModeloEndereco.Bairro));
                    cmd.Parameters.Add(new SqliteParameter("@UsuarioId", ModeloEndereco.UsuarioId));

                    await cmd.ExecuteNonQueryAsync();//executa a query
                }
            }
            catch(Exception ex)
            {
                throw new Exception("erro ao atualizar o endereço no banco", ex);//acusa caso de alguma merda
            }
        }

        //Seleciona um endereço pelo seu ID e UsuarioId//
        public async Task<Endereco?> GetByIdAndUserIdAsync(int enderecoId, int usuarioId)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)//cria a conexão
                throw new InvalidOperationException("Falha ao obter conexão");
            try
            {
                using (conn)
                {
                    await conn.OpenAsync();//abre a conexão

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = Queries.EnderecoSelectByIdAndUserId;//query para o banco de dados

                    cmd.Parameters.Add(new SqliteParameter("@Id", enderecoId));//parametro para encontrar o Endereço
                    cmd.Parameters.Add(new SqliteParameter("@UsuarioId", usuarioId));//parametro para encontrar o usuario

                    using var reader = await cmd.ExecuteReaderAsync();//executa a leitura do banco de dados

                    if (await reader.ReadAsync())//caso encontre no banco, cria um novo objeto de endereco
                    {
                        // Mapeamento manual dos dados da coluna para o objeto Endereco
                        return new Endereco
                        {
                            Id = reader.GetInt32(0),//pega o dado da primeira coluna e coloca no id do modelo
                            Apelido = reader.GetString(1),//pega o dado da segunda coluna e coloca no apelido do modelo...
                            Logradouro = reader.GetString(2),
                            Numero = reader.GetString(3),
                            Complemento = reader.GetString(4),
                            Bairro = reader.GetString(5),
                            UsuarioId = reader.GetInt32(6)
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter os endereços do usuario", ex);//caso de merda ele lança o erro e acusa onde foi
            }
        }

        //Metodo para "deletar" o Endereço//
        public async Task DesativarAsync(int id)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)//cria a conexão
                throw new Exception("Falha ao obter conexão");

            try //caso algo de errado pula para catch
            {
                using (conn)//usa a conexão
                {
                    await conn.OpenAsync();//abre conexão

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = Queries.EnderecoDesativar;//muda status de ativo
                    cmd.Parameters.Add(new SqliteParameter("@Id", id));//com base no endereço

                    await cmd.ExecuteNonQueryAsync();// executa a query
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Algo deu errado ao desativar endereço no banco de dados.", ex); //caso de alguma merda, acusa aqui
            }
        }

        //===============================================================//
        //=============Não usar esse metodo em produção==================//
        //===============================================================//

        //Realmente deleta o endereço pelo id//
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
    }
}