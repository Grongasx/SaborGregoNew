using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Models;
using SaborGregoNew.DTOs;
using SaborGregoNew.Repository;
using SaborGregoNew.DTOs.Pedido;
using SaborGregoNew.Extensions;

namespace SaborGregoNew.Pages
{
    public class CheckoutModel : PageModel
    {
        // injeção dos repositories e jsonservices
        private readonly ICarrinhoRepository _carrinhoService;
        private readonly IEnderecoRepository _enderecoService;
        private readonly IPedidoRepository _pedidoService;

        // O ViewModel para coletar as informações de entrega/pagamento
        [BindProperty]
        public CheckoutDTO CheckoutInfo { get; set; } = new CheckoutDTO(); //cria um modelo DTO para verificar os dados q voltam da pagina
        public List<CarrinhoItem> Carrinho { get; set; } = new List<CarrinhoItem>(); //cria uma lista nova de itens do carrinho
        public List<Endereco> Enderecos { get; set; } // lista de endereços do usuário
        public decimal CarrinhoTotal { get; set; } //para calcular o total do pedido


        // cria variaveis para os repositories e services
        public CheckoutModel(ICarrinhoRepository carrinhoService, IEnderecoRepository enderecoService, IPedidoRepository pedidoRepository)
        {
            _carrinhoService = carrinhoService;
            _enderecoService = enderecoService;
            _pedidoService = pedidoRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            Carrinho = _carrinhoService.GetCarrinho(); //busca para ver se o carrinho existe
            CarrinhoTotal = _carrinhoService.CalcularTotal(); //calcula o valor total do pedido

            var clienteIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(clienteIdString))
            {
                return RedirectToPage("/Usuario/Login");
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
        
        public async Task<IActionResult> OnPostAtualizarQuantidade(int produtoId, int novaQuantidade)
        {
            _carrinhoService.AtualizarQuantidade(produtoId, novaQuantidade);

            var carrinho = _carrinhoService.GetCarrinho();
            var item = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);

            if (item == null)
            {
                return new JsonResult(new { removido = true });
            }
            return new JsonResult(new
            {
                removido = false,
                novosubTotal = item.SubTotal.ToString("C")
            });
        }

        public async Task<IActionResult> OnPostFinalizarCompraAsync()
        {
            // 1. Pegamos o ID do usuário logo no início para usar no tratamento de erro
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                Carrinho = _carrinhoService.GetCarrinho();
                if (Carrinho == null || !Carrinho.Any())
                {
                    return RedirectToPage("/Pedido/Carrinho/Carrinho");
                }

                CarrinhoTotal = _carrinhoService.CalcularTotal();

                // Validação: Endereço não selecionado
                if (CheckoutInfo.EnderecoId <= 0)
                {
                    ModelState.AddModelError("", "Selecione um endereço de entrega.");
                    await LoadEnderecosAsync(userId); // <--- RECARREGA A LISTA
                    return Page();
                }

                var enderecoSelecionado = await _enderecoService.SelectByIdAsync(CheckoutInfo.EnderecoId);
                
                // Validação: Endereço não existe
                if (enderecoSelecionado == null)
                {
                    ModelState.AddModelError("", "Endereço não encontrado.");
                    await LoadEnderecosAsync(userId); // <--- RECARREGA A LISTA
                    return Page();
                }

                // Validação: Modelo inválido (ex: falta pagamento)
                if (!ModelState.IsValid)
                {
                    await LoadEnderecosAsync(userId); // <--- RECARREGA A LISTA
                    return Page();
                }

                // Criação do DTO
                var pedido = new PedidoDTO
                {
                    ClienteId = userId,
                    DataPedido = DateTime.Now,
                    ValorTotal = CarrinhoTotal,
                    Status = 0, // Solicitado
                    EnderecoId = CheckoutInfo.EnderecoId,
                    MetodoPagamento = CheckoutInfo.MetodoPagamento,
                };

                var detalhesParaSalvar = new List<DetalhePedido>();
                foreach (var item in Carrinho)
                {
                    detalhesParaSalvar.Add(new DetalhePedido
                    {
                        PedidoId = pedido.Id,
                        ProdutoId = item.ProdutoId,
                        Imagem = item.Imagem,
                        NomeProduto = item.Nome,
                        PrecoUnitario = item.Preco,
                        Quantidade = item.Quantidade
                    });
                }

                // Tenta salvar no banco
                await _pedidoService.CriarPedidoCompletoAsync(pedido, detalhesParaSalvar);

                _carrinhoService.ClearCarrinho();

                HttpContext.Session.SetObjectFromJson("PedidoConfirmacao", pedido);
                HttpContext.Session.SetObjectFromJson("EnderecoConfirmacao", enderecoSelecionado);
                HttpContext.Session.SetObjectFromJson("DetalhesConfirmacao", detalhesParaSalvar);
                HttpContext.Session.SetObjectFromJson("CarrinhoTotal", CarrinhoTotal);

                return RedirectToPage("/Pedido/ConfirmacaoPedido");
            }
            catch (Exception ex)
            {
                // RECARREGA A LISTA PARA A TELA NÃO QUEBRAR
                await LoadEnderecosAsync(userId);
                
                // Mostra o erro real para sabermos o que corrigir (ex: table not found)
                TempData["MensagemErro"] = $"Erro ao finalizar: {ex.Message}";
                return Page();
            }
        }
        private async Task LoadEnderecosAsync(int userId)
        {
            // 🚨 Certifique-se de que a lista pública Enderecos é populada aqui!
            Enderecos = await _enderecoService.SelectAllByUserIdAsync(userId);
        }
    }
}