using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.DTOs.Usuario;

namespace SaborGregoNew.Pages.Usuario
{
    public class CadastroEnderecoModel : PageModel
    {
        private readonly SaborGregoNew.Repository.IEnderecoRepository _enderecoRepository;

        public CadastroEnderecoModel(SaborGregoNew.Repository.IEnderecoRepository enderecoRepository)
        {
            _enderecoRepository = enderecoRepository;
        }

        [BindProperty]
        public CadastroEnderecoDTO? enderecoDTO { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id.HasValue && id.Value > 0)
            {
                var endereco = await _enderecoRepository.SelectByIdAsync(id.Value);
                if (endereco != null)
                {
                    enderecoDTO = new CadastroEnderecoDTO
                    {
                        Id = endereco.Id,
                        Apelido = endereco.Apelido,
                        Logradouro = endereco.Logradouro,
                        Numero = endereco.Numero,
                        Complemento = endereco.Complemento,
                        Bairro = endereco.Bairro,
                        UsuarioId = endereco.UsuarioId
                    };
                }
            }
            
            if (enderecoDTO == null)
            {
                enderecoDTO = new CadastroEnderecoDTO();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var UserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(UserIdString))
            {
                return Unauthorized();
            }

            var UserId = int.Parse(UserIdString);
            enderecoDTO.UsuarioId = UserId;

            if (enderecoDTO.Id > 0)
            {
                await _enderecoRepository.UpdateById(enderecoDTO.Id, enderecoDTO);
            }
            else
            {
                await _enderecoRepository.Create(enderecoDTO, UserId);
            }

            return RedirectToPage("/Usuario/Endereco/Index");
        }
    }
}