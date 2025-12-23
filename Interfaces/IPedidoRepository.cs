using System.Data.Common;
using SaborGregoNew.DTOs.Pedido;
using SaborGregoNew.Enums;
using SaborGregoNew.Models;

namespace SaborGregoNew.Repository
{
    public interface IPedidoRepository
    {
        Task CriarPedidoCompletoAsync(PedidoDTO ModeloPedido, IEnumerable<DetalhePedido> detalhes);
        Task<Pedido?> GetPedidoByIdAsync(int id);
        Task UpdateStatusByIdAsync(int id, StatusPedido status);
        Task<List<Pedido>> GetPedidosFluxoTrabalhoAsync(StatusPedido status, int usuarioId);
        Task<List<Pedido>> GetPedidosPendentesAsync(int usuarioId);
        Task<List<Pedido>> GetPedidosPorClienteAsync(int usuarioId);
        Task UpdateStatusAndAssignAsync(int id, StatusPedido status, int usuarioId);
    }
}