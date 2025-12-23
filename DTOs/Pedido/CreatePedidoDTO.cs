using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SaborGregoNew.Enums;
using SaborGregoNew.Models;

namespace SaborGregoNew.DTOs.Pedido;

public class CreatePedidoDTO
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
    public Models.Usuario? Cliente { get; set; }

    [ForeignKey("FuncionarioId")]
    public int? FuncionarioId { get; set; }
    public Models.Usuario? Funcionario { get; set; }

    [ForeignKey("EntregadorId")]
    public int? EntregadorId { get; set; }
    public Models.Usuario? Entregador { get; set; }
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
