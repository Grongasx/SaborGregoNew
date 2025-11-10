using System.ComponentModel.DataAnnotations;
using SaborGregoNew.Enums;

namespace SaborGregoNew.DTOs
{
    public class CheckoutDTO
    {
        // ------------------ Endereço ------------------
        [Required(ErrorMessage = "O endereço de entrega é obrigatório.")]
        [Display(Name = "Endereço de Entrega Completo")]
        public int EnderecoId { get; set; }
        
        // ------------------ Pagamento ------------------
        [Required(ErrorMessage = "O método de pagamento é obrigatório.")]
        [Display(Name = "Método de Pagamento")]
        public MetodoPagamento MetodoPagamento { get; set; }
        
        // TODO: Você pode adicionar um campo 'Observacoes' ou 'Cupom' aqui.
    }
}