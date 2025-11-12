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
        public Pedido? pedido { get; set; }
        public Endereco? endereco { get; set; }
        public List<DetalhePedido>? detalhesPedido { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {

           var session = HttpContext.Session;

            var pedidotemp = session.GetObjectFromJson<Pedido>("PedidoConfirmacao");
            var enderecotemp = session.GetObjectFromJson<Endereco>("EnderecoConfirmacao");
            var detalhesPedidotemp = session.GetObjectFromJson<List<DetalhePedido>>("DetalhesConfirmacao");

            // ⭐️ Remover os dados da SESSÃO IMEDIATAMENTE após a leitura ⭐️
            session.Remove("PedidoConfirmacao");
            session.Remove("EnderecoConfirmacao");
            session.Remove("DetalhesConfirmacao");

            // ⚠️ Nota: PedidoDTO deve ser o tipo correto que você está salvando (ou Pedido)
            if (pedidotemp == null || enderecotemp == null || detalhesPedidotemp == null || detalhesPedidotemp.Count == 0)
            {
                // Se falhar, redireciona para o carrinho (Sem usar TempData, limpa a mensagem aqui)
                // session.Remove() já foi feito, então o Redirect é seguro.
                return RedirectToPage("/Pedido/Carrinho/Carrinho");
            }

            // Atribui os modelos para exibição na página
            pedido = (Pedido)pedidotemp; // Ajuste o casting se PedidoDTO não for Pedido
            endereco = enderecotemp;
            detalhesPedido = detalhesPedidotemp;
            
            return Page();
        }
    }
}