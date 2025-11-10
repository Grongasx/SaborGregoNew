using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using saborGregoNew.Repository;
using SaborGregoNew.DTOs.Usuario;

namespace SaborGregoNew.Pages.Usuario
{
    public class CadastroEnderecoModel : PageModel
    {
        private readonly IEnderecoRepository _enderecoService;

        public CadastroEnderecoModel(IEnderecoRepository enderecoService)
        {
            _enderecoService = enderecoService;
        }

        [BindProperty]
        public EnderecoDTO endereco { get; set; } = new EnderecoDTO();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {


            if (!ModelState.IsValid)
            {
                TempData["MensagemErro"] = "Preencha todos os campos corretamente.";
                return Page();
            }
            
            var UserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserIdString == null)
            {
                TempData["MensagemErro"] = "Para acessar esta página, você precisa estar logado.";
                return Unauthorized();
            }

            var UserId = int.Parse(UserIdString);
            
            await _enderecoService.Create(endereco, UserId);


            TempData["MensagemSucesso"] = "Endereço cadastrado com sucesso!";
            return RedirectToPage("/Usuario/Endereco/ListaEnderecos");
        }
    }
}
        