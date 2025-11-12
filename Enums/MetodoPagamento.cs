using Microsoft.OpenApi.Attributes;

namespace SaborGregoNew.Enums;

public enum MetodoPagamento
{
    [Display(name: "CartãoNaEntrega")]
    CartãoNaEntrega,
    [Display(name: "Dinheiro")]
    Dinheiro,
    [Display(name: "Pix")]
    Pix
}