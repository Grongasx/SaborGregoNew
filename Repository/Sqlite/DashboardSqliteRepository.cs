// using System.Data.Common;
// using saborGregoNew.Repository;
// using SaborGregoNew.Models;
// using Microsoft.Data.Sqlite;
// using System.Data.Common;


// namespace SaborGregoNew.Repository
// {

//     public class DashboardSqliteRepository : IDashboardRepository
//     {
//         private readonly IDbConnectionFactory _connectionFactory;

//         public DashboardSqliteRepository(IDbConnectionFactory connectionFactory)
//         {
//             _connectionFactory = connectionFactory;
//         }


//         //Lembrete:
//         //criar os models para tratar a vinda de dados da tabela


//         //================//
//         //=====Pedidos====//
//         //================//
//         public async Task<List<Pedido>> PedidosMensaisAsync()
//         {
//             if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
//                 throw new InvalidOperationException("Falha ao obter conexão");
//             using (conn)
//             {
//                 await conn.OpenAsync();

//                 using var cmd = conn.CreateCommand();
//                 cmd.CommandText = Queries.QtdPedidosMensais;
//                 using var reader = await cmd.ExecuteReaderAsync();
//                 var pedidos = new List<Pedido>();
//                 while (await reader.ReadAsync())
//                 {
//                     pedidos.Add(new Pedido
//                     {
//                         //arrumar oque vem da tabela aqui

//                     }
//                 );
//                 }
//                 return pedidos;
//             }
//         }

//         public async Task<List<Pedido>> PedidosDiariosAsync()
//         {
//             if (_connectionFactory.CreateConnection() is not DbConnection conn)
//                 throw new InvalidOperationException("Falha ao obter conexão");
//             using (conn)
//             {
//                 await conn.OpenAsync();
//                 using var cmd = conn.CreateCommand();
//                 cmd.CommandText = Queries.QtdPedidosDiarios;
//                 using var reader = await cmd.ExecuteReaderAsync();
//                 var pedidos = new List<Pedido>();
//                 while (await reader.ReadAsync())
//                 {
//                     pedidos.Add(new Produto
//                     {
//                         //arrumar oque vem da tabela aqui

//                     }
//                 );
//                 }
//                 return pedidos;
//             }
//         }


//         //================//
//         //====Produtos====//
//         //================//

//         public async Task<List<Produto>> ProdutosMensaisAsync()
//         {
//             if (_connectionFactory.CreateConnection() is not DbConnection conn)
//                 throw new InvalidOperationException("Falha ao obter conexão");
//             using (conn)
//             {
//                 await conn.OpenAsync();
//                 using var cmd = conn.CreateCommand();
//                 cmd.CommandText = Queries.QtdPedidosDiarios;
//                 using var reader = await cmd.ExecuteReaderAsync();
//                 var produtos = new List<Produto>();
//                 while (await reader.ReadAsync())
//                 {
//                     produtos.Add(new Produto
//                     {
//                         //arrumar oque vem da tabela aqui

//                     }
//                 );
//                 }
//                 return produtos;
//             }
//         }

//         public async Task<List<Produto>> ProdutosDiariosAsync()
//         {
//             if (_connectionFactory.CreateConnection() is not DbConnection conn)
//                 throw new InvalidOperationException("Falha ao obter conexão");
//             using (conn)
//             {
//                 await conn.OpenAsync();
//                 using var cmd = conn.CreateCommand();
//                 cmd.CommandText = Queries.QtdPedidosDiarios;
//                 using var reader = await cmd.ExecuteReaderAsync();
//                 var produtos = new List<Produto>();
//                 while (await reader.ReadAsync())
//                 {
//                     produtos.Add(new Produto
//                     {
//                         //arrumar oque vem da tabela aqui

//                     }
//                 );
//                 }
//                 return produtos;
//             }
//         }


//         //==================//
//         //===Funcionarios===//
//         //==================//

//         public async Task<List<Usuario>> QtdFuncionariosMensaisAsync()
//         {
//             if (_connectionFactory.CreateConnection() is not DbConnection conn)
//                 throw new InvalidOperationException("Falha ao obter conexão");
//             using (conn)
//             {
//                 await conn.OpenAsync();
//                 using var cmd = conn.CreateCommand();
//                 cmd.CommandText = Queries.QtdPedidosDiarios;
//                 using var reader = await cmd.ExecuteReaderAsync();
//                 var funcionario = new List<Usuario>();
//                 while (await reader.ReadAsync())
//                 {
//                     funcionario.Add(new Usuario
//                     {
//                         //arrumar oque vem da tabela aqui

//                     }
//                 );
//                 }
//                 return funcionario;
//             }
//         }

//         public async Task<List<Usuario>> qtdFuncionariosDiariosAsync()
//         {
//             if (_connectionFactory.CreateConnection() is not DbConnection conn)
//                 throw new InvalidOperationException("Falha ao obter conexão");
//             using (conn)
//             {
//                 await conn.OpenAsync();
//                 using var cmd = conn.CreateCommand();
//                 cmd.CommandText = Queries.QtdPedidosDiarios;
//                 using var reader = await cmd.ExecuteReaderAsync();
//                 var funcionarios = new List<Usuario>();
//                 while (await reader.ReadAsync())
//                 {
//                     funcionarios.Add(new Usuario
//                     {
//                         //arrumar oque vem da tabela aqui

//                     }
//                 );
//                 }
//                 return funcionarios;
//             }
//         }


//         //================//
//         //===Entregador===//
//         //================//

//         public async Task<List<Usuario>> EntregadorMensaisAsync()
//         {            if (_connectionFactory.CreateConnection() is not DbConnection conn)
//                 throw new InvalidOperationException("Falha ao obter conexão");
//             using (conn)
//             {
//                 await conn.OpenAsync();
//                 using var cmd = conn.CreateCommand();
//                 cmd.CommandText = Queries.QtdPedidosDiarios;
//                 using var reader = await cmd.ExecuteReaderAsync();
//                 var entregadores = new List<Usuario>();
//                 while (await reader.ReadAsync())
//                 {
//                     entregadores.Add(new Usuario
//                     {
//                         //arrumar oque vem da tabela aqui

//                     }
//                 );
//                 }
//                 return entregadores;
//             }
//         }
//         public async Task<List<Usuario>> EntregadorDiariosAsync()
//         {
//             if (_connectionFactory.CreateConnection() is not DbConnection conn)
//                 throw new InvalidOperationException("Falha ao obter conexão");
//             using (conn)
//             {
//                 await conn.OpenAsync();
//                 using var cmd = conn.CreateCommand();
//                 cmd.CommandText = Queries.QtdPedidosDiarios;
//                 using var reader = await cmd.ExecuteReaderAsync();
//                 var entregadores = new List<Usuario>();
//                 while (await reader.ReadAsync())
//                 {
//                     entregadores.Add(new Usuario
//                     {
//                         //arrumar oque vem da tabela aqui

//                     }
//                 );
//                 }
//                 return entregadores;
//             }
//         }
//     } 
// }