using Microsoft.EntityFrameworkCore;
using Domain;
using System.Reflection;
using Domain.Entidades;

namespace Infraestrutura;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Ingrediente> Ingredientes { get; set; } = null!;

    public DbSet<Cliente> Clientes { get; set; } = null!;

    public DbSet<IngredienteLanche> IngredientesLanche { get; set; } = null!;

    public DbSet<ItemPedido> ItensPedido { get; set; } = null!;

    public DbSet<ProdutoImagem> ProdutoImagens { get; set; } = null!;

    public DbSet<ProdutoIngrediente> ProdutoIngredientes { get; set; } = null!;

    public DbSet<Categoria> Categorias { get; set; } = null!;
    public DbSet<Pedido> Pedidos { get; set; } = null!;
    public DbSet<Produto> Produtos { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProdutoIngrediente>()
        .HasKey(pi => new { pi.IdProduto, pi.IdIngrediente });

        modelBuilder.Entity<Cliente>().OwnsOne(c => c.CPF, cpf =>
        {
            cpf.Property(c => c.Valor)
               .HasColumnName("Cpf")
               .IsRequired()
               .HasMaxLength(11);  
        });

        modelBuilder.Entity<Categoria>().HasData(new Categoria("Lanche"));
        modelBuilder.Entity<Categoria>().HasData(new Categoria("Acompanhamento"));
        modelBuilder.Entity<Categoria>().HasData(new Categoria("Bebida"));
        modelBuilder.Entity<Categoria>().HasData(new Categoria("Sobremesa"));

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}