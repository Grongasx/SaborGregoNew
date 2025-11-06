
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Models;
using SaborGregoNew.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaborGregoNew.Pages
{
    public class CardapioModel : PageModel
    {
        private readonly ProdutoService _produtoService;

        public CardapioModel(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        public List<Models.Produto> Produtos { get; set; }

        public async Task OnGetAsync()
        {
            Produtos = await _produtoService.GetAllAsync();
        }
    }
}
