using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using SaborGregoNew.Models;
using SaborGregoNew.Extensions;
using SaborGregoNew.DTOs.Pedido;

namespace SaborGregoNew.Pages
{
    // Apenas usuários logados podem ver a confirmação de um pedido
    public class ConfirmacaoPedidoModel : PageModel
    {

        //modelos para receber as viewbags
        public SaborGregoNew.Models.Pedido? pedido { get; set; }
        public Endereco? endereco { get; set; }
        public List<DetalhePedido>? detalhesPedido { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var session = HttpContext.Session;

            // 1. Ler como DTO (pois foi assim que salvamos no Checkout)
            var pedidoDto = session.GetObjectFromJson<PedidoDTO>("PedidoConfirmacao");
            var enderecotemp = session.GetObjectFromJson<Endereco>("EnderecoConfirmacao");
            var detalhesPedidotemp = session.GetObjectFromJson<List<DetalhePedido>>("DetalhesConfirmacao");

            // Limpa a sessão
            session.Remove("PedidoConfirmacao");
            session.Remove("EnderecoConfirmacao");
            session.Remove("DetalhesConfirmacao");

            if (pedidoDto == null || enderecotemp == null || detalhesPedidotemp == null)
            {
                return RedirectToPage("/Pedido/Carrinho/Carrinho");
            }

            // 2. Converter DTO para Model manualmente para garantir que os dados batam
            pedido = new SaborGregoNew.Models.Pedido
            {
                Id = pedidoDto.Id,
                DataPedido = pedidoDto.DataPedido,
                TotalPedido = pedidoDto.ValorTotal, // CORREÇÃO: Mapeia ValorTotal (DTO) para TotalPedido (Model)
                Status = pedidoDto.Status,
                MetodoPagamento = pedidoDto.MetodoPagamento,
                ClienteId = pedidoDto.ClienteId,
                EnderecoId = pedidoDto.EnderecoId,
                
                // 3. A MÁGICA: Colocar a lista de detalhes dentro do pedido
                Itens = detalhesPedidotemp 
            };

            endereco = enderecotemp;
            detalhesPedido = detalhesPedidotemp; // Mantemos aqui também por garantia
            
            return Page();
        }
    }
}