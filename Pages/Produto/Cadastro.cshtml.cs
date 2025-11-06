using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.DTOs.Produtos;
using SaborGregoNew.Models;
using SaborGregoNew.Services;

namespace SaborGregoNew.Pages.Produto
{
    public class CadastroModel : PageModel
    {
        private readonly ProdutoService _produtoService;

        [BindProperty] 
        public CreateProdutoDTO ProdutoDto { get; set; }

        public CadastroModel(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("O modelo nao e valido");
                return Page();
            }

            try
            {
                Console.WriteLine("Cadastro de produto");
                await _produtoService.Create(ProdutoDto);
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cadastro de produto nao foi");
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar o produto.");
                return Page();
            }
        }
    }
}