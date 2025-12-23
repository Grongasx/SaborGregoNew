// using saborGregoNew.Repository;
// using Microsoft.Data.SqlClient;
// using SaborGregoNew.Models;
// using System.Data;
// using SaborGregoNew.DTOs.Usuario;

// namespace SaborGregoNew.Repository
// {
//     public class EnderecoDatabaseRepository : IEnderecoRepository
//     {
//         private readonly IDbConnection _connection;

//         public EnderecoDatabaseRepository(IDbConnectionFactory connection)
//         {
//             _connection = connection.CreateConnection();
//         }

//         public async Task Create(EnderecoDTO ModeloEndereco, int usuarioId)
//         {
//             using var conn = _connection;
//             conn.Open();

//             using var cmd = conn.CreateCommand();
//             cmd.CommandText = Queries.EnderecoInsert;
//             cmd.Parameters.Add(new SqlParameter("@Apelido", ModeloEndereco.Apelido));
//             cmd.Parameters.Add(new SqlParameter("@Logradouro", ModeloEndereco.Logradouro));
//             cmd.Parameters.Add(new SqlParameter("@Numero", ModeloEndereco.Numero));
//             cmd.Parameters.Add(new SqlParameter("@Complemento", ModeloEndereco.Complemento));
//             cmd.Parameters.Add(new SqlParameter("@Bairro", ModeloEndereco.Bairro));
//             cmd.Parameters.Add(new SqlParameter("@UsuarioId", ModeloEndereco.UsuarioId));

//             cmd.ExecuteNonQuery();
//             conn.Close();
//         }

//         public async Task<List<Endereco>> SelectAllByUserIdAsync(int usuarioId)
//         {
//             using var conn = _connection;
//             conn.Open();
//             using var cmd = conn.CreateCommand();
//             cmd.CommandText = Queries.EnderecoSelectByUserId;
//             cmd.Parameters.Add(new SqlParameter("@UsuarioId", usuarioId));
//             using var reader = cmd.ExecuteReader();
//             if (reader.Read())
//             {
//                 new Endereco
//                 {
//                     Id = reader.GetInt32(0),
//                     Apelido = reader.GetString(1),
//                     Logradouro = reader.GetString(2),
//                     Numero = reader.GetString(3),
//                     Complemento = reader.GetString(4),
//                     Bairro = reader.GetString(5),
//                     UsuarioId = reader.GetInt32(6)
//                 };
//             }
//             conn.Close();
//             return new List<Endereco>();
//         }

//         public async Task<Endereco?> SelectByIdAsync(int id)
//         {
//             using var conn = _connection;
//             conn.Open();
//             using var cmd = conn.CreateCommand();
//             cmd.CommandText = Queries.EnderecoSelectById;
//             cmd.Parameters.Add(new SqlParameter("@Id", id));
//             using var reader = cmd.ExecuteReader();
//             if (reader.Read())
//             {
//                 return new Endereco
//                 {
//                     Id = reader.GetInt32(0),
//                     Apelido = reader.GetString(1),
//                     Logradouro = reader.GetString(2),
//                     Numero = reader.GetString(3),
//                     Complemento = reader.GetString(4),
//                     Bairro = reader.GetString(5),
//                     UsuarioId = reader.GetInt32(6)
//                 };
//             }
//             conn.Close();
//             return null;
//         }

//         public async Task<Endereco?> SelectByUserIdAsync(int usuarioId)
//         {
//             using var conn = _connection;
//             conn.Open();
//             using var cmd = conn.CreateCommand();
//             cmd.CommandText = Queries.EnderecoSelectByUserId;
//             cmd.Parameters.Add(new SqlParameter("@UsuarioId", usuarioId));
//             using var reader = cmd.ExecuteReader();
//             if (reader.Read())
//             {
//                 return new Endereco
//                 {
//                     Id = reader.GetInt32(0),
//                     Apelido = reader.GetString(1),
//                     Logradouro = reader.GetString(2),
//                     Numero = reader.GetString(3),
//                     Complemento = reader.GetString(4),
//                     Bairro = reader.GetString(5),
//                     UsuarioId = reader.GetInt32(6)
//                 };
//             }
//             conn.Close();
//             return null; // Retorna null se nenhum endereço for encontrado
//         }

//         public async Task UpdateById(int Id, EnderecoDTO ModeloEndereco)
//         {
//             using var conn = _connection;
//             conn.Open();

//             using var cmd = conn.CreateCommand();
//             cmd.CommandText = Queries.EnderecoUpdate;
//             cmd.Parameters.Add(new SqlParameter("@Id", Id));
//             cmd.Parameters.Add(new SqlParameter("@Apelido", ModeloEndereco.Apelido));
//             cmd.Parameters.Add(new SqlParameter("@Logradouro", ModeloEndereco.Logradouro));
//             cmd.Parameters.Add(new SqlParameter("@Numero", ModeloEndereco.Numero));
//             cmd.Parameters.Add(new SqlParameter("@Complemento", ModeloEndereco.Complemento));
//             cmd.Parameters.Add(new SqlParameter("@Bairro", ModeloEndereco.Bairro));
//             cmd.Parameters.Add(new SqlParameter("@UsuarioId", ModeloEndereco.UsuarioId));

//             cmd.ExecuteNonQuery();
//             conn.Close();
//         }

//         public async Task DeleteById(int id)
//         {
//             using var conn = _connection;
//             conn.Open();

//             using var cmd = conn.CreateCommand();
//             cmd.CommandText = Queries.EnderecoDeleteById;
//             cmd.Parameters.Add(new SqlParameter("@Id", id));

//             cmd.ExecuteNonQuery();
//             conn.Close();
//         }
        
//         public async Task<Endereco?> GetByIdAndUserIdAsync(int enderecoId, int usuarioId)
//         {
//             using var conn = _connection;
//             conn.Open();

//             using var cmd = conn.CreateCommand();
//             // A Query deve selecionar todas as colunas necessárias para montar o objeto Endereco
//             cmd.CommandText = "SELECT Id, Apelido, Logradouro, Numero, Complemento, Bairro, UsuarioId FROM Enderecos WHERE Id = @Id AND UsuarioId = @UsuarioId";
            
//             // Parâmetros de segurança para prevenir SQL Injection
//             cmd.Parameters.Add(new SqlParameter("@Id", enderecoId));
//             cmd.Parameters.Add(new SqlParameter("@UsuarioId", usuarioId));

//             using var reader = cmd.ExecuteReader();
            
//             if (reader.Read())
//             {
//                 // Mapeamento manual dos dados da coluna para o objeto Endereco
//                 return new Endereco
//                 {
//                     Id = reader.GetInt32(0),
//                     Apelido = reader.GetString(1),
//                     Logradouro = reader.GetString(2),
//                     Numero = reader.GetString(3),
//                     Complemento = reader.IsDBNull(4) ? null : reader.GetString(4),
//                     Bairro = reader.GetString(5),
//                     UsuarioId = reader.GetInt32(6)
//                 };
//             }
            
//             return null; // Retorna null se não encontrar o endereço
//         }
        
//     }
// }