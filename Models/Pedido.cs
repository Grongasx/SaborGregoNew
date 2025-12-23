using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SaborGregoNew.Enums;

namespace SaborGregoNew.Models
{
    public class Pedido
    {
        // Chave Primária
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; } // Renomeado de UsuarioId para ClienteId

        
        public int? FuncionarioId { get; set; } // O ponto de interrogação indica que é anulável (opcional)

         
        public int? EntregadorId { get; set; } // O ponto de interrogação indica que é anulável (opcional)
        
        // Dados do Pedido
        [Required]
        public DateTime DataPedido { get; set; }
        
        [Required]
        public decimal TotalPedido { get; set; }
        
        [Required]
        public StatusPedido Status { get; set; } 
        
        // Dados de Entrega e Pagamento
        [Required]
        public int EnderecoId { get; set; }
        
        [Required]
        public MetodoPagamento MetodoPagamento { get; set; }
        
        // Propriedade de Navegação para os Itens do Pedido (relação 1-para-muitos)
        public ICollection<DetalhePedido> Itens { get; set; } = new List<DetalhePedido>();
    }

    public class DetalhePedido
    {
        // Chave Estrangeira para a tabela Pedido
        [Required]
        [ForeignKey("PedidoId")]
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }

        // Chave Estrangeira para a tabela Produto
        [Required]
        [ForeignKey("ProdutoId")]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        // Snapshot dos dados do Produto no momento da compra
        [Required]
        [StringLength(100)]
        public string NomeProduto { get; set; } = string.Empty;
        public string? Imagem { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecoUnitario { get; set; }

        // Propriedade calculada (opcional, mas útil)
        [NotMapped]
        public decimal SubTotal => Quantidade * PrecoUnitario;
    }
} 