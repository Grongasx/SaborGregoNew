using SaborGregoNew.Models;
using SaborGregoNew.Extensions;
using saborGregoNew.Repository;

namespace SaborGregoNew.Repositories
{
    // ⭐️ RENOMEADO para refletir o uso da Session e implementar a interface
    public class CarrinhoSessionRepository : ICarrinhoRepository 
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CarrinhoSessionRepository(IProdutoRepository produtoRepository, IHttpContextAccessor httpContextAccessor)
        {
            _produtoRepository = produtoRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<CarrinhoItem> GetCarrinho()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
            {
                return new List<CarrinhoItem>();
            }
            // Usa o método de extensão GetObjectFromJson
            var carrinho = session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho");
            return carrinho ?? new List<CarrinhoItem>();
        }

        public void SaveCarrinho(List<CarrinhoItem> carrinho)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.SetObjectFromJson("Carrinho", carrinho); 
        }

        public void ClearCarrinho()
        {
            _httpContextAccessor.HttpContext?.Session.Remove("Carrinho");
        }

        public async Task AdicionarAoCarrinhoAsync(int produtoId, int quantidade = 1)
        {
            // 1. ✅ CORREÇÃO: Usar 'await' para desempacotar o Produto do Task
            var produto = await _produtoRepository.SelectByIdAsync(produtoId); 
            
            if (produto == null)
            {
                throw new Exception($"Produto não encontrado.");
            }

            // 2. ✅ CORREÇÃO: Usar o próprio GetCarrinho() da classe (que lê a Session)
            var carrinho = GetCarrinho(); 
            var itemExistente = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);

            if (itemExistente != null)
            {
                itemExistente.Quantidade += quantidade;
            }
            else
            {
                // Mapeamento
                carrinho.Add(new CarrinhoItem
                {
                    Nome = produto.Nome,
                    ProdutoId = produto.Id,
                    Quantidade = quantidade,
                    Preco = produto.Preco,
                    Imagem = produto.Imagem,
                });
            }

            // 3. Salva a lista atualizada de volta na Sessão usando o próprio método da classe
            SaveCarrinho(carrinho); 
        }
        
        public void RemoverItem(int produtoId)
        {
            // ✅ CORREÇÃO: Usar o próprio GetCarrinho()
            var carrinho = GetCarrinho(); 
            var itemParaRemover = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);
            
            if (itemParaRemover != null)
            {
                carrinho.Remove(itemParaRemover);
                // ✅ CORREÇÃO: Usar o próprio SaveCarrinho()
                SaveCarrinho(carrinho); 
            }
        }
        
        public decimal CalcularTotal()
        {
            // ✅ OK: Usa o próprio GetCarrinho()
            return GetCarrinho().Sum(i => i.SubTotal);
        }

        public void AtualizarQuantidade(int produtoId, int novaQuantidade)
        {
            var carrinho = GetCarrinho();

            var item = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);

            if (item != null)
            {
                if (novaQuantidade <= 0)
                {
                    carrinho.Remove(item);
                }
                else
                {
                    item.Quantidade = novaQuantidade;
                }
                // ✅ CORREÇÃO: Usar o próprio SaveCarrinho()
                SaveCarrinho(carrinho); 
            }
        }
    }
}