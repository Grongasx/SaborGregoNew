using SaborGregoNew.Models;
using SaborGregoNew.Repository;
using SaborGregoNew.DTOs.Usuario;

namespace SaborGregoNew.Services
{
    // This service handles business logic for Endereco (Address) entities.

{
    public class EnderecoService
    {
        private readonly EnderecoRepository _enderecoRepository;

        public EnderecoService(EnderecoRepository enderecoRepository)
        {
            _enderecoRepository = enderecoRepository;
        }

        public async Task<List<Endereco>> GetEnderecosByUsuarioId(int usuarioId)
        {
            return await _enderecoRepository.GetEnderecosByUsuarioId(usuarioId);
        }

        public async Task AddEndereco(CadastroEnderecoDTO dto, int usuarioId)
        {
            var endereco = new Endereco
            {
                Apelido = dto.Apelido,
                Logradouro = dto.Logradouro,
                Numero = dto.Numero,
                Bairro = dto.Bairro,
                Complemento = dto.Complemento,
                UsuarioId = usuarioId
            };
            await _enderecoRepository.AddEndereco(endereco);
        }

        public async Task UpdateAsync(int id, CadastroEnderecoDTO dto)
        {
            var endereco = await _enderecoRepository.GetEnderecoById(id);
            if (endereco == null)
            {
                throw new KeyNotFoundException("Endereço não encontrado.");
            }

            endereco.Apelido = dto.Apelido;
            endereco.Logradouro = dto.Logradouro;
            endereco.Numero = dto.Numero;
            endereco.Bairro = dto.Bairro;
            endereco.Complemento = dto.Complemento;

            await _enderecoRepository.UpdateEndereco(endereco);
        }

        public async Task DeleteAsync(int id)
        {
            var endereco = await _enderecoRepository.GetEnderecoById(id);
            if (endereco == null)
            {
                throw new KeyNotFoundException("Endereço não encontrado.");
            }
            await _enderecoRepository.DeleteEndereco(endereco);
        }

        public async Task<Endereco> GetById(int id)
        {
            return await _enderecoRepository.GetEnderecoById(id);
        }

        public async Task<List<Endereco>> GetAllByUserId(int userId)
        {
            return await _enderecoRepository.GetEnderecosByUsuarioId(userId);
        }
    }
}
