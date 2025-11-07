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
    public class PedidosSolicitadosModel : PageModel
    {
        private readonly ApplicationDbContext _contextDb;

        public List<Pedido> PedidosSolicitados { get; set; }
        public List<Pedido> PedidosEmPreparacao { get; set; }


        public PedidosSolicitadosModel(ApplicationDbContext context)
        {
            _contextDb = context;
        }

        // -----------------------------------------------------
        // MÉTODO HTTP GET: Listar Pedidos Solicitados
        // -----------------------------------------------------
        // Em Pages/Funcionario/PedidosSolicitados.cshtml.cs
        // ...
        public async Task<IActionResult> OnGetAsync()
        {
            // 🎯 FILTRO: Buscar todos com StatusPedido.Solicitado
            // ID do funcionário logado (opcional, mas bom para filtrar o trabalho dele)
            var funcionarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Carregar Pedidos Solicitados (Fila de entrada)
            PedidosSolicitados = await _contextDb.Pedidos
                                    .Include(p => p.Itens)
                                    .Where(p => p.Status == StatusPedido.Solicitado)
                                    .OrderBy(p => p.DataPedido)
                                    .ToListAsync();

            // Carregar Pedidos Em Preparação (Trabalho em andamento)
            // Se você quiser mostrar APENAS os pedidos EM PREPARAÇÃO desse funcionário:
            // .Where(p => p.Status == StatusPedido.EmPreparacao && p.FuncionarioId == funcionarioId)
            // Se você quiser mostrar TODOS os pedidos Em Preparação:
            PedidosEmPreparacao = await _contextDb.Pedidos
                                    .Include(p => p.Itens)
                                    .Where(p => p.Status == StatusPedido.EmPreparacao &&
                                        p.FuncionarioId == funcionarioId)
                                    .OrderBy(p => p.DataPedido)
                                    .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostIniciarPreparoAsync(int id)
        {
            var pedido = await _contextDb.Pedidos
                                       .FirstOrDefaultAsync(p => p.Id == id && p.Status == StatusPedido.Solicitado);

            if (pedido == null)
            {
                TempData["ErrorMessage"] = "Pedido não encontrado ou o preparo já foi iniciado.";
                return RedirectToPage();
            }

            // 1. Atualiza o status para Em Preparo
            pedido.Status = StatusPedido.EmPreparacao; // ⬅️ Usando o Enum ajustado

            // 2. Assinala o Funcionário (se o seu modelo Pedido tiver um FuncionarioId)
            // É a melhor hora para associar o funcionário que iniciou a tarefa.
            pedido.FuncionarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            await _contextDb.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Preparo do Pedido #{pedido.Id} iniciado por você.";

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostConcluirPreparoAsync(int id)
        {
            var pedido = await _contextDb.Pedidos
                                       .FirstOrDefaultAsync(p => p.Id == id && p.Status == StatusPedido.EmPreparacao);

            if (pedido == null)
            {
                // Se o pedido não estiver em preparo, não pode ser concluído
                TempData["ErrorMessage"] = "Pedido não está com status 'Em Preparação' ou não foi encontrado.";
                return RedirectToPage();
            }
            
            // 1. Atualiza o status para Pronto para Retirada
            pedido.Status = StatusPedido.ProntoParaRetirada; 
            
            await _contextDb.SaveChangesAsync();
            
            TempData["SuccessMessage"] = $"Preparo do Pedido #{pedido.Id} CONCLUÍDO. Enviado para a fila de entrega/retirada.";
            
            return RedirectToPage();
        }
    }
}