using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.DTOs.Produtos;
using SaborGregoNew.Services;


namespace SaborGregoNew.Pages.Produto
{
    public class CadastroModel : PageModel
    {
        private readonly ProdutoService _produtoService;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty] 
        public CreateProdutoDTO ProdutoDto { get; set; }

        public CadastroModel(ProdutoService produtoService, IWebHostEnvironment hostEnvironment)
        {
            _produtoService = produtoService;
            _hostEnvironment = hostEnvironment;
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
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string pathDaImagem = Path.Combine(wwwRootPath, "images");
            string nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(ProdutoDto.ImagemUpload.FileName);
            string filePath = Path.Combine(pathDaImagem, nomeArquivo);
            Directory.CreateDirectory(pathDaImagem);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ProdutoDto.ImagemUpload.CopyToAsync(fileStream);
            }
            ProdutoDto.Imagem = "/images/" + nomeArquivo;

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