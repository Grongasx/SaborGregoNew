using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SaborGregoNew.Enums;
using SaborGregoNew.Data;
using SaborGregoNew.Models;
using System.Security.Claims;
using Microsoft.VisualBasic;

namespace SaborGregoNew.Pages.Funcionario // ⬅️ Namespace e Pasta alterados
{
    // 🚨 REGRA DE ACESSO: Agora usando a Role "Funcionario"
    //[Authorize(Roles = "Funcionario")] 
    public class EntregasSolicitadasModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<SaborGregoNew.Models.Pedido> EntregasSolicitadas { get; set; }
        public List<SaborGregoNew.Models.Pedido> Entregando { get; set; }


        public EntregasSolicitadasModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // -----------------------------------------------------
        // MÉTODO HTTP GET: Listar Entregas Solicitadas
        // -----------------------------------------------------
        // Em Pages/Funcionario/EntregasSolicitadas.cshtml.cs
        // ...
        public async Task<IActionResult> OnGetAsync()
        {
            // 🎯 FILTRO: Buscar todos com StatusPedido.Solicitado
            // ID do funcionário logado (opcional, mas bom para filtrar o trabalho dele)
            var entregadorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Carregar Entregas Solicitadas (Fila de entrada)
            EntregasSolicitadas = await _context.Pedidos
                                    .Include(p => p.Itens)
                                    .Where(p => p.Status == StatusPedido.ProntoParaRetirada)
                                    .OrderBy(p => p.DataPedido)
                                    .ToListAsync();

            // Carregar Entregas Em Preparação (Trabalho em andamento)
            // Se você quiser mostrar APENAS os Entregas EM PREPARAÇÃO desse funcionário:
            // .Where(p => p.Status == StatusPedido.EmPreparacao && p.FuncionarioId == funcionarioId)
            // Se você quiser mostrar TODOS os Entregas Em Preparação:
            Entregando = await _context.Pedidos
                                    .Include(p => p.Itens)
                                    .Where(p => p.Status == StatusPedido.EmRotaDeEntrega &&
                                        p.EntregadorId == entregadorId)
                                    .OrderBy(p => p.DataPedido)
                                    .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostIniciarPreparoAsync(int id)
        {
            var pedido = await _context.Pedidos
                                       .FirstOrDefaultAsync(p => p.Id == id && p.Status == StatusPedido.ProntoParaRetirada);

            if (pedido == null)
            {
                TempData["ErrorMessage"] = "Pedido não encontrado ou o preparo já foi iniciado.";
                return RedirectToPage();
            }

            // 1. Atualiza o status para Em Preparo
            pedido.Status = StatusPedido.EmRotaDeEntrega; // ⬅️ Usando o Enum ajustado

            // 2. Assinala o Funcionário (se o seu modelo Pedido tiver um FuncionarioId)
            // É a melhor hora para associar o funcionário que iniciou a tarefa.
            pedido.EntregadorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Preparo do Pedido #{pedido.Id} iniciado por você.";

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostConcluirPreparoAsync(int id)
        {
            var pedido = await _context.Pedidos
                                       .FirstOrDefaultAsync(p => p.Id == id && p.Status == StatusPedido.EmRotaDeEntrega);

            if (pedido == null)
            {
                // Se o pedido não estiver em preparo, não pode ser concluído
                TempData["ErrorMessage"] = "Pedido não está com status 'Em Preparação' ou não foi encontrado.";
                return RedirectToPage();
            }
            
            // 1. Atualiza o status para Pronto para Retirada
            pedido.Status = StatusPedido.Entregue; 
            
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = $"Preparo do Pedido #{pedido.Id} CONCLUÍDO. Enviado para a fila de entrega/retirada.";
            
            return RedirectToPage();
        }
    }
}