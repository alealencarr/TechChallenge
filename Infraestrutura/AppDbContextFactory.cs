using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infraestrutura
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            
            var projectPath = Path.Combine(Directory.GetCurrentDirectory(), "../API");

            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddUserSecrets<Infraestrutura.AppDbContextFactory>() // carrega os user-secrets
                .Build();

            var connectionString = config.GetConnectionString("minhaconnectionstring");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
