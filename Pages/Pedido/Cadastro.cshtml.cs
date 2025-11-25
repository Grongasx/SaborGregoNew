using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.DTOs.Pedido;
using SaborGregoNew.Models;
using SaborGregoNew.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaborGregoNew.Repository;

namespace SaborGregoNew.Pages.Pedido
{
    public class CadastroPedidoModel : PageModel
    {
        private readonly PedidoService _pedidoService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ProdutoService _produtoService;

        [BindProperty]
        public CreatePedidoDTO PedidoDto { get; set; }

        public CadastroPedidoModel(PedidoService pedidoService, IUsuarioRepository usuarioRepository, ProdutoService produtoService)
        {
            _pedidoService = pedidoService;
            _usuarioRepository = usuarioRepository;
            _produtoService = produtoService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public SelectList Clientes { get; set; }
        public List<Models.Produto> Produtos { get; set; }
    }
}
