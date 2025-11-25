using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Models;
using SaborGregoNew.Extensions;
using SaborGregoNew.DTOs.Pedido;
using SaborGregoNew.Repository;

namespace SaborGregoNew.Pages
{
    public class ConfirmacaoPedidoModel : PageModel
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public ConfirmacaoPedidoModel(IPedidoRepository pedidoRepository, IEnderecoRepository enderecoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _enderecoRepository = enderecoRepository;
        }

        public SaborGregoNew.Models.Pedido? pedido { get; set; }
        public Endereco? endereco { get; set; }
        public List<DetalhePedido>? detalhesPedido { get; set; }
        public decimal CarrinhoTotal { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // CENÁRIO 1: Acesso via Histórico (DB)
            if (id.HasValue && id > 0)
            {
                pedido = await _pedidoRepository.GetPedidoByIdAsync(id.Value);
                
                if (pedido != null)
                {
                    endereco = await _enderecoRepository.SelectByIdAsync(pedido.EnderecoId);
                    detalhesPedido = pedido.Itens.ToList();
                    return Page();
                }
            }

            var session = HttpContext.Session;
            var pedidoDto = session.GetObjectFromJson<PedidoDTO>("PedidoConfirmacao");
            var enderecotemp = session.GetObjectFromJson<Endereco>("EnderecoConfirmacao");
            var detalhesPedidotemp = session.GetObjectFromJson<List<DetalhePedido>>("DetalhesConfirmacao");
            CarrinhoTotal = session.GetObjectFromJson<decimal>("CarrinhoTotal");

            session.Remove("PedidoConfirmacao");
            session.Remove("EnderecoConfirmacao");
            session.Remove("DetalhesConfirmacao");

            if (pedidoDto != null && enderecotemp != null && detalhesPedidotemp != null)
            {
                pedido = new SaborGregoNew.Models.Pedido
                {
                    Id = pedidoDto.Id,
                    DataPedido = pedidoDto.DataPedido,
                    TotalPedido = pedidoDto.ValorTotal,
                    Status = pedidoDto.Status,
                    MetodoPagamento = pedidoDto.MetodoPagamento,
                    ClienteId = pedidoDto.ClienteId,
                    EnderecoId = pedidoDto.EnderecoId,
                    Itens = detalhesPedidotemp
                };
                endereco = enderecotemp;
                detalhesPedido = detalhesPedidotemp;
                
                return Page();
            }
            return RedirectToPage("/Pedido/Carrinho/Carrinho");
        }
    }
}