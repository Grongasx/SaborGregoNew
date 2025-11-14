using SaborGregoNew.Models;
using SaborGregoNew.Extensions;
using saborGregoNew.Repository;

namespace SaborGregoNew.Repository
{
    public class CarrinhoSessionRepository : ICarrinhoRepository
    {
        private readonly IProdutoRepository _produtoRepository; //conexão com o repositorio de pedidos

        //conexão com a sessão
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CarrinhoSessionRepository(IHttpContextAccessor httpContextAccessor, IProdutoRepository produtoRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _produtoRepository = produtoRepository;
        }

        //listagem de todos os itens do carrinho
        public List<CarrinhoItem> GetCarrinho()
        {
            var carrinho = _httpContextAccessor.HttpContext?//cria uma variavel
                .Session?//procura pela sessão
                .GetObjectFromJson<List<CarrinhoItem>>("Carrinho");//Lê o arquivo json da sessão chamado carrinho
            return carrinho ?? new List<CarrinhoItem>();//retorna a lista, senão, cria uma nova
        }

        //salva o carrinho atualizado no arquivo json
        public void SaveCarrinho(List<CarrinhoItem> carrinho) //recebe uma lista de itens e retorna "deu certo"
        {
            try
            {
                var session = _httpContextAccessor.HttpContext?.Session;//procura a sessão
                if (session == null) throw new Exception("a sessão não existe");
                session.SetObjectFromJson("Carrinho", carrinho);//atualiza o carrinho
            }
            catch(Exception ex)
            {
                throw new Exception("erro ao salvar os itens no carrinho", ex);//caso de merda ele acusa
            }
        }

        //limpa o carrinho (usado quando o pedido é realizado)
        public void ClearCarrinho()
        {
            try
            {
                _httpContextAccessor.HttpContext?.Session.Remove("Carrinho");//limpa o objeto carrinho do arquivo json
            }
            catch
            {
                throw new Exception("Problema ao limpar o carrinho");
            }
        }

        //Adiciona produtos ao carrinho
        public async Task AdicionarAoCarrinhoAsync(Produto produto)//recebe um objeto produto retorna um "ok"
        {
            try
            {
                if (produto == null)//caso o produto não exista
                {
                    throw new Exception($"Produto não encontrado.");
                }
                var carrinho = GetCarrinho();//buscar o carrinho na sessão

                //copia o produto para inserir no carrinho como um item
                carrinho.Add(new CarrinhoItem
                {
                    Nome = produto.Nome,
                    ProdutoId = produto.Id,
                    Quantidade = 1, //seta a quantidade automaticamente para 1
                    Preco = produto.Preco,
                    Imagem = produto.Imagem,
                });

                SaveCarrinho(carrinho);//salva o carrinho no arquivo json
            }
            catch(Exception ex)
            {
                throw new Exception("Ocorreu um problema ao Inserir o produto no carrinho", ex);
            }
        }

        //remove um item do carrinho
        public void RemoverItem(int produtoId)//recebe o id do produto
        {
            try
            {
                var carrinho = GetCarrinho(); //busca o carrinho
                var itemParaRemover = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);//procura o item no carrinho pelo id do produto

                if (itemParaRemover != null)
                {
                    carrinho.Remove(itemParaRemover);
                    SaveCarrinho(carrinho);
                }
                else throw new Exception("O produto é nulo");
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um problema ao deletar o item do carrinho", ex);
            }
        }
        
        //soma o valor dos itens no carrinho (usado para quando for salvar no pedido)
        public decimal CalcularTotal()
        {
            var itens = GetCarrinho();

            // 1. Verifica se a lista é nula ou vazia.
            if (itens == null || !itens.Any())
            {
                return 0.00m; // Retorna 0.00 se o carrinho estiver vazio.
            }

            // 2. Tenta somar os SubTotais
            return itens.Sum(i => i.SubTotal);
        }
        //Atualiza a quantidade de itens em um unico item do carrinho
        public void AtualizarQuantidade(int produtoId, int novaQuantidade)//recebe o id do produto e a quantidade nova
        {
            var carrinho = GetCarrinho();//busca o carrinho

            var item = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);//busca o item no carrinho

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