using SaborGregoNew.DTOs.Pedido;
using SaborGregoNew.Models;

namespace saborGregoNew.Repository
{
    public interface IPedidoRepository
    {
        Task Create(PedidoDTO ModeloPedido);

        Task UpdateStatusById(int id, PedidoDTO ModeloPedido);

        Task AddDetalhesAsync(IEnumerable<DetalhePedido> detalhes);
        Task<Pedido?> SelectByIdAsync(int id);
    }
}