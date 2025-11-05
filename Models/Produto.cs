using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaborGregoNew.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; } = string.Empty;
        [Required]
        public decimal Preco { get; set; } = 0.0M;
        [Required]
        public string Categoria { get; set; } = string.Empty;
        [ForeignKey("Pedido")]
        public int PedidoId { get; set; }
        public string? Descricao { get; set; }
    }
}