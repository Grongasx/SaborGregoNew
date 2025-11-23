namespace SaborGregoNew.Data
{
    using Microsoft.EntityFrameworkCore;
    using SaborGregoNew.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<ItensPedido> ItensPedidos { get; set; }
        public DbSet<CarrinhoItem> CarrinhoItens { get; set; }
        public DbSet<Carrinho> Carrinhos { get; set; }
        public DbSet<DetalhePedido> DetalhePedido { get; set; }
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>()
                .Property(p => p.TotalPedido)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Produto>()
                .Property(p => p.Preco)
                .HasPrecision(10, 2); 

            modelBuilder.Entity<ItensPedido>()
                .HasKey(ip => new { ip.PedidoId, ip.ProdutoId });

            modelBuilder.Entity<DetalhePedido>()
                .HasKey(dp => new { dp.PedidoId, dp.ProdutoId });

            base.OnModelCreating(modelBuilder);
        }
    }
}