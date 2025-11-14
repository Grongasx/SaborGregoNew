using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Enums;
using SaborGregoNew.Models;
using saborGregoNew.Repository.Interfaces;
using SaborGregoNew.Extensions;

namespace SaborGregoNew.Pages.Funcionario // ⬅️ Namespace e Pasta alterados
{
    // 🚨 REGRA DE ACESSO: Agora usando a Role "Funcionario"
    //[Authorize(Roles = "Funcionario")] 
    public class EntregasModel : PageModel
    {
        private readonly IPedidoRepository _pedidoRepository;

        // Propriedades para exibir na página
        [BindProperty]
        public List<Pedido> PedidosProntos { get; set; } = new List<Pedido>();

        [BindProperty]
        public List<Pedido> PedidosEmRota { get; set; } = new List<Pedido>();

        public EntregasModel(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }
        
        // Método auxiliar (reutilizado do cozinheiro)
        private IActionResult TryGetUserId(out int userId)
        {
            userId = 0;
            try
            {
                userId = User.GetUserId();
                return null;
            }
            catch
            {
                TempData["MensagemErro"] = "Usuário não autenticado. Por favor, faça Login para continuar!";
                return RedirectToPage("/Usuario/Login/Login");
            }
        }
        public async Task<IActionResult> OnGetAsync()
        {
            // A lógica pública (PedidosProntos) não precisa do userId, mas o restante sim.
            if (TryGetUserId(out int userId) is IActionResult authResult)
            {
                return authResult; // Redireciona para login
            }

            try
            {
                // 1. Pedidos Prontos para Qualquer Entregador pegar (Usando método público existente)
                // PedidosProntos está acessível publicamente para qualquer funcionário.
                PedidosProntos = await _pedidoRepository.GetPedidosPublicosPorStatusAsync(StatusPedido.ProntoParaRetirada);
                
                // 2. Pedidos que estão em rota com ESTE entregador (Usando método de funcionário existente)
                PedidosEmRota = await _pedidoRepository.GetPedidosFuncionarioPorStatusAsync(StatusPedido.EmRotaDeEntrega, userId);
                
                return Page();
            }
            catch (ArgumentException ex)
            {
                TempData["MensagemErro"] = "Erro ao carregar pedidos: " + ex.Message;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAceitarAsync(int pedidoId)
        {
            if (TryGetUserId(out int userId) is IActionResult authResult)
            {
                return authResult;
            }

            try
            {
                // O método deve ser modificado no repositório para SETAR EntregadorId
                // ao invés de FuncionarioId quando o status for EmRotaDeEntrega.
                // O userId aqui é o ID do entregador.
                await _pedidoRepository.UpdateStatusByIdAsync(pedidoId, userId, StatusPedido.EmRotaDeEntrega);
                
                return RedirectToPage();
            }
            catch (ArgumentException ex)
            {
                TempData["MensagemErro"] = "Erro ao aceitar pedido: " + ex.Message;
                return RedirectToPage();
            }
        }
        // Dentro da classe DashboardModel : PageModel
        public async Task<IActionResult> OnPostEntregarAsync(int pedidoId)
        {
            if (TryGetUserId(out int userId) is IActionResult authResult)
            {
                return authResult;
            }

            try
            {
                // Apenas atualiza o status para Entregue. Nenhum ID de funcionário adicional é necessário.
                await _pedidoRepository.UpdateStatusByIdAsync(pedidoId, userId, StatusPedido.Entregue);
                
                return RedirectToPage();
            }
            catch (ArgumentException ex)
            {
                TempData["MensagemErro"] = "Erro ao finalizar pedido: " + ex.Message;
                return RedirectToPage();
            }
        }
    }
}