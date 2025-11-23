namespace SaborGregoNew.DTOs.Produtos;

public class CreateProdutoDTO
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; } = 0.0M;
    public string Categoria { get; set; } = string.Empty;
    public byte[] Imagem { get; set; } = new byte[0];
}