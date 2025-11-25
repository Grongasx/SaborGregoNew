using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.DTOs;
using SaborGregoNew.Repository;

namespace SaborGregoNew.Pages.Usuario.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly IDashboardRepository _dashboardRepo;

        public DashboardAdminDTO DashboardData { get; set; }
        
        // Armazena os dados do gráfico como JSON serializado
        public string VendasDiariasJson { get; private set; } = "{}";
        public string VendasProdutosJson { get; private set; } = "{}";

        public DashboardModel(IDashboardRepository dashboardRepo)
        {
            _dashboardRepo = dashboardRepo;
            DashboardData = new DashboardAdminDTO();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Busca os dados dos cards (código existente)
            DashboardData = await _dashboardRepo.GetDashboardAdminDataAsync();

            // 1. Buscar os dados para os gráficos
            var vendasDiarias = await _dashboardRepo.GetVendasDiariasMesAsync();
            var vendasProdutos = await _dashboardRepo.GetVendasPorProdutoMesAsync();
            // Gráfico de Linha (Vendas Diárias)
            var vendasDiariasChart = new
            {
                labels = vendasDiarias.Select(v => v.Dia), // Eixo X
                datasets = new[]
                {
                    new
                    {
                        label = "Vendas Diárias (R$)",
                        data = vendasDiarias.Select(v => v.Total), // Eixo Y
                        backgroundColor = "rgba(0, 123, 255, 0.5)",
                        borderColor = "rgba(0, 123, 255, 1)",
                        borderWidth = 1,
                        fill = true
                    }
                }
            };

            // Gráfico de Pizza (Vendas por Produto)
            var vendasProdutosChart = new
            {
                labels = vendasProdutos.Select(v => v.Produto),
                datasets = new[]
                {
                    new
                    {
                        label = "Vendas por Categoria (R$)",
                        data = vendasProdutos.Select(v => v.Total),
                        backgroundColor = new[] // Cores para cada fatia
                        {
                            "rgba(255, 99, 132, 0.7)",
                            "rgba(54, 162, 235, 0.7)",
                            "rgba(255, 206, 86, 0.7)",
                            "rgba(75, 192, 192, 0.7)",
                            "rgba(153, 102, 255, 0.7)",
                            "rgba(255, 159, 64, 0.7)",
                            "rgba(199, 199, 199, 0.7)",
                            "rgba(83, 102, 255, 0.7)",
                            "rgba(40, 159, 64, 0.7)",
                            "rgba(210, 99, 132, 0.7)"
                        }
                    }
                }
            };

            // 3. Serializar os dados como JSON
            VendasDiariasJson = JsonSerializer.Serialize(vendasDiariasChart);
            VendasProdutosJson = JsonSerializer.Serialize(vendasProdutosChart);

            return Page();
        }
    }
}