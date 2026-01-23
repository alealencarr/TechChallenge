using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.ModelsConfiguration.ProductAggregateConfiguration
{
    [ExcludeFromCodeCoverage]
    public class ProductDbModelConfiguration : IEntityTypeConfiguration<ProductDbModel>
    {
        public void Configure(EntityTypeBuilder<ProductDbModel> entity)
        {
            entity.ToTable("Product");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Price)
                .HasColumnType("decimal(18,6)")
                .IsRequired();

            entity.Property(x => x.Description)
                .HasMaxLength(100)
                .IsRequired(false);

            entity.Property(x => x.CreatedAt)
                .IsRequired();
 
            // Relações 1:N para ProductImages e ProductIngredients
            entity.HasMany(x => x.ProductImages)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.ProductIngredients)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
