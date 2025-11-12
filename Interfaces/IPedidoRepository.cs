using System.Data.Common;
using SaborGregoNew.DTOs.Pedido;
using SaborGregoNew.Enums;
using SaborGregoNew.Models;

namespace saborGregoNew.Repository.Interfaces
{
    public interface IPedidoRepository
    {
        Task CriarPedidoCompletoAsync(PedidoDTO ModeloPedido, IEnumerable<DetalhePedido> detalhes);
        Task UpdateStatusByIdAsync(int id, StatusPedido status);
        Task<List<Pedido>> GetPedidosFluxoTrabalhoAsync(StatusPedido status, int usuarioId);
        Task<List<Pedido>> GetPedidosPendentesAsync(int usuarioId);
    }
}