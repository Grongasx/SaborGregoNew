using Microsoft.OpenApi.Attributes;

namespace SaborGregoNew.Enums;

public enum Status
{
    [Display(name: "Feito")]
    Feito,
    [Display(name: "Preparando")]
    Preparando,
    [Display(name: "Pronto")]
    Pronto,
    [Display(name: "Entregando")]
    Entregando,
    [Display(name: "Concluído")]
    Concluido,
    [Display(name: "Cancelado")]
    Cancelado
}