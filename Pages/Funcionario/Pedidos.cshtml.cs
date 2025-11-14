using Microsoft.AspNetCore.Mvc.RazorPages; //funcionalidade da pagina
using Microsoft.AspNetCore.Mvc; //Metodos para post e get
using saborGregoNew.Repository.Interfaces; //conexão com o repository
using SaborGregoNew.Enums; //transformações de enumerate
using SaborGregoNew.Models; //models
using SaborGregoNew.Extensions; //Claims e Sessions


namespace SaborGregoNew.Pages.Funcionario
{
    public class PedidosModel : PageModel
    {

        //conexão com o repository
        private readonly IPedidoRepository _pedidoRepository;
        public PedidosModel(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }


        //variaveis
        public List<Pedido> PedidosSolicitados { get; set; } = new();
        public List<Pedido> PedidosEmPreparacao { get; set; } = new();


        //Carregamento da pagina
        public async Task<IActionResult> OnGetAsync()
        {
            // Tenta obter o userId. Se o retorno NÃO for null, é um erro de autenticação.
            if (TryGetUserId(out int userId) is IActionResult authResult)
            {
                return authResult; // Redireciona para login
            }

            // Se a autenticação foi bem-sucedida, continua a obter os dados
            PedidosSolicitados = await _pedidoRepository.GetPedidosPublicosPorStatusAsync(StatusPedido.Solicitado);
            PedidosEmPreparacao = await _pedidoRepository.GetPedidosFuncionarioPorStatusAsync(StatusPedido.EmPreparacao, userId);
            
            // OnGetAsync deve retornar um PageResult (se não houver Redirect), 
            // então retornamos 'Page()' explicitamente se tudo estiver OK.
            return Page(); 
        }

        // ----------------------------------------------------------------------------------------------------

        // Metodos para mudar status do pedido
        public async Task<IActionResult> OnPostIniciarAsync(int pedidoId)
        {
            // Tenta obter o userId. Se o retorno NÃO for null, é um erro de autenticação.
            if (TryGetUserId(out int userId) is IActionResult authResult)
            {
                return authResult; // Redireciona para login
            }
            
            try
            {
                await _pedidoRepository.UpdateStatusByIdAsync(pedidoId, userId, StatusPedido.EmPreparacao);
                return RedirectToPage();
            }
            catch (ArgumentException ex)
            {
                TempData["MensagemErro"] = "Erro ao iniciar o pedido: " + ex.Message;
                return RedirectToPage();
            }
        }

        // ----------------------------------------------------------------------------------------------------

        public async Task<IActionResult> OnPostConcluirAsync(int pedidoId)
        {
            // Tenta obter o userId. Se o retorno NÃO for null, é um erro de autenticação.
            if (TryGetUserId(out int userId) is IActionResult authResult)
            {
                return authResult; // Redireciona para login
            }

            try
            {
                await _pedidoRepository.UpdateStatusByIdAsync(pedidoId, userId, StatusPedido.ProntoParaRetirada);
                return RedirectToPage();
            }
            catch (ArgumentException ex)
            {
                TempData["MensagemErro"] = "Erro ao concluir o pedido: " + ex.Message;
                return RedirectToPage();
            }
        }
        private IActionResult TryGetUserId(out int userId)
        {
            userId = 0; // Inicializa a variável de saída

            try
            {
                userId = User.GetUserId();
                return null; // Retorna null, indicando sucesso
            }
            catch // Falha na autenticação/obtenção do ID
            {
                TempData["MensagemErro"] = "Usuário não autenticado. Por favor, faça Login para continuar!";
                return RedirectToPage("/Usuario/Login/Login");
            }
        }
    }
}