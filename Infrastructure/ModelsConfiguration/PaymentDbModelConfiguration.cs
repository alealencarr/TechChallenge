using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelsConfiguration
{
    public class PaymentDbModelConfiguration : IEntityTypeConfiguration<PaymentDbModel>
    {
        public void Configure(EntityTypeBuilder<PaymentDbModel> entity)
        {
            entity.ToTable("Payments");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Amount)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)");

            entity.Property(p => p.PaidAt)
                  .IsRequired(false);

            entity.Property(p => p.CreatedAt)
                 .IsRequired();

            entity.Property(p => p.PaymentMethod)
                  .IsRequired();

            entity.Property(p => p.PaymentStatus)
                  .IsRequired();

            entity.Property(p => p.Processed);

            entity.Property(p => p.QrBytes);
            entity.Property(p => p.FileName).HasMaxLength(200);
            entity.Property(p => p.PathRoot).HasMaxLength(200);

            entity.HasOne(p => p.Order)
          .WithMany(o => o.Payments)  // coleção de pagamentos na Order
          .HasForeignKey(p => p.OrderId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

        }
    }

}

 
 