using System.Data;
using SaborGregoNew.DTOs.Produtos;
using Microsoft.Data.Sqlite;
using SaborGregoNew.Models;
using System.Data.Common;

namespace saborGregoNew.Repository
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
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.ProdutoInsert;
                cmd.Parameters.Add(new SqliteParameter("@Nome", ModeloProduto.Nome));
                cmd.Parameters.Add(new SqliteParameter("@Descricao", ModeloProduto.Descricao));
                cmd.Parameters.Add(new SqliteParameter("@Preco", ModeloProduto.Preco));
                cmd.Parameters.Add(new SqliteParameter("@Categoria", ModeloProduto.Categoria));
                cmd.Parameters.Add(new SqliteParameter("@Imagem", ModeloProduto.Imagem));

                await cmd.ExecuteNonQueryAsync();

            }
        }
        public async Task<List<Produto>> SelectAllAsync()
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.ProdutoSelectAll;
                using var reader = await cmd.ExecuteReaderAsync();
                var produtos = new List<Produto>();
                while (await reader.ReadAsync())
                {
                    produtos.Add(new Produto
                    {
                        Id = reader.GetInt32(0),
                        Categoria = reader.GetString(1),
                        Descricao = reader.GetString(2),
                        Imagem = reader.GetString(3),
                        Nome = reader.GetString(4),
                        Preco = reader.GetDecimal(5)

                    });
                }
                return produtos;

            }
        }
        public async Task<Produto?> SelectByIdAsync(int id)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");
            using (conn)
            {
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.ProdutoSelectById;
                cmd.Parameters.Add(new SqliteParameter("@Id", id));
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Produto
                    {
                        Id = reader.GetInt32(0),
                        Categoria = reader.GetString(1),
                        Descricao = reader.GetString(2),
                        Imagem = reader.GetString(3),
                        Nome = reader.GetString(4),
                        Preco = reader.GetDecimal(5)
                    };
                }
                return null;

            }
        }
        public async Task UpdateById(int id, ProdutoDTO ModeloProduto)
        {
            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão.");
            using (conn)
            {
                await conn.OpenAsync();
                
                using var cmd = conn.CreateCommand();
                cmd.CommandText = Queries.ProdutoUpdate;
                cmd.Parameters.Add(new SqliteParameter("@Id", id));
                cmd.Parameters.Add(new SqliteParameter("@Nome", ModeloProduto.Nome));
                cmd.Parameters.Add(new SqliteParameter("@Descricao", ModeloProduto.Descricao));
                cmd.Parameters.Add(new SqliteParameter("@Preco", ModeloProduto.Preco));
                cmd.Parameters.Add(new SqliteParameter("@Categoria", ModeloProduto.Categoria));
                cmd.Parameters.Add(new SqliteParameter("@Imagem", ModeloProduto.Imagem));

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
                cmd.CommandText = Queries.ProdutoDeleteById;
                cmd.Parameters.Add(new SqliteParameter("@Id", id));

                cmd.ExecuteNonQuery();
                conn.Close();

            }
        }
    }
}