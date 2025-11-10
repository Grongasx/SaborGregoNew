namespace SaborGregoNew.Data
{
    using Microsoft.EntityFrameworkCore;
    using SaborGregoNew.Models;

    public class ApplicationDbContext : DbContext
    {

        //criando conexao com o banco de dados
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        

        //associando os models as tabelas do banco de dados;
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<DetalhePedido> DetalhesPedido { get; set; }
        public DbSet<CarrinhoItem> CarrinhoItens { get; set; }
        public DbSet<Carrinho> Carrinhos { get; set; }
        
        //Ajustes dos modelos para o banco de dados
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //definindo a chave composta no banco de dados;
            modelBuilder.Entity<DetalhePedido>()
                .HasKey(dp => new
                {
                    dp.PedidoId,
                    dp.ProdutoId
                });

            //defininda a precisao dos decimais no banco de dados;
            modelBuilder.Entity<Produto>()
                .Property(p => p.Preco)
                .HasPrecision(10, 2); 
            

            //Definindo os Enumerates no banco de dados;
            modelBuilder.Entity<Produto>()
                .Property(p => p.Categoria)
                .HasConversion<string>();
            modelBuilder.Entity<Pedido>()
                .Property(p => p.Status)
                .HasConversion<string>();
            modelBuilder.Entity<Pedido>()
                .Property(p => p.MetodoPagamento)
                .HasConversion<string>();
            modelBuilder.Entity<Usuario>()
                .Property(p => p.Role)
                .HasConversion<string>();
        }
    }
}