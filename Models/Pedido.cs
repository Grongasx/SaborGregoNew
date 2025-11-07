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

        // 1. CHAVE OBRIGATÓRIA: ID do Cliente
        /// <summary>
        /// ID do usuário que fez o pedido (o Cliente). Obrigatório.
        /// </summary>
        public int ClienteId { get; set; } // Renomeado de UsuarioId para ClienteId

        // 2. CHAVE OPCIONAL: ID do Funcionário
        /// <summary>
        /// ID do funcionário que está gerenciando o pedido. Opcional (será associado depois).
        /// </summary>
        ///  
        public int? FuncionarioId { get; set; } // O ponto de interrogação indica que é anulável (opcional)

        // 3. CHAVE OPCIONAL: ID do Entregador
        /// <summary>
        /// ID do entregador associado ao pedido. Opcional (será associado depois).
        /// </summary> 
        public int? EntregadorId { get; set; } // O ponto de interrogação indica que é anulável (opcional)
        
        // Dados do Pedido
        public DateTime DataPedido { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPedido { get; set; }
        
        [Required]
        [StringLength(50)]
        public StatusPedido Status { get; set; } 
        
        // Dados de Entrega e Pagamento
        [StringLength(255)]
        public string EnderecoEntrega { get; set; }
        
        [StringLength(50)]
        public string MetodoPagamento { get; set; }
        
        // Propriedade de Navegação para os Itens do Pedido (relação 1-para-muitos)
        public ICollection<DetalhePedido> Itens { get; set; } = new List<DetalhePedido>();
    }

    public class DetalhePedido
    {
        // Chave Primária
        [Key]
        public int Id { get; set; }

        // Chave Estrangeira para a tabela Pedido
        [Required]
        public int PedidoId { get; set; }

        // Chave Estrangeira para a tabela Produto
        [Required]
        public int ProdutoId { get; set; }

        // Snapshot dos dados do Produto no momento da compra
        [Required]
        [StringLength(100)]
        public string NomeProduto { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecoUnitario { get; set; }

        // Propriedade de Navegação
        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; }

        // Propriedade calculada (opcional, mas útil)
        [NotMapped]
        public decimal SubTotal => Quantidade * PrecoUnitario;
    }
} 