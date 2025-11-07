using SaborGregoNew.Models;
using SaborGregoNew.Repositories;
using SaborGregoNew.Repository;

namespace SaborGregoNew.Services
{
    public class CarrinhoService
    {
        private readonly CarrinhoRepository _carrinhoRepository;
        private readonly ProdutoRepository _produtoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;// Assumindo que já existe

        public CarrinhoService(CarrinhoRepository carrinhoRepository, ProdutoRepository produtoRepository, IHttpContextAccessor httpContextAccessor)
        {
            _carrinhoRepository = carrinhoRepository;
            _produtoRepository = produtoRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<CarrinhoItem> GetCarrinho()
        {
            // Pede ao Repositório para carregar os dados
            return _carrinhoRepository.GetCarrinho();
        }

        public void AdicionarAoCarrinho(int produtoId, int quantidade = 1)
        {
            // 1. Busca os detalhes do produto no BD via ProdutoRepository
            // (Você pode querer retornar a Entidade Produto, ou um ProdutoDTO)
            var produto = _produtoRepository.GetById(produtoId); 
            
            if (produto == null)
            {
                // Tratar erro: produto não encontrado
                throw new Exception("Produto não encontrado."); 
            }

            // 2. Obtém a lista atual
            var carrinho = _carrinhoRepository.GetCarrinho();
            var itemExistente = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);

            if (itemExistente != null)
            {
                itemExistente.Quantidade += quantidade;
            }
            else
            {
                // Mapeamento da Entidade Produto para CarrinhoItem
                carrinho.Add(new CarrinhoItem
                {
                    ProdutoId = produto.Id,
                    Nome = produto.Nome,
                    Preco = produto.Preco,
                    Imagem = produto.Imagem,
                    Quantidade = quantidade
                });
            }

            // 3. Salva a lista atualizada de volta na Sessão
            _carrinhoRepository.SaveCarrinho(carrinho);
        }
        
        public void RemoverItem(int produtoId)
        {
            var carrinho = _carrinhoRepository.GetCarrinho();
            var itemParaRemover = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);
            
            if (itemParaRemover != null)
            {
                carrinho.Remove(itemParaRemover);
                _carrinhoRepository.SaveCarrinho(carrinho);
            }
        }
        
        public decimal CalcularTotal()
        {
            return GetCarrinho().Sum(i => i.SubTotal);
        }

        public void AtualizarQuantidade(int produtoId, int novaQuantidade)
        {
            // 1. Obter o carrinho atual
            var carrinho = _carrinhoRepository.GetCarrinho();

            // 2. Localizar o item pelo ID
            var item = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);

            if (item != null)
            {
                if (novaQuantidade <= 0)
                {
                    // 3. Se a quantidade for zero ou negativa, remove o item
                    carrinho.Remove(item);
                }
                else
                {
                    // 4. Caso contrário, atualiza a quantidade
                    item.Quantidade = novaQuantidade;
                }

                // 5. Salvar o carrinho modificado de volta na Sessão (via Repositório)
                _carrinhoRepository.SaveCarrinho(carrinho);
            }
            // Se o item não for encontrado, não faz nada (pode-se adicionar log, se necessário)
        }
        public void ClearCarrinho()
        {
            _carrinhoRepository.ClearCarrinho();
        }
    }
}