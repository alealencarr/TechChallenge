using Infrastructure.DbModels;
using Infrastructure.DbModels.OrdersModelsAggregate;
using Infrastructure.DbModels.ProductModelsAggregate;
using Infrastructure.DbModels.UsersModelsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Infrastructure.DbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<UserRoleDbModel> UserRoles { get; set; } = null!;
    public DbSet<RoleDbModel> Role { get; set; } = null!;
    public DbSet<UserDbModel> User { get; set; } = null!;
    public DbSet<PaymentDbModel> Payment { get; set; } = null!;

    public DbSet<CustomerDbModel> Customer { get; set; } = null!;
    public DbSet<IngredientSnackDbModel> IngredientSnack { get; set; } = null!;
    public DbSet<ItemOrderDbModel> ItemOrder { get; set; } = null!;
    public DbSet<OrderDbModel> Order { get; set; } = null!;
    public DbSet<IngredientDbModel> Ingredient  { get; set; } = null!;

    public DbSet<ProductDbModel> Product { get; set; } = null!;
    public DbSet<ProductImageDbModel> ProductImages { get; set; } = null!;
    public DbSet<ProductIngredientDbModel> ProductIngredients { get; set; } = null!;
    public DbSet<CategorieDbModel> Categorie { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}