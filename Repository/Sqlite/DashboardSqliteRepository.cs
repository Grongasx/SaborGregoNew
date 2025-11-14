using saborGregoNew.DTOs;
using saborGregoNew.Repository;
using System.Data.Common;
using System.Threading.Tasks;
using System;
using System.Data;
using System.Collections.Generic;

namespace SaborGregoNew.Repository
{
    public class DashboardSqliteRepository : IDashboardRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DashboardSqliteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<DashboardAdminDTO> GetDashboardAdminDataAsync()
        {
            var dto = new DashboardAdminDTO();
            const string queryHoje = @"
                SELECT 
                    COALESCE(SUM(TotalPedido), 0) AS VendasHoje,
                    COUNT(Id) AS PedidosHoje
                FROM Pedidos
                WHERE DATE(DataPedido) = DATE('now', 'localtime');";
            
            const string queryMes = @"
                SELECT 
                    COALESCE(SUM(TotalPedido), 0) AS VendasMes,
                    COUNT(Id) AS PedidosMes
                FROM Pedidos
                WHERE STRFTIME('%Y-%m', DataPedido) = STRFTIME('%Y-%m', 'now', 'localtime');";

            const string queryProdutoHoje = @"
                SELECT 
                    p.Nome, 
                    SUM(dp.Quantidade) AS QtdTotal
                FROM DetalhesPedido AS dp
                JOIN Produtos AS p ON dp.ProdutoId = p.Id
                JOIN Pedidos AS pe ON dp.PedidoId = pe.Id
                WHERE DATE(pe.DataPedido) = DATE('now', 'localtime')
                GROUP BY p.Nome
                ORDER BY QtdTotal DESC
                LIMIT 1;";
            
            const string queryProdutoMes = @"
                SELECT 
                    p.Nome, 
                    SUM(dp.Quantidade) AS QtdTotal
                FROM DetalhesPedido AS dp
                JOIN Produtos AS p ON dp.ProdutoId = p.Id
                JOIN Pedidos AS pe ON dp.PedidoId = pe.Id
                WHERE STRFTIME('%Y-%m', pe.DataPedido) = STRFTIME('%Y-%m', 'now', 'localtime')
                GROUP BY p.Nome
                ORDER BY QtdTotal DESC
                LIMIT 1;";


            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão SQLite");

            using (conn)
            {
                await conn.OpenAsync();
                // 1. Buscar Vendas/Pedidos de HOJE
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = queryHoje;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            dto.VendasHoje = reader.GetDecimal(reader.GetOrdinal("VendasHoje"));
                            dto.PedidosHoje = reader.GetInt32(reader.GetOrdinal("PedidosHoje"));
                        }
                    }
                }

                // 2. Buscar Vendas/Pedidos do MÊS
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = queryMes;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            dto.VendasMes = reader.GetDecimal(reader.GetOrdinal("VendasMes"));
                            dto.PedidosMes = reader.GetInt32(reader.GetOrdinal("PedidosMes"));
                        }
                    }
                }
                
                // 3. Buscar Produto mais vendido HOJE
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = queryProdutoHoje;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            dto.ProdutoMaisVendidoHoje = reader.GetString(reader.GetOrdinal("Nome"));
                        }
                    }
                }
                
                // 4. Buscar Produto mais vendido MÊS
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = queryProdutoMes;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            dto.ProdutoMaisVendidoMes = reader.GetString(reader.GetOrdinal("Nome"));
                        }
                    }
                }
            }
            return dto;
        }
        
        public async Task<List<VendasPorDiaDTO>> GetVendasDiariasMesAsync()
        {
            var resultado = new List<VendasPorDiaDTO>();
            
            // Agrupa as vendas por dia para o mês atual
            const string query = @"
                SELECT 
                    STRFTIME('%d/%m', DataPedido) AS Dia, 
                    SUM(TotalPedido) AS Total
                FROM Pedidos
                WHERE STRFTIME('%Y-%m', DataPedido) = STRFTIME('%Y-%m', 'now', 'localtime')
                GROUP BY DATE(DataPedido)
                ORDER BY DATE(DataPedido);";

            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão SQLite");

            using (conn)
            {
                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            resultado.Add(new VendasPorDiaDTO
                            {
                                Dia = reader.GetString(reader.GetOrdinal("Dia")),
                                Total = reader.GetDecimal(reader.GetOrdinal("Total"))
                            });
                        }
                    }
                }
            }
            return resultado;
        }

        public async Task<List<VendasPorCategoriaDTO>> GetVendasPorCategoriaMesAsync()
        {
            var resultado = new List<VendasPorCategoriaDTO>();

            // Junta Pedidos, DetalhesPedido e Produtos para
            // agrupar o total vendido por categoria no mês atual.
            const string query = @"
                SELECT 
                    p.Categoria, 
                    SUM(dp.PrecoUnitario * dp.Quantidade) AS Total
                FROM DetalhesPedido AS dp
                JOIN Produtos AS p ON dp.ProdutoId = p.Id
                JOIN Pedidos AS pe ON dp.PedidoId = pe.Id
                WHERE STRFTIME('%Y-%m', pe.DataPedido) = STRFTIME('%Y-%m', 'now', 'localtime')
                GROUP BY p.Categoria
                ORDER BY Total DESC;";

            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão SQLite");

            using (conn)
            {
                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            resultado.Add(new VendasPorCategoriaDTO
                            {
                                Categoria = reader.GetString(reader.GetOrdinal("Categoria")),
                                Total = reader.GetDecimal(reader.GetOrdinal("Total"))
                            });
                        }
                    }
                }
            }
            return resultado;
        }
    }
}