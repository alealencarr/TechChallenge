using Microsoft.EntityFrameworkCore;
using Domain;
using System.Reflection;
using Domain.Entidades;
using Domain.Entidades.Agregados.AgregadoPedido;
using Domain.Entidades.Agregados.AgregadoProduto;

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
        .HasKey(pi => new { pi.ProdutoId, pi.IngredienteId });

        modelBuilder.Entity<Cliente>().OwnsOne(c => c.CPF, cpf =>
        {
            cpf.Property(c => c.Valor)
               .HasColumnName("Cpf")
               .IsRequired()
               .HasMaxLength(11);  
        });

    

        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Nome = "Lanche" },
            new Categoria { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Nome = "Acompanhamento" },
            new Categoria { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Nome = "Bebida" },
            new Categoria { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Nome = "Sobremesa" }
        );

        modelBuilder.Entity<Ingrediente>().HasData(
            // Carnes
            new Ingrediente { Id = Guid.Parse("10000000-0000-0000-0000-000000000011"), Nome = "Hambúrguer Angus 200g", Preco = 10.00m },
            new Ingrediente { Id = Guid.Parse("10000000-0000-0000-0000-000000000012"), Nome = "Frango Grelhado", Preco = 7.50m },
            new Ingrediente { Id = Guid.Parse("10000000-0000-0000-0000-000000000013"), Nome = "Carne Desfiada BBQ", Preco = 9.00m },

            // Queijos
            new Ingrediente { Id = Guid.Parse("10000000-0000-0000-0000-000000000014"), Nome = "Queijo Muçarela", Preco = 1.50m },
            new Ingrediente { Id = Guid.Parse("10000000-0000-0000-0000-000000000015"), Nome = "Queijo Gorgonzola", Preco = 2.00m },

            // Extras e Vegetais
            new Ingrediente { Id = Guid.Parse("10000000-0000-0000-0000-000000000016"), Nome = "Ovo Frito", Preco = 1.50m },
            new Ingrediente { Id = Guid.Parse("10000000-0000-0000-0000-000000000017"), Nome = "Rúcula Fresca", Preco = 0.80m },
            new Ingrediente { Id = Guid.Parse("10000000-0000-0000-0000-000000000018"), Nome = "Jalapeño", Preco = 1.00m },

            // Molhos Premium
            new Ingrediente { Id = Guid.Parse("10000000-0000-0000-0000-000000000019"), Nome = "Barbecue Defumado", Preco = 1.00m },
            new Ingrediente { Id = Guid.Parse("10000000-0000-0000-0000-000000000020"), Nome = "Mostarda e Mel", Preco = 1.00m },
            new Ingrediente { Id = Guid.Parse("10000000-0000-0000-0000-000000000021"), Nome = "Pimenta Chipotle", Preco = 1.20m },

            // Extras Gourmet
            new Ingrediente { Id = Guid.Parse("10000000-0000-0000-0000-000000000022"), Nome = "Cebola Crispy", Preco = 1.20m },
            new Ingrediente { Id = Guid.Parse("10000000-0000-0000-0000-000000000023"), Nome = "Tomate Confit", Preco = 1.50m },
            new Ingrediente { Id = Guid.Parse("10000000-0000-0000-0000-000000000024"), Nome = "Molho Trufado", Preco = 2.50m }
        );

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}