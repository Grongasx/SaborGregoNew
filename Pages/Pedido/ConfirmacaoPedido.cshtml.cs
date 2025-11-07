using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SaborGregoNew.Data;
using SaborGregoNew.Models;
using SaborGregoNew.Services;
using System.Security.Claims;


namespace SaborGregoNew.Pages
{
    // Apenas usuários logados podem ver a confirmação de um pedido
    [Authorize] 
    public class ConfirmacaoPedidoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly EnderecoService _enderecoService;


        // Propriedade para vincular e exibir o pedido na View
        public Pedido Pedido { get; set; }

        public ConfirmacaoPedidoModel(ApplicationDbContext context, EnderecoService enderecoService)
        {
            _context = context;
            _enderecoService = enderecoService;
        }

        // O método OnGet receberá o 'id' do Pedido via query string (URL)
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // 1. Verificar Autenticação (redundante, mas seguro)
            var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioIdString))
            {
                return RedirectToPage("/Usuario/Login/Login");
            }
            var clienteId = int.Parse(usuarioIdString);

            // 2. Buscar o Pedido e seus Itens
            // Inclui: Carrega os DetalhesPedido (Itens) e verifica se pertence ao ClienteId
            Pedido = await _context.Pedidos
                                   .Include(p => p.Itens) // Carrega os detalhes do pedido
                                   .FirstOrDefaultAsync(p => p.Id == id && p.ClienteId == clienteId);

            if (Pedido == null)
            {
                // Se o pedido não existir ou não pertencer ao usuário logado
                return RedirectToPage("/Index"); // Ou NotFound()
            }

            return Page();
        }
    }
}