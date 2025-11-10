using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Data;
using SaborGregoNew.Models;
using SaborGregoNew.DTOs;
using saborGregoNew.Repository;
using SaborGregoNew.DTOs.Pedido;


namespace SaborGregoNew.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly ICarrinhoRepository _carrinhoService;
        private readonly IEnderecoRepository _enderecoService;
        private readonly IPedidoRepository _pedidoService;

        // O ViewModel para coletar as informações de entrega/pagamento
        [BindProperty]
        public CheckoutDTO CheckoutInfo { get; set; } = new CheckoutDTO();

        // Propriedade para exibir o carrinho na página de confirmação
        public List<CarrinhoItem> Carrinho { get; set; }
        public List<Endereco> Enderecos { get; set; }
        public decimal CarrinhoTotal { get; set; }

        public CheckoutModel(ICarrinhoRepository carrinhoService, IEnderecoRepository enderecoService, IPedidoRepository pedidoRepository)
        {
            _carrinhoService = carrinhoService;
            _enderecoService = enderecoService;
            _pedidoService = pedidoRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            Carrinho = _carrinhoService.GetCarrinho();
            // Inclua aqui sua verificação de carrinho vazio se ainda não o fez
            if (Carrinho == null || !Carrinho.Any())
            {
                TempData["MensagemErro"] = "Seu carrinho está vazio. Adicione itens antes de finalizar a compra.";
                return RedirectToPage("/Carrinho");
            }

            var clienteIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(clienteIdString))
            {
                return RedirectToPage("/Usuario/Login/Login");
            }
            var ClienteId = int.Parse(clienteIdString);

            
            
            this.Enderecos = await _enderecoService.SelectAllByUserIdAsync(ClienteId);

            if (Enderecos == null)
            {
                TempData["MensagemAviso"] = "Você precisa cadastrar um endereço para continuar.";
                // Redireciona para a página de cadastro de endereço
                return RedirectToPage("/Usuario/Endereco/Criar");
            }

            return Page();
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Carrinho = _carrinhoService.GetCarrinho();
            CarrinhoTotal = _carrinhoService.CalcularTotal();

            // 1. OBTENÇÃO DO ID DO USUÁRIO LOGADO (Lógica solicitada)
            var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // A verificação é defensiva. O atributo [Authorize] já deve garantir que não é null.
            if (string.IsNullOrEmpty(usuarioIdString))
            {
                // Este código só deve ser atingido se houver falha na autenticação.
                // Redireciona para o login ou retorna erro.
                return RedirectToPage("/Usuario/Login/Login");
            }
            var usuarioId = int.Parse(usuarioIdString);

            var enderecoSelecionado = await _enderecoService.GetByIdAndUserIdAsync(
                CheckoutInfo.EnderecoId, 
                usuarioId
            );

            if (enderecoSelecionado == null)
            {
                ModelState.AddModelError("CheckoutInfo.EnderecoId", "Endereço de entrega não encontrado ou inválido.");
                return Page();
            }

            // 2. Criação do Pedido (Order Header)
            if (!ModelState.IsValid)
            {
                TempData["MensagemErro"] = "Preencha todos os campos corretamente.";
                return Page();
            }
            
            var pedido = new PedidoDTO
            {
                ClienteId = usuarioId, // ⬅️ AGORA USAMOS ClienteId
                FuncionarioId = null,  // Inicializa como nulo
                EntregadorId = null,   // Inicializa como nulo
                DataPedido = DateTime.Now,
                ValorTotal = CarrinhoTotal,
                Status = 0, // Status inicial do pedido
                

                // Mapeia os dados do DTO
                EnderecoId = CheckoutInfo.EnderecoId,
                MetodoPagamento = CheckoutInfo.MetodoPagamento,
                // ...
            };

            // INÍCIO DA TRANSAÇÃO NO BANCO DE DADOS
            _pedidoService.Create(pedido);

            var detalhesParaSalvar = new List<DetalhePedido>();
            // 3. Criação dos Detalhes do Pedido (Order Details)
            foreach (var item in Carrinho)
            {
                var detalheModelo = new DetalhePedido
                {
                    PedidoId = pedido.Id,
                    ProdutoId = item.ProdutoId,
                    NomeProduto = item.Nome,
                    PrecoUnitario = item.Preco,
                    Quantidade = item.Quantidade
                };
                detalhesParaSalvar.Add(detalheModelo);
            }
            _pedidoService.AddDetalhesAsync(detalhesParaSalvar);

            // 4. Limpar o carrinho da sessão
            _carrinhoService.ClearCarrinho();
            
            ViewData["MensagemSucesso"] = "Pedido realizado com sucesso!";

            // Redireciona para a página de confirmação
            return RedirectToPage("/Pedido/ConfirmacaoPedido", new { pedidoid = pedido.Id, enderecoid = CheckoutInfo.EnderecoId });
        }
    }
}