using Infrastructure.DbModels.ProductModelsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelsConfiguration.ProductAggregateConfiguration
{
    public class ProductImageDbModelConfiguration : IEntityTypeConfiguration<ProductImageDbModel>
    {
        public void Configure(EntityTypeBuilder<ProductImageDbModel> entity)
        {
            entity.ToTable("ProductImage");            

            entity.HasKey(pi => new { pi.ProductId, pi.Id });

            entity.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.FileName)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.MimeType)
             .HasColumnType("NVARCHAR(MAX)")
             .IsRequired();

            entity.Property(x => x.ImagePath)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.Blob)
                .IsRequired();

            // Relação N:1 ProductImage -> Product
            entity.HasOne(x => x.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
