using Microsoft.OpenApi.Attributes;

namespace SaborGregoNew.Enums;

public enum MetodoPagamento
{
    [Display(name: "CartãoNaEntrega")]
    CartãoNaEntrega = 0,
    [Display(name: "Dinheiro")]
    Dinheiro = 1,
    [Display(name: "Pix")]
    Pix = 2
}