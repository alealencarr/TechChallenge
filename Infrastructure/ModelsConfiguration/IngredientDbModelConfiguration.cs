using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelsConfiguration
{
    public class IngredientDbModelConfiguration : IEntityTypeConfiguration<IngredientDbModel>
    {
        public void Configure(EntityTypeBuilder<IngredientDbModel> entity)
        {
            entity.ToTable("Ingredient");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            // Relação 1:N Ingredient -> ProductIngredients
            entity.HasMany(x => x.ProductIngredients)
                .WithOne(pi => pi.Ingredient)
                .HasForeignKey(pi => pi.IngredientId)
                .OnDelete(DeleteBehavior.Restrict); // para evitar exclusão em cascata
        }
    }
}
