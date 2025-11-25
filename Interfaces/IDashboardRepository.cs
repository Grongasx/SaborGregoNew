using SaborGregoNew.DTOs; // Importe o DTO
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SaborGregoNew.Repository
{
    public interface IDashboardRepository
    {
        Task<DashboardAdminDTO> GetDashboardAdminDataAsync();
        Task<List<VendasPorDiaDTO>> GetVendasDiariasMesAsync();
        Task<List<VendasPorProdutoDTO>> GetVendasPorProdutoMesAsync();
    }
}