// using System.Data;
// using SaborGregoNew.DTOs.Usuario;
// using Microsoft.Data.SqlClient;
// using SaborGregoNew.Models;
// using System.Security.Claims;

// namespace saborGregoNew.Repository
// {
//     public class UsuarioDatabaseRepository : IUsuarioRepository
//     {
//         private readonly IDbConnection _connection;

//         public UsuarioDatabaseRepository(IDbConnectionFactory connection)
//         {
//             _connection = connection.CreateConnection();
//         }

//         public async Task Create(RegisterDto ModeloUsuario)
//         {
//             using var conn = _connection;
//             conn.Open();

//             using var cmd = conn.CreateCommand();
//             cmd.CommandText = Queries.UsuarioRegistrar;
//             cmd.Parameters.Add(new SqlParameter("@Nome", ModeloUsuario.Nome));
//             cmd.Parameters.Add(new SqlParameter("@Telefone", ModeloUsuario.Telefone));
//             cmd.Parameters.Add(new SqlParameter("@Email", ModeloUsuario.Email));
//             cmd.Parameters.Add(new SqlParameter("@Senha", ModeloUsuario.Senha));
//             cmd.Parameters.Add(new SqlParameter("@Role", (int)ModeloUsuario.Role));

//             cmd.ExecuteNonQuery();
//             conn.Close();
//         }

//         public async Task<Usuario?> Login(LoginDTO ModeloUsuario)
//         {
//             using var conn = _connection;
//             conn.Open();

//             using var cmd = conn.CreateCommand();
//             cmd.CommandText = Queries.UsuarioLogin;
//             cmd.Parameters.Add(new SqlParameter("@Email", ModeloUsuario.Email));
//             cmd.Parameters.Add(new SqlParameter("@Senha", ModeloUsuario.Senha));

//             using var reader = cmd.ExecuteReader();
//             if (reader.Read())
//             {
//                 return new Usuario
//                 {
//                     Id = reader.GetInt32(0),
//                     Nome = reader.GetString(1),
//                     Telefone = reader.GetString(2),
//                     Email = reader.GetString(3),
//                     Senha = reader.GetString(4),
//                     Role = (SaborGregoNew.Enums.UserRole)reader.GetInt32(5)
//                 };
//             }
//             conn.Close();
//             return null;
//         }

//         public async Task<List<Usuario>> SelectAllAsync()
//         {
//             using var conn = _connection;
//             conn.Open();
//             using var cmd = conn.CreateCommand();
//             cmd.CommandText = Queries.UsuarioListagem;
//             using var reader = cmd.ExecuteReader();
//             var usuarios = new List<Usuario>();
//             while (reader.Read())
//             {
//                 usuarios.Add(new Usuario
//                 {
//                     Id = reader.GetInt32(0),
//                     Nome = reader.GetString(1),
//                     Telefone = reader.GetString(2),
//                     Email = reader.GetString(3),
//                     Senha = reader.GetString(4),
//                     Role = (SaborGregoNew.Enums.UserRole)reader.GetInt32(5)
//                 });
//             }
//             conn.Close();
//             return usuarios;
//         }

//         public async Task<Usuario?> SelectByIdAsync(int id)
//         {
//             using var conn = _connection;
//             conn.Open();
//             using var cmd = conn.CreateCommand();
//             cmd.CommandText = Queries.UsuarioById;
//             cmd.Parameters.Add(new SqlParameter("@Id", id));
//             using var reader = cmd.ExecuteReader();
//             if (reader.Read())
//             {
//                 return new Usuario
//                 {
//                     Id = reader.GetInt32(0),
//                     Nome =  reader.GetString(1),
//                     Telefone = reader.GetString(2),
//                     Email = reader.GetString(3),
//                     Senha = reader.GetString(4),
//                     Role = (SaborGregoNew.Enums.UserRole)reader.GetInt32(5)
//                 };
//             }
//             conn.Close();
//             return null;
//         }

//         public async Task UpdateById(int id, RegisterDto ModeloUsuario)
//         {
//             using var conn = _connection;
//             conn.Open();

//             using var cmd = conn.CreateCommand();
//             cmd.CommandText = Queries.UsuarioUpdateById;

//             cmd.Parameters.Add(new SqlParameter("@Id", id));
//             cmd.Parameters.Add(new SqlParameter("@Nome", ModeloUsuario.Nome));
//             cmd.Parameters.Add(new SqlParameter("@Telefone", ModeloUsuario.Telefone));
//             cmd.Parameters.Add(new SqlParameter("@Email", ModeloUsuario.Email));
//             cmd.Parameters.Add(new SqlParameter("@Senha", ModeloUsuario.Senha));
//             cmd.Parameters.Add(new SqlParameter("@Role", (int)ModeloUsuario.Role));

//             cmd.ExecuteNonQuery();
//             conn.Close();
//         }

//         public async Task DeleteById(int id)
//         {
//             using var conn = _connection;
//             conn.Open();

//             using var cmd = conn.CreateCommand();
//             cmd.CommandText = Queries.UsuarioDeleteById;
//             cmd.Parameters.Add(new SqlParameter("@Id", id));

//             cmd.ExecuteNonQuery();
//             conn.Close();
//         }
//     }
// }