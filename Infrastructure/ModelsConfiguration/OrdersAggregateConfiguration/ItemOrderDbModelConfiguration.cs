using Infrastructure.DbModels.OrdersModelsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelsConfiguration.OrdersAggregateConfiguration
{
    public class ItemOrderDbModelConfiguration : IEntityTypeConfiguration<ItemOrderDbModel>
    {
        public void Configure(EntityTypeBuilder<ItemOrderDbModel> entity)
        {
            entity.ToTable("ItemOrder");

            entity.HasKey(io => io.Id);

            entity.Property(io => io.Id)
                .IsRequired();

            entity.Property(io => io.OrderId)
                .IsRequired();

            entity.Property(io => io.ProductId)
                .IsRequired();

            entity.Property(io => io.Quantity)
                .IsRequired();

            entity.Property(io => io.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.HasOne(io => io.Order)
                .WithMany(o => o.Itens)
                .HasForeignKey(io => io.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com IngredientsSnack, se for composição:
            entity.HasMany(io => io.IngredientsSnack)
                .WithOne(ing => ing.ItemOrder)
                .HasForeignKey(ing => ing.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
