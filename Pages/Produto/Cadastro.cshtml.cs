using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Repository;
using SaborGregoNew.DTOs.Produtos;

namespace SaborGregoNew.Pages.Produto
{
    public class CadastroModel : PageModel
    {
        private readonly IProdutoRepository _produtoService;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty] 
        public ProdutoDTO ProdutoDto { get; set; }

        public CadastroModel(IProdutoRepository produtoService, IWebHostEnvironment hostEnvironment)
        {
            _produtoService = produtoService;
            _hostEnvironment = hostEnvironment;
        }

        public async Task OnGet(int? id)
        {
            if (id.HasValue && id.Value > 0)
            {
                var produto = await _produtoService.SelectByIdAsync(id.Value);
                if (produto != null)
                {
                    ProdutoDto = new ProdutoDTO
                    {
                        Id = produto.Id,
                        Nome = produto.Nome,
                        Descricao = produto.Descricao ?? string.Empty,
                        Preco = produto.Preco,
                        Categoria = produto.Categoria,
                        Imagem = string.Empty 
                    };
                }
                else
                {
                    ProdutoDto = new ProdutoDTO();
                }
            }
            else
            {
                ProdutoDto = new ProdutoDTO();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("O modelo nao e valido");
                return Page();
            }

            if (ProdutoDto.ImagemUpload != null)
            {
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
            }

            try
            {
                Console.WriteLine("Cadastro de produto");
                if (ProdutoDto.Id > 0)
                {
                     await _produtoService.UpdateById(ProdutoDto.Id, ProdutoDto);
                }
                else
                {
                    await _produtoService.Create(ProdutoDto);
                }
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cadastro de produto nao foi: " + ex.ToString());
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar o produto.");
                return Page();
            }
        }
    }
}