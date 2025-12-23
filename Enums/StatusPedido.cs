using System.ComponentModel.DataAnnotations;

namespace SaborGregoNew.Enums
{
    public enum StatusPedido
    {
        [Display(Name = "Aguardando Confirmação")]
        Solicitado,

        [Display(Name = "Em Preparação")]
        EmPreparacao,

        [Display(Name = "Pronto para Retirada")]
        ProntoParaRetirada, // Ou "Pronto para Entrega"

        [Display(Name = "Saiu para Entrega")]
        EmRotaDeEntrega,

        [Display(Name = "Concluído")]
        Entregue,

        [Display(Name = "Cancelado")]
        Cancelado
    }
}