using Infrastructure.DbModels;
using Infrastructure.DbModels.OrdersModelsAggregate;
using Infrastructure.DbModels.ProductModelsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Infrastructure.DbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<PaymentDbModel> Payment { get; set; } = null!;

    public DbSet<CustomerDbModel> Customer { get; set; } = null!;
    public DbSet<IngredientSnackDbModel> IngredientSnack { get; set; } = null!;
    public DbSet<ItemOrderDbModel> ItemOrder { get; set; } = null!;
    public DbSet<OrderDbModel> Order { get; set; } = null!;
    public DbSet<IngredientDbModel> Ingredients  { get; set; } = null!;

    public DbSet<ProductDbModel> Product { get; set; } = null!;
    public DbSet<ProductImageDbModel> ProductImages { get; set; } = null!;
    public DbSet<ProductIngredientDbModel> ProductIngredients { get; set; } = null!;
    public DbSet<CategorieDbModel> Categories { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}