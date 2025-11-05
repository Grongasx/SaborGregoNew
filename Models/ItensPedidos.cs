using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaborGregoNew.Models
{
    public class ItensPedido
    {
        [Key]
        [ForeignKey("PedidoId")]
        public int PedidoId { get; set; }
        [Key]
        [ForeignKey("ProdutoId")]
        public int ProdutoId { get; set; }
        [Required]
        public int Quantidade { get; set; } = 0;
    }
}