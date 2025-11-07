using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Data;
using SaborGregoNew.Models;
using SaborGregoNew.Services;
using SaborGregoNew.DTOs;
using Microsoft.EntityFrameworkCore;


namespace SaborGregoNew.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly CarrinhoService _carrinhoService;
        private readonly ApplicationDbContext _context;
        private readonly EnderecoService _enderecoService;

        // O ViewModel para coletar as informações de entrega/pagamento
        [BindProperty]
        public CheckoutDTO CheckoutInfo { get; set; } = new CheckoutDTO();

        // Propriedade para exibir o carrinho na página de confirmação
        public List<CarrinhoItem> Carrinho { get; set; }
        public List<Endereco> Enderecos { get; set; }
        public decimal CarrinhoTotal { get; set; }

        public CheckoutModel(CarrinhoService carrinhoService, ApplicationDbContext context, EnderecoService enderecoService)
        {
            _carrinhoService = carrinhoService;
            _context = context;
            _enderecoService = enderecoService;
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

            
            
            this.Enderecos = await _enderecoService.GetAllByUserId(ClienteId);

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

            if (!ModelState.IsValid)
            {
                return Page();
            }

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

            if (!ModelState.IsValid)
            {
                // Se o modelo não for válido, recarrega a página com os erros
                // e os dados do carrinho para que o usuário possa corrigir.
                return Page();
            }

            var enderecoSelecionado = await _context.Enderecos // Use o seu DbSet<Endereco> aqui
                .AsNoTracking() // Não precisamos rastrear a entidade Endereco
                .FirstOrDefaultAsync(e => e.Id == CheckoutInfo.EnderecoId && e.UsuarioId == usuarioId);
            
            if (enderecoSelecionado == null)
            {
                ModelState.AddModelError("CheckoutInfo.EnderecoId", "Endereço de entrega não encontrado ou inválido.");
                return Page();
            }

            // 2. Criação do Pedido (Order Header)

            var pedido = new Models.Pedido
            {
                ClienteId = usuarioId, // ⬅️ AGORA USAMOS ClienteId
                FuncionarioId = null,  // Inicializa como nulo
                EntregadorId = null,   // Inicializa como nulo
                DataPedido = DateTime.Now,
                TotalPedido = CarrinhoTotal,
                Status = 0, // Status inicial do pedido
                

                // Mapeia os dados do DTO
                EnderecoEntrega = $"{CheckoutInfo.EnderecoId}",
                MetodoPagamento = CheckoutInfo.MetodoPagamento,
                // ...
            };

            // INÍCIO DA TRANSAÇÃO NO BANCO DE DADOS
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            // 3. Criação dos Detalhes do Pedido (Order Details)
            foreach (var item in Carrinho)
            {
                var detalhe = new DetalhePedido
                {
                    PedidoId = pedido.Id,
                    ProdutoId = item.ProdutoId,
                    NomeProduto = item.Nome,
                    PrecoUnitario = item.Preco,
                    Quantidade = item.Quantidade
                };
                _context.DetalhesPedido.Add(detalhe);
            }

            await _context.SaveChangesAsync();

            // 4. Limpar o carrinho da sessão
            _carrinhoService.ClearCarrinho();

            // Redireciona para a página de confirmação
            return RedirectToPage("/Pedido/ConfirmacaoPedido", new { id = pedido.Id });
        }

    }
}