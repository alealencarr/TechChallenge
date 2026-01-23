using Infrastructure.DbModels.OrdersModelsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.ModelsConfiguration.OrdersAggregateConfiguration
{
    [ExcludeFromCodeCoverage]
    public class IngredientSnackDbModelConfiguration : IEntityTypeConfiguration<IngredientSnackDbModel>
    {
        public void Configure(EntityTypeBuilder<IngredientSnackDbModel> entity)
        {
            entity.ToTable("IngredientSnack");

            entity.HasKey(i => i.Id);

            entity.Property(i => i.Id)
                .IsRequired();

            entity.Property(i => i.IdIngredient)
                .IsRequired();

            entity.Property(i => i.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(i => i.Additional)
                .IsRequired();

            entity.Property(i => i.Quantity)
                .IsRequired();

            entity.Property(i => i.ItemId)
                .IsRequired();

            entity.HasOne(i => i.ItemOrder)
                .WithMany(io => io.IngredientsSnack)
                .HasForeignKey(i => i.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

