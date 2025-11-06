using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.DTOs.Pedido;
using SaborGregoNew.Models;
using SaborGregoNew.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SaborGregoNew.Pages.Pedido
{
    public class CadastroPedidoModel : PageModel
    {
        private readonly PedidoService _pedidoService;
        private readonly UsuarioService _usuarioService;
        private readonly ProdutoService _produtoService;

        [BindProperty]
        public CreatePedidoDTO PedidoDto { get; set; }

        public CadastroPedidoModel(PedidoService pedidoService, UsuarioService usuarioService, ProdutoService produtoService)
        {
            _pedidoService = pedidoService;
            _usuarioService = usuarioService;
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
