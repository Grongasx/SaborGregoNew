using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using saborGregoNew.Repository;
using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Models;
using System.Security.Claims;

namespace SaborGregoNew.Pages.Usuario
{
    public class EditarEnderecoModel : PageModel
    {
        private readonly IEnderecoRepository _enderecoService;

        [BindProperty]
        public Endereco enderecoatualizado { get; set; } = new Endereco();
        public Endereco enderecoConfirmacao { get; set; } = new Endereco();

        public EditarEnderecoModel(IEnderecoRepository enderecoRepository)
        {
            _enderecoService = enderecoRepository;
        }

        public async Task<IActionResult> OnGet(int Id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
            {
                TempData["MensagemErro"] = "Para acessar esta página, você precisa estar logado.";
                return RedirectToPage("/Usuario/Login");
            }
            var UserId = int.Parse(userIdString);

            enderecoConfirmacao = await _enderecoService.SelectByIdAsync(Id);

            if (enderecoConfirmacao == null)
            {
                TempData["MensagemErro"] = "Endereço não encontrado.";
                return Page();
            }
            if (enderecoConfirmacao.UsuarioId != UserId)
            {
                TempData["MensagemErro"] = "Este endereço não pertence ao usuário logado.";
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int Id)
        {
            if (!ModelState.IsValid)
            {
                TempData["MensagemErro"] = "Preencha todos os campos corretamente.";
                return Page();
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                TempData["MensagemErro"] = "Para acessar esta página, você precisa estar logado.";
                return RedirectToPage("/Usuario/Login/Login");
            }
            var UserId = int.Parse(userIdClaim);

            enderecoConfirmacao = await _enderecoService.SelectByIdAsync(Id);
            if (enderecoConfirmacao == null)
            {
                TempData["MensagemErro"] = "Endereço não encontrado.";
                return Page();
            }
            if (enderecoConfirmacao.UsuarioId != UserId)
            {
                TempData["MensagemErro"] = "O endereço não pertence ao usuário logado.";
                return Forbid();
            }
            var dto = new EnderecoDTO();
            try
            {
                dto = new EnderecoDTO
                {
                    Apelido = enderecoatualizado.Apelido,
                    Logradouro = enderecoatualizado.Logradouro,
                    Numero = enderecoatualizado.Numero,
                    Bairro = enderecoatualizado.Bairro,
                    Complemento = enderecoatualizado.Complemento ?? "",
                    UsuarioId = enderecoatualizado.UsuarioId
                };
                if (enderecoatualizado != enderecoConfirmacao)
                {
                    await _enderecoService.UpdateById(enderecoatualizado.Id, dto);

                    TempData["MensagemSucesso"] = "Endereço atualizado com sucesso!";
                    return RedirectToPage("/Usuario/Endereco/ListaEnderecos");
                }
                else
                {
                    TempData["MensagemErro"] = "Os dados não foram alterados.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro na associação dos dados: " + ex.Message;
                return Page();
            }
        }
    }
}
