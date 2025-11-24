using SaborGregoNew.DTOs.Produtos;
using Microsoft.Data.Sqlite;
using SaborGregoNew.Models;
using System.Data.Common;
using SaborGregoNew.Repository;

namespace SaborGregoNew.Repository
{
    public class ProdutoSqliteRepository : IProdutoRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public ProdutoSqliteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task Create(ProdutoDTO ModeloProduto)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");
            using (conn)
            {
                try
                {
                    await conn.OpenAsync();
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = SaborGregoNew.Repository.Queries.ProdutoInsert; 
                    cmd.Parameters.Add(new SqliteParameter("@Nome", ModeloProduto.Nome ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Descricao", ModeloProduto.Descricao ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Preco", ModeloProduto.Preco));
                    cmd.Parameters.Add(new SqliteParameter("@Categoria", ModeloProduto.Categoria ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Imagem", ModeloProduto.Imagem ?? (object)DBNull.Value));
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("Falha ao Criar Produto no banco de dados.", ex); 
                }
            }
        }
        public async Task<List<Produto>> SelectAllAsync() 
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");
            try
            {
                using (conn)
                {
                    await conn.OpenAsync();
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = SaborGregoNew.Repository.Queries.ProdutoSelectAll;//query para o banco de dados.
                    using var reader = await cmd.ExecuteReaderAsync();//executa um reader para ler cada coluna no banco de dados
                    var produtos = new List<Produto>();//cria uma lista de produtos para enviar ao frontend
                    while (await reader.ReadAsync())// abre um loop que lê todas as linhas e tras os dados um por um do banco de dados.
                    {
                        produtos.Add(new Produto
                        {
                            Id = reader.GetInt32(0),//le a primeira coluna e coloca no model como Id.
                            Categoria = reader.GetString(1),//le a segunda coluna e coloca no model como Categoria...
                            Descricao = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Imagem = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Nome = reader.GetString(4),
                            Preco = reader.GetDecimal(5),
                            Ativo = reader.GetBoolean(6)
                        });
                    }
                    return produtos;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao listar Produtos no banco de dados.", ex);
            }
        }
        public async Task<Produto?> SelectByIdAsync(int id) 
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn) 
                throw new InvalidOperationException("Falha ao obter conexão.");
            try
            {
                using (conn)
                {
                    await conn.OpenAsync();//abre conexão com o banco de dados
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = SaborGregoNew.Repository.Queries.ProdutoSelectById;//query para o banco de dados.
                    cmd.Parameters.Add(new SqliteParameter("@Id", id));//cria um parametro para buscar o produto pelo id
                    using var reader = await cmd.ExecuteReaderAsync();//executa um reader para ler cada coluna no banco de dados
                    if (await reader.ReadAsync())
                    {
                        return new Produto
                        {
                            Id = reader.GetInt32(0), //le a primeira coluna e coloca no model como Id.
                            Categoria = reader.GetString(1), //le a segunda coluna e coloca no model como Categoria...
                            Descricao = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Imagem = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Nome = reader.GetString(4),
                            Preco = reader.GetDecimal(5),
                            Ativo = reader.GetBoolean(6)
                        };
                    }
                    return null;//caso ele não ache nada, retorna nulo
                }
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao listar Produto no banco de dados.", e); //caso algo de erro, ele acusa onde foi.
            }
        }
        
        //Metodo para "deletar" produto//
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
                    cmd.CommandText = SaborGregoNew.Repository.Queries.ProdutoDesativar;//muda status de ativo
                    cmd.Parameters.Add(new SqliteParameter("@Id", id));//com base no endereço
                    await cmd.ExecuteNonQueryAsync();// executa a query
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Algo deu errado ao desativar endereço no banco de dados.", ex); //caso de alguma merda, acusa aqui
            }
        }

        //Metodo para "ativar" produto//
        public async Task AtivarAsync(int id)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)//cria a conexão
                throw new Exception("Falha ao obter conexão");
            try //caso algo de errado pula para catch
            {
                using (conn)//usa a conexão
                {
                    await conn.OpenAsync();//abre conexão
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = SaborGregoNew.Repository.Queries.ProdutoAtivar;//muda status de ativo
                    cmd.Parameters.Add(new SqliteParameter("@Id", id));//com base no endereço
                    await cmd.ExecuteNonQueryAsync();// executa a query
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Algo deu errado ao ativar produto no banco de dados.", ex); //caso de alguma merda, acusa aqui
            }
        }
    
        public async Task UpdateById(int id, ProdutoDTO ModeloProduto)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");
            try
            {
                using (conn)
                {
                    await conn.OpenAsync();
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = SaborGregoNew.Repository.Queries.ProdutoUpdate; 
                    cmd.Parameters.Add(new SqliteParameter("@Id", id));
                    cmd.Parameters.Add(new SqliteParameter("@Nome", ModeloProduto.Nome ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Descricao", ModeloProduto.Descricao ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Preco", ModeloProduto.Preco)); 
                    cmd.Parameters.Add(new SqliteParameter("@Categoria", ModeloProduto.Categoria ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Imagem", ModeloProduto.Imagem ?? (object)DBNull.Value));
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao atualizar Produto no banco de dados.", ex);
            }
        }
    }
}