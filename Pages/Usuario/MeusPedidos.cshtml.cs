using Microsoft.AspNetCore.Mvc.RazorPages;
using saborGregoNew.Repository.Interfaces;
using SaborGregoNew.Extensions;
using SaborGregoNew.Models;

namespace SaborGregoNew.Pages.Usuario;

public class MeusPedidosModel : PageModel
{
    private readonly IPedidoRepository _pedidoRepository;
    public List<Pedido> pedidosPendentes { get; set; }

    public MeusPedidosModel(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task OnGet()
    {
        var userId = 0;
        try
        {
            userId = User.GetUserId();
        }
        catch
        {
            TempData["MensagemErro"] = "Usuário não autenticado, Porfavor faça Login para continuar!";
            RedirectToPage("/Usuario/Login/Login");
        }
        try
        {
            pedidosPendentes = await _pedidoRepository.GetPedidosPendentesAsync(userId);
        }
        catch (Exception ex)
        { throw new Exception("Erro ao listar pedidos", ex); }
    }
}