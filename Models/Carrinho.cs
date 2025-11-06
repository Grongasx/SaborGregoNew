using System.ComponentModel.DataAnnotations;
using SaborGregoNew.Enums;

namespace SaborGregoNew.Models
{
    public class Carrinho
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string SessionId { get; set; } = string.Empty;
        public ICollection<CarrinhoItem> Produtos { get; set; } = new List<CarrinhoItem>();
        [Required]
        public decimal Total { get; set; } = 0.0M;
        
        [Required]
        public FormaPagamento? FormaPagamento { get; set; }
    }
    public class CarrinhoItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CarrinhoId { get; set; }
        public Carrinho? Carrinho { get; set; }
        [Required]
        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }
        [Required]
        public int Quantidade { get; set; } = 1;
        [Required]
        public decimal Preco { get; set; } = 0.0M;
    }
}