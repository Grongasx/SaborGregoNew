using saborGregoNew.DTOs; // Importe o DTO
using System.Threading.Tasks;
using System.Collections.Generic;

namespace saborGregoNew.Repository
{
    public interface IDashboardRepository
    {
        Task<DashboardAdminDTO> GetDashboardAdminDataAsync();
        Task<List<VendasPorDiaDTO>> GetVendasDiariasMesAsync();
        Task<List<VendasPorCategoriaDTO>> GetVendasPorCategoriaMesAsync();
    }
}