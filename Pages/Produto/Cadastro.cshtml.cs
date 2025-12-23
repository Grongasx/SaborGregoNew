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
                        Imagem = produto.Imagem ?? string.Empty 
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
            // Remove validação automática da imagem (nós tratamos manualmente)
            ModelState.Remove("ProdutoDto.Imagem");
            ModelState.Remove("ProdutoDto.ImagemUpload");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // 1. Lógica de Upload (Se o usuário enviou um arquivo novo)
            if (ProdutoDto.ImagemUpload != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string pathDaImagem = Path.Combine(wwwRootPath, "images");
                
                if (!Directory.Exists(pathDaImagem)) Directory.CreateDirectory(pathDaImagem);

                string nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(ProdutoDto.ImagemUpload.FileName);
                string filePath = Path.Combine(pathDaImagem, nomeArquivo);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ProdutoDto.ImagemUpload.CopyToAsync(fileStream);
                }
                ProdutoDto.Imagem = "/images/" + nomeArquivo;
            }
            // 2. Lógica de Recuperação (Se é edição e NÃO enviou foto nova)
            else if (ProdutoDto.Id > 0)
            {
                if (string.IsNullOrEmpty(ProdutoDto.Imagem))
                {
                    var produtoOriginal = await _produtoService.SelectByIdAsync(ProdutoDto.Id);
                    if (produtoOriginal != null)
                    {
                        ProdutoDto.Imagem = produtoOriginal.Imagem;
                    }
                }
            }

            if (string.IsNullOrEmpty(ProdutoDto.Imagem))
            {
                ModelState.AddModelError("ProdutoDto.ImagemUpload", "A imagem é obrigatória.");
                return Page();
            }

            try
            {
                if (ProdutoDto.Id > 0)
                {
                     await _produtoService.UpdateById(ProdutoDto.Id, ProdutoDto);
                }
                else
                {
                    await _produtoService.Create(ProdutoDto);
                }
                return RedirectToPage("/Produto/Cardapio");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Erro ao salvar: " + ex.Message);
                return Page();
            }
        }
    }
}