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
        public Endereco endereco { get; set; } = new Endereco();

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

            endereco = await _enderecoService.SelectByIdAsync(Id);

            if (endereco == null)
            {
                TempData["MensagemErro"] = "Endereço não encontrado.";
                return Page();
            }
            if (endereco.UsuarioId != UserId)
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

            // 2. Segurança: Obter o ID do usuário logado
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                TempData["MensagemErro"] = "Para acessar esta página, você precisa estar logado.";
                return RedirectToPage("/Usuario/Login/Login");
            }
            var UserId = int.Parse(userIdClaim);

            // 3. Carregar o Endereço ORIGINAL do banco para SEGURANÇA E PROPRIEDADE
            var enderecoOriginal = await _enderecoService.SelectByIdAsync(Id);
            
            if (enderecoOriginal == null || enderecoOriginal.UsuarioId != UserId)
            {
                TempData["MensagemErro"] = "Endereço não encontrado ou não pertence ao usuário logado.";
                return Forbid(); // Retorna 403 (Proibido) se não for o dono
            }

            // 4. Mapear os dados EDITADOS (contidos em this.endereco) para o DTO
            // Os dados do formulário (this.endereco) são usados, mas o ID do usuário é obtido do original.
            var enderecodto = new EnderecoDTO
            {
                Apelido = endereco.Apelido,
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                Bairro = endereco.Bairro,
                Complemento = endereco.Complemento ?? "",
                // ⭐️ IMPORTANTÍSSIMO: Usar o ID ORIGINAL DO BANCO para garantir a integridade
                UsuarioId = enderecoOriginal.UsuarioId 
            };

            // 5. Atualizar no Banco de Dados
            await _enderecoService.UpdateById(Id, enderecodto);

            TempData["MensagemSucesso"] = "Endereço atualizado com sucesso!";
            return RedirectToPage("/Usuario/Endereco/ListaEnderecos");
        }
    }
}
