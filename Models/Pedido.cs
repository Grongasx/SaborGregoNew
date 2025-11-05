using System.ComponentModel.DataAnnotations;
using SaborGregoNew.Models;

namespace SaborGregoNew.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DataPedido { get; set; } = DateTime.Now;
        [Required]
        public decimal ValorTotal { get; set; } = 0.0M;
        [Required]
        public string Status { get; set; } = string.Empty;
        [Required]
        public int ClienteId { get; set; }
        public Usuario? Cliente { get; set; }

        public int? FuncionarioId { get; set; }
        public Usuario? Funcionario { get; set; }

        public int? EntregadorId { get; set; }
        public Usuario? Entregador { get; set; }
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}