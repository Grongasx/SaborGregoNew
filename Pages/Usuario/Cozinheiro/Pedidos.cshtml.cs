using Microsoft.AspNetCore.Mvc.RazorPages; //funcionalidade da pagina
using Microsoft.AspNetCore.Mvc; //Metodos para post e get
using SaborGregoNew.Repository; //conexão com o repository
using SaborGregoNew.Enums; //transformações de enumerate
using SaborGregoNew.Models; //models
using SaborGregoNew.Extensions; //Claims e Sessions
namespace SaborGregoNew.Pages.Usuario.Cozinheiro
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
        public List<Models.Pedido> PedidosSolicitados { get; set; } = new();
        public List<Models.Pedido> PedidosEmPreparacao { get; set; } = new();
        //Carregamento da pagina
        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.IsInRole("Cozinheiro"))
            {
                TempData["MensagemErro"] = "Acesso negado! Apenas cozinheiros podem acessar esta página.";
                return RedirectToPage("/Index");
            }
            var userId = 0; // seta a variavel
            try
            {
                userId = User.GetUserId(); // tenta pegar o id do usuario logado
            }
            catch // caso de erro envia o cliente para login
            {
                TempData["MensagemErro"] = "Usuário não autenticado, Porfavor faça Login para continuar!";
                return RedirectToPage("/Usuario/Login");
            }
            PedidosSolicitados = await _pedidoRepository.GetPedidosFluxoTrabalhoAsync(StatusPedido.Solicitado, userId);
            PedidosEmPreparacao = await _pedidoRepository.GetPedidosFluxoTrabalhoAsync(StatusPedido.EmPreparacao, userId);
            return Page();
        }
        //Metodos para mudar status do pedido
        public async Task<IActionResult> OnPostIniciarAsync(int pedidoId)
        {
            if (!User.IsInRole("Cozinheiro"))
            {
                TempData["MensagemErro"] = "Acesso negado! Apenas cozinheiros podem realizar esta ação.";
                return RedirectToPage("/Index");
            }
            var userId = 0; // seta a variavel
            try
            {
                userId = User.GetUserId(); // tenta pegar o id do usuario logado
            }
            catch // caso de erro envia o cliente para login
            {
                TempData["MensagemErro"] = "Usuário não autenticado, Por favor faça Login para continuar!";
                return RedirectToPage("/Usuario/Login");
            }
            try
            {
                await _pedidoRepository.UpdateStatusAndAssignAsync(pedidoId, StatusPedido.EmPreparacao, userId);
                return RedirectToPage();
            }
            catch (ArgumentException ex)
            {
                TempData["MensagemErro"] = "Erro ao iniciar o pedido: " + ex.Message;
                return RedirectToPage();
            }
        }
        public async Task<IActionResult> OnPostConcluirAsync(int pedidoId)
        {
            if (!User.IsInRole("Cozinheiro"))
            {
                TempData["MensagemErro"] = "Acesso negado! Apenas cozinheiros podem realizar esta ação.";
                return RedirectToPage("/Index");
            }
            var userId = 0; // seta a variavel
            try
            {
                userId = User.GetUserId(); // tenta pegar o id do usuario logado
            }
            catch // caso de erro envia o cliente para login
            {
                TempData["MensagemErro"] = "Usuário não autenticado, Porfavor faça Login para continuar!";
                return RedirectToPage("/Usuario/Login");
            }
            try
            {
                await _pedidoRepository.UpdateStatusByIdAsync(pedidoId, StatusPedido.ProntoParaRetirada); // tenta mudar o status do pedido
                return RedirectToPage();//reload da pagina
            }
            catch (ArgumentException ex)// caso de erro
            {
                TempData["MensagemErro"] = "Erro ao iniciar o pedido: " + ex.Message;
                return RedirectToPage();
            }
        }
    }
}