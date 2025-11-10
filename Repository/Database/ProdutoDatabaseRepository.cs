using System.Data;
using SaborGregoNew.DTOs.Produtos;
using Microsoft.Data.SqlClient;
using SaborGregoNew.Models;
using saborGregoNew.Repository;

namespace SaborGregoNew.Repository
{
    public class ProdutoDatabaseRepository : IProdutoRepository
    {
        private readonly IDbConnection _connection;

        public ProdutoDatabaseRepository(IDbConnectionFactory connection)
        {
            _connection = connection.CreateConnection();
        }

        public async Task Create(ProdutoDTO ModeloProduto)
        {
            using var conn = _connection;
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = Queries.ProdutoInsert;
            cmd.Parameters.Add(new SqlParameter("@Nome", ModeloProduto.Nome));
            cmd.Parameters.Add(new SqlParameter("@Descricao", ModeloProduto.Descricao));
            cmd.Parameters.Add(new SqlParameter("@Preco", ModeloProduto.Preco));
            cmd.Parameters.Add(new SqlParameter("@Categoria", ModeloProduto.Categoria));
            cmd.Parameters.Add(new SqlParameter("@Imagem", ModeloProduto.Imagem));

            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public async Task<List<Produto>> SelectAllAsync()
        {
            using var conn = _connection;
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = Queries.ProdutoSelectAll;
            using var reader = cmd.ExecuteReader();
            var produtos = new List<Produto>();
            while (reader.Read())
            {
                produtos.Add(new Produto
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Descricao = reader.GetString(2),
                    Preco = reader.GetDecimal(3),
                    Categoria = reader.GetString(4),
                    Imagem = reader.GetString(5)
                });
            }
            conn.Close();
            return produtos;
        }

        public async Task<Produto?> SelectByIdAsync(int id)
        {
            using var conn = _connection;
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = Queries.ProdutoSelectById;
            cmd.Parameters.Add(new SqlParameter("@Id", id));
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Produto
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Descricao = reader.GetString(2),
                    Preco = reader.GetDecimal(3),
                    Categoria = reader.GetString(4),
                    Imagem = reader.GetString(5)
                };
            }
            conn.Close();
            return null;
        }

        public async Task UpdateById(int id, ProdutoDTO ModeloProduto)
        {
            using var conn = _connection;
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = Queries.ProdutoUpdate;
            cmd.Parameters.Add(new SqlParameter("@Id", id));
            cmd.Parameters.Add(new SqlParameter("@Nome", ModeloProduto.Nome));
            cmd.Parameters.Add(new SqlParameter("@Descricao", ModeloProduto.Descricao));
            cmd.Parameters.Add(new SqlParameter("@Preco", ModeloProduto.Preco));
            cmd.Parameters.Add(new SqlParameter("@Categoria", ModeloProduto.Categoria));
            cmd.Parameters.Add(new SqlParameter("@Imagem", ModeloProduto.Imagem));

            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public async Task DeleteById(int id)
        {
            using var conn = _connection;
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = Queries.ProdutoDeleteById;
            cmd.Parameters.Add(new SqlParameter("@Id", id));

            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}