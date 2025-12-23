using System.ComponentModel.DataAnnotations;

namespace SaborGregoNew.DTOs
{
    public class DashboardPedido
    {
        public string Ordem { get; set; } = string.Empty;
        public string Quantidade { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
    }
    
    public class DashboardAdminDTO
    {
        [Display(Name = "Vendas de Hoje")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal VendasHoje { get; set; }

        [Display(Name = "Vendas do Mês")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal VendasMes { get; set; }

        [Display(Name = "Pedidos de Hoje")]
        public int PedidosHoje { get; set; }

        [Display(Name = "Pedidos do Mês")]
        public int PedidosMes { get; set; }

        [Display(Name = "Produto Mais Vendido (Hoje)")]
        public string ProdutoMaisVendidoHoje { get; set; } = "Nenhum";

        [Display(Name = "Produto Mais Vendido (Mês)")]
        public string ProdutoMaisVendidoMes { get; set; } = "Nenhum";
    }
    
    public class VendasPorDiaDTO
    {
        // Armazena o dia formatado (ex: "13/11")
        public string Dia { get; set; } = string.Empty; 
        
        // Armazena o total de vendas nesse dia
        public decimal Total { get; set; }
    }
    
    public class VendasPorCategoriaDTO
    {
        public string Categoria { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }

    public class VendasPorProdutoDTO
    {
        public string Produto { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}