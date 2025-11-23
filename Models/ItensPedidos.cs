using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaborGregoNew.Models
{
    public class ItensPedido
    {
        [ForeignKey("PedidoId")]
        public int PedidoId { get; set; }
        [ForeignKey("ProdutoId")]
        public int ProdutoId { get; set; }
        [Required]
        public int Quantidade { get; set; } = 0;
    }
}