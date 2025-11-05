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
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>()
                .Property(p => p.ValorTotal)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Produto>()
                .Property(p => p.Preco)
                // Define a precisão total como 18 e a escala como 2 (duas casas decimais)
                .HasPrecision(10, 2); 
                
            // ...

            base.OnModelCreating(modelBuilder);
        }
    }
    
}