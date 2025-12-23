using Microsoft.OpenApi.Attributes;

namespace SaborGregoNew.Enums;

public enum FormaPagamento
{
    [Display(name: "Cartão de Crédito")]
    CartaoCredito,
    [Display(name: "Cartão de Débito")]
    CartaoDebito,
    [Display(name: "Dinheiro")]
    Dinheiro,
    [Display(name: "Pix")]
    Pix
}