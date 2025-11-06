using SaborGregoNew.DTOs.Usuario;
using SaborGregoNew.Models;
using SaborGregoNew.Repository;

namespace SaborGregoNew.Services
{
    public class EnderecoService
    {
        private readonly EnderecoRepository _EnderecoRepository;

        public EnderecoService(EnderecoRepository EnderecoRepository)
        {
            _EnderecoRepository = EnderecoRepository;
        }

        public async Task<bool> CadastrarEndereco(CadastroEnderecoDTO dto)
        {
            var endereco = new Endereco
            {
                Apelido = dto.Apelido,
                Logradouro = dto.Logradouro,
                Numero = dto.Numero,
                Complemento = dto.Complemento,
                Bairro = dto.Bairro,
                UsuarioId = dto.UsuarioId
            };

            await _EnderecoRepository.Create(endereco);
            return true;
        }
        public List<Endereco> GetAllByUserId(int userId)
        {
            return _EnderecoRepository.GetAllByUserId(userId);
        }
        public Endereco GetById(int id)
        {
            return _EnderecoRepository.GetById(id);
        }
        public async Task UpdateAsync(Endereco endereco)
        {
            await _EnderecoRepository.UpdateAsync(endereco);
        }

        public async Task DeleteAsync(int id)
        {
            await _EnderecoRepository.DeleteAsync(id);
        }
    }
}
        

            

                    

            