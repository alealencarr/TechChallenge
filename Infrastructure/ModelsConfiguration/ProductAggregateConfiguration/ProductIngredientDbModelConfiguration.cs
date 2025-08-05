using Infrastructure.DbModels.ProductModelsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelsConfiguration.ProductAggregateConfiguration
{
    public class ProductIngredientDbModelConfiguration : IEntityTypeConfiguration<ProductIngredientDbModel>
    {
        public void Configure(EntityTypeBuilder<ProductIngredientDbModel> entity)
        {
            entity.ToTable("ProductIngredient");

            // Chave primária composta (ProductId + IngredientId)
            entity.HasKey(pi => new { pi.ProductId, pi.IngredientId });

            entity.Property(pi => pi.Quantity)                
                .IsRequired();

            // Relacionamento com ProductDbModel
            entity.HasOne(pi => pi.Product)
                .WithMany(p => p.ProductIngredients)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com IngredientDbModel
            entity.HasOne(pi => pi.Ingredient)
                .WithMany(i => i.ProductIngredients)
                .HasForeignKey(pi => pi.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
