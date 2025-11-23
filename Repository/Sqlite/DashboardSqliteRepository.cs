using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using SaborGregoNew.DTOs;

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

            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão SQLite");

            using (conn)
            {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand())
                {
                    // 1. Vendas Hoje (Usando a Query Centralizada)
                    cmd.CommandText = Queries.DashboardVendasHoje;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            dto.VendasHoje = reader.GetDecimal(reader.GetOrdinal("VendasHoje"));
                            dto.PedidosHoje = reader.GetInt32(reader.GetOrdinal("PedidosHoje"));
                        }
                    }

                    // 2. Vendas Mês
                    cmd.CommandText = Queries.DashboardVendasMes;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            dto.VendasMes = reader.GetDecimal(reader.GetOrdinal("VendasMes"));
                            dto.PedidosMes = reader.GetInt32(reader.GetOrdinal("PedidosMes"));
                        }
                    }
                
                    // 3. Produto Hoje
                    cmd.CommandText = Queries.DashboardProdutoMaisVendidoHoje;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            dto.ProdutoMaisVendidoHoje = reader.GetString(reader.GetOrdinal("Nome"));
                        }
                    }
                
                    // 4. Produto Mês
                    cmd.CommandText = Queries.DashboardProdutoMaisVendidoMes;
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

            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão SQLite");

            using (conn)
            {
                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    // Usando a Query Centralizada
                    cmd.CommandText = Queries.DashboardVendasDiarias;
                    
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

        public async Task<List<VendasPorProdutoDTO>> GetVendasPorProdutoMesAsync()
        {
            var resultado = new List<VendasPorProdutoDTO>();

            if (_connectionFactory.CreateSqliteConnection() is not DbConnection conn)
                throw new InvalidOperationException("Falha ao obter conexão SQLite");

            using (conn)
            {
                await conn.OpenAsync();
                using (var cmd = conn.CreateCommand())
                {
                    // Usando a Query Centralizada
                    cmd.CommandText = Queries.DashboardVendasPorProduto;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            resultado.Add(new VendasPorProdutoDTO
                            {
                                Produto = reader.GetString(reader.GetOrdinal("NomeProduto")),
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