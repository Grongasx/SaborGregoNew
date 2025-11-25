using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.DTOs;
using SaborGregoNew.Repository;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaborGregoNew.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardAdminDTO? DashboardData { get; set; } = new DashboardAdminDTO();

        // CORREÇÃO AQUI: Usamos "SaborGregoNew.Models.Pedido" explicitamente
        // Isso evita a confusão com a pasta "Pages/Pedido"
        public List<SaborGregoNew.Models.Pedido> PedidosRecentes { get; set; } = new List<SaborGregoNew.Models.Pedido>();

        public IndexModel(ILogger<IndexModel> logger, IPedidoRepository pedidoRepository, IDashboardRepository dashboardRepository)
        {
            _logger = logger;
            _pedidoRepository = pedidoRepository;
            _dashboardRepository = dashboardRepository;
        }

        public async Task OnGetAsync()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                // 1. Lógica para ADMIN (Dashboard)
                if (User.IsInRole("Admin"))
                {
                    try
                    {
                        DashboardData = await _dashboardRepository.GetDashboardAdminDataAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao carregar dashboard");
                    }
                }
                // 2. Lógica para CLIENTE (Meus Pedidos)
                else
                {
                    string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (int.TryParse(userIdString, out int userId))
                    {
                        try
                        {
                            var todosPedidos = await _pedidoRepository.GetPedidosPorClienteAsync(userId);
                            
                            // Pega os 5 últimos pedidos
                            PedidosRecentes = todosPedidos.Take(5).ToList();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Erro ao carregar pedidos");
                        }
                    }
                }
            }
        }
    }
}