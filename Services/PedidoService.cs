using SaborGregoNew.Models;

using SaborGregoNew.DTOs.Pedido;


using SaborGregoNew.Enums;
using SaborGregoNew.Repository;

namespace SaborGregoNew.Services
{
    public class PedidoService
    {
        private readonly IPedidoRepository _PedidoRepository;

        public PedidoService(IPedidoRepository PedidoRepository)
        {
            _PedidoRepository = PedidoRepository;
        }

        public async Task CadastrarPedido(CreatePedidoDTO dto)
        {
            // Note: This should use CriarPedidoCompletoAsync with DTOs
            // For now, simplified version
            throw new NotImplementedException("Use CriarPedidoCompletoAsync instead");
        }
        
        public async Task UpdateStatusAsync(int id, StatusPedido status)
        {
            await _PedidoRepository.UpdateStatusByIdAsync(id, status);
        }

        public async Task<List<Pedido>> GetPedidosPorClienteAsync(int usuarioId)
        {
            return await _PedidoRepository.GetPedidosPorClienteAsync(usuarioId);
        }
    }
}
