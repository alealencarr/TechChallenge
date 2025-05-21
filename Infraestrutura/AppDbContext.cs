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
    public DbSet<Cliente> Clientes { get; set; } = null!;
 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}