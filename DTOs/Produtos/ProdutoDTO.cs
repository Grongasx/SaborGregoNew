namespace SaborGregoNew.DTOs.Produtos;

public class ProdutoDTO
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; } = 0.0M;
    public string Categoria { get; set; } = string.Empty;
    public IFormFile? ImagemUpload { get; set; }
    public string Imagem { get; set; } = string.Empty;
}