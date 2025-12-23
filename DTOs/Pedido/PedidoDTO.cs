using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SaborGregoNew.Enums;
using SaborGregoNew.Models;

namespace SaborGregoNew.DTOs.Pedido;

public class PedidoDTO
{

    [Key]
    public int Id { get; set; }
    [Required]
    public DateTime DataPedido { get; set; } = DateTime.Now;
    [Required]
    public decimal ValorTotal { get; set; } = 0.0M;
    [Required]
    public StatusPedido Status { get; set; } = 0;
    [Required]
    public int EnderecoId { get; set; }
    [Required]
    public MetodoPagamento MetodoPagamento { get; set; }
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
public class DetalhePedidoDTO
{
    // Chave Estrangeira para a tabela Pedido
    [Required]
    [Key]
    [ForeignKey("PedidoId")]
    public int PedidoId { get; set; }

    // Chave Estrangeira para a tabela Produto
    [Key]
    [Required]
    [ForeignKey("ProdutoId")]
    public int ProdutoId { get; set; }

    // Snapshot dos dados do Produto no momento da compra
    [Required]
    [StringLength(100)]
    public string NomeProduto { get; set; }

    [Required]
    public int Quantidade { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PrecoUnitario { get; set; }

    // Propriedade calculada (opcional, mas útil)
    [NotMapped]
    public decimal SubTotal => Quantidade * PrecoUnitario;
}
