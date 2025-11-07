using System.ComponentModel.DataAnnotations;

namespace SaborGregoNew.Enums;

public enum Categoria
{
    [Display(Name = "Lanches")]
    Lanche,
    [Display(Name = "Bebidas")]
    Bebida,
    [Display(Name = "Complementos")]
    Complemento
}