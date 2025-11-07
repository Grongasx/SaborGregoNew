using SaborGregoNew.Models;
using SaborGregoNew.Repository;


namespace SaborGregoNew.Repositories
{
    public class CarrinhoRepository 
    {
        private const string CarrinhoKey = "CarrinhoDeCompras";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UsuarioRepository _UsuarioRepository;


        public CarrinhoRepository(IHttpContextAccessor httpContextAccessor)
        {
            // Injeção de dependência para acessar o contexto HTTP e a Sessão
            _httpContextAccessor = httpContextAccessor;
        }

        // Propriedade auxiliar para acessar a Sessão
        private ISession Session => _httpContextAccessor.HttpContext.Session;

        // Recupera a lista de itens do carrinho da Sessão
        public List<CarrinhoItem> GetCarrinho()
        {
            return Session.Get<List<CarrinhoItem>>(CarrinhoKey) ?? new List<CarrinhoItem>();
        }

        // Salva a lista de itens do carrinho na Sessão
        public void SaveCarrinho(List<CarrinhoItem> carrinho)
        {
            Session.Set(CarrinhoKey, carrinho);
        }

        // Exemplo: Limpar o carrinho (útil após checkout)
        public void ClearCarrinho()
        {
            Session.Remove(CarrinhoKey);
        }
    }
}