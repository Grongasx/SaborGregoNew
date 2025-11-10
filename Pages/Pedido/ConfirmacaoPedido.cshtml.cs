using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SaborGregoNew.Data;
using SaborGregoNew.Models;
using System.Security.Claims;
using saborGregoNew.Repository;


namespace SaborGregoNew.Pages
{
    // Apenas usuários logados podem ver a confirmação de um pedido
    [Authorize] 
    public class ConfirmacaoPedidoModel : PageModel
    {
        private readonly IEnderecoRepository _enderecoService;
        private readonly IPedidoRepository _pedidoRepository;

        // Propriedade para vincular e exibir o pedido na View
        public Pedido pedido { get; set; }
        public Endereco endereco { get; set; }

        public ConfirmacaoPedidoModel(IEnderecoRepository enderecoService, IPedidoRepository pedidoRepository)
        {
            _enderecoService = enderecoService;
            _pedidoRepository = pedidoRepository;
        }

        // O método OnGet receberá o 'id' do Pedido via query string (URL)
        public async Task<IActionResult> OnGetAsync(int pedidoid, int enderecoid)
        {
            // 1. Verificar Autenticação (redundante, mas seguro)
            var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioIdString))
            {
                return RedirectToPage("/Usuario/Login/Login");
            }
            var clienteId = int.Parse(usuarioIdString);


            pedido = await _pedidoRepository.SelectByIdAsync(pedidoid);
            endereco = await _enderecoService.SelectByIdAsync(enderecoid);
            if (pedido == null)
            {
                TempData["MensagemErro"] = "O Pedido não foi encontrado";
                return RedirectToPage("/Index"); // Ou NotFound()
            }
                
            if (endereco == null)
            {
                TempData["MensagemErro"] = "O Endereço não foi encontrado";
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}