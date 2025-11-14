using System.Data.Common;
using SaborGregoNew.DTOs.Pedido;
using SaborGregoNew.Enums;
using SaborGregoNew.Models;

namespace saborGregoNew.Repository.Interfaces
{
    public interface IPedidoRepository
    {
        Task CriarPedidoCompletoAsync(PedidoDTO ModeloPedido, IEnumerable<DetalhePedido> detalhes);
        Task UpdateStatusByIdAsync(int id,int FuncionarioId, StatusPedido status);
        Task<List<Pedido>> GetPedidosPublicosPorStatusAsync(StatusPedido status);
        Task<List<Pedido>> GetPedidosFuncionarioPorStatusAsync(StatusPedido status, int usuarioId);
        Task<List<Pedido>> GetPedidosPendentesAsync(int usuarioId);
    }
}