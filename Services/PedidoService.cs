using SaborGregoNew.Models;
using SaborGregoNew.Repository;
using SaborGregoNew.DTOs.Pedido;

namespace SaborGregoNew.Services
{
    public class PedidoService
    {
        private readonly PedidoRepository _PedidoRepository;

        public PedidoService(PedidoRepository PedidoRepository)
        {
            _PedidoRepository = PedidoRepository;
        }

        public Pedido GetById(int id)
        {
            return _PedidoRepository.GetById(id);
        }
        public async Task UpdateAsync(Pedido Pedido)
        {
            await _PedidoRepository.UpdateAsync(Pedido);
        }

        public async Task DeleteAsync(int id)
        {
            await _PedidoRepository.DeleteAsync(id);
        }
    }
}
        

            

                    

            