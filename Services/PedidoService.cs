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

        public async Task CadastrarPedido(CreatePedidoDTO dto )
        {
            var Pedido = new Pedido
            {
                DataPedido = dto.DataPedido,
                ValorTotal = dto.ValorTotal,
                Status = dto.Status,
                ClienteId = dto.ClienteId,
                FuncionarioId = dto.FuncionarioId,
                EntregadorId = dto.EntregadorId,
                Produtos = dto.Produtos
            };

            await _PedidoRepository.Create(Pedido);
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
        

            

                    

            