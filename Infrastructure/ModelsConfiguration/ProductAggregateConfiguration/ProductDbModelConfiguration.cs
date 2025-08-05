using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelsConfiguration.ProductAggregateConfiguration
{
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

            // Relação N:1 Product -> Categorie
            entity.HasOne(x => x.Categorie)
                .WithMany(c => c.Products)
                .HasForeignKey(x => x.CategorieId)
                .OnDelete(DeleteBehavior.Restrict);

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
