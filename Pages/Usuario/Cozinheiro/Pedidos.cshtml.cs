using Microsoft.AspNetCore.Mvc.RazorPages; //funcionalidade da pagina
using Microsoft.AspNetCore.Mvc; //Metodos para post e get
using saborGregoNew.Repository.Interfaces; //conexão com o repository
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
        public List<Pedido> PedidosSolicitados { get; set; } = new();
        public List<Pedido> PedidosEmPreparacao { get; set; } = new();


        //Carregamento da pagina
        public async Task OnGetAsync()
        {
            var userId = 0; // seta a variavel
            try
            {
                userId = User.GetUserId(); // tenta pegar o id do usuario logado
            }
            catch // caso de erro envia o cliente para login
            {
                TempData["MensagemErro"] = "Usuário não autenticado, Porfavor faça Login para continuar!";
                RedirectToPage("/Usuario/Login/Login");
            }

            PedidosSolicitados = await _pedidoRepository.GetPedidosFluxoTrabalhoAsync(StatusPedido.Solicitado, userId);
            PedidosEmPreparacao = await _pedidoRepository.GetPedidosFluxoTrabalhoAsync(StatusPedido.EmPreparacao, userId);
        }


        //Metodos para mudar status do pedido
        public async Task<IActionResult> OnPostIniciarAsync(int pedidoId)
        {
            var userId = 0; // seta a variavel
            try
            {
                userId = User.GetUserId(); // tenta pegar o id do usuario logado
            }
            catch // caso de erro envia o cliente para login
            {
                TempData["MensagemErro"] = "Usuário não autenticado, Porfavor faça Login para continuar!";
                RedirectToPage("/Usuario/Login/Login");
            }
            try
            {
                await _pedidoRepository.UpdateStatusByIdAsync(pedidoId, StatusPedido.EmPreparacao);
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
            var userId = 0; // seta a variavel
            try
            {
                userId = User.GetUserId(); // tenta pegar o id do usuario logado
            }
            catch // caso de erro envia o cliente para login
            {
                TempData["MensagemErro"] = "Usuário não autenticado, Porfavor faça Login para continuar!";
                RedirectToPage("/Usuario/Login/Login");
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