using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaborGregoNew.Models;
using SaborGregoNew.DTOs;
using saborGregoNew.Repository;
using SaborGregoNew.DTOs.Pedido;
using saborGregoNew.Repository.Interfaces;
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
            try
            {
                Carrinho = _carrinhoService.GetCarrinho();//tenta pegar o carrinho
                //verifica se ele está vazio
                if (Carrinho == null || !Carrinho.Any())
                {
                    return RedirectToPage("/Pedido/Carrinho/Carrinho");
                }


                CarrinhoTotal = _carrinhoService.CalcularTotal();//calcula o valor total do pedido


                if (CheckoutInfo.EnderecoId <= 0)//verifica se o endereço foi selecionado
                {
                    await LoadEnderecosAsync(User.GetUserId());//recarega os endereços da pagina
                    return Page();
                }


                
                int userId = User.GetUserId();//tenta pegar o id do usuario logado


                var enderecoSelecionado = await _enderecoService.SelectByIdAsync(CheckoutInfo.EnderecoId); //pega as informações com base no id do endereço selecionado
                
                if (enderecoSelecionado == null)//verifica se o endereço selecionado existe
                {
                    await LoadEnderecosAsync(User.GetUserId());//recarrega os endereços na pagina
                    return Page();
                }

                if (!ModelState.IsValid)//verifica os modelo dos objetos da pagina
                {
                    return Page();
                }

                //cria o novo pedido
                var pedido = new PedidoDTO
                {
                    ClienteId = userId,
                    FuncionarioId = null,
                    EntregadorId = null,
                    DataPedido = DateTime.Now,
                    ValorTotal = CarrinhoTotal,
                    Status = 0,

                    EnderecoId = CheckoutInfo.EnderecoId,
                    MetodoPagamento = CheckoutInfo.MetodoPagamento,
                };


                //far um loop para inserir os itens do carrinho em uma lista de detalhes pedido
                var detalhesParaSalvar = new List<DetalhePedido>();//variavel de armazenamento
                foreach (var item in Carrinho)
                {
                    var detalheModelo = new DetalhePedido //variavel para cada item
                    {
                        PedidoId = pedido.Id,
                        ProdutoId = item.ProdutoId,
                        Imagem = item.Imagem,
                        NomeProduto = item.Nome,
                        PrecoUnitario = item.Preco,
                        Quantidade = item.Quantidade
                    };
                    detalhesParaSalvar.Add(detalheModelo);//salva na variavel de armazenamento
                }
                await _pedidoService.CriarPedidoCompletoAsync(pedido, detalhesParaSalvar);//Cria o pedido com os itens no DB

                _carrinhoService.ClearCarrinho();//limpa o carrinho da sessão


                //cria tempDatas para mostrar na tela de cofirmação
                HttpContext.Session.SetObjectFromJson("PedidoConfirmacao", pedido);
                HttpContext.Session.SetObjectFromJson("EnderecoConfirmacao", enderecoSelecionado);
                HttpContext.Session.SetObjectFromJson("DetalhesConfirmacao", detalhesParaSalvar);

                return RedirectToPage("/Pedido/ConfirmacaoPedido");// Redireciona para a página de confirmação do pedido
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro interno ao finalizar a compra. Tente novamente.";
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