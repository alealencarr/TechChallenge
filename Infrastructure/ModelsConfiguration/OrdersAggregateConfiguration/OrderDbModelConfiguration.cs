using Infrastructure.DbModels;
using Infrastructure.DbModels.OrdersModelsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Infrastructure.ModelsConfiguration.OrdersAggregateConfiguration;

public class OrderDbModelConfiguration : IEntityTypeConfiguration<OrderDbModel>
{
    public void Configure(EntityTypeBuilder<OrderDbModel> entity)
    {
        entity.ToTable("Orders");

        entity.HasKey(o => o.Id);

        entity.Property(o => o.Id)
            .IsRequired();

        entity.Property(o => o.CreatedAt)
            .IsRequired();

        entity.Property(o => o.StatusPedido)
            .IsRequired();

        entity.Property(o => o.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        entity.Property(o => o.CustomerId)
            .IsRequired(false);

        entity.Property(o => o.PaymentId)
           .IsRequired(false);

        // Relação 1:N entre Order e Payments (todos os pagamentos)
        entity.HasMany(o => o.Payments)
              .WithOne(p => p.Order)
              .HasForeignKey(p => p.OrderId)
              .IsRequired();

        // Relação 1:1 (opcional) entre Order e o Payment atual via PaymentId
        entity.HasOne(o => o.Payment)
              .WithMany()                // sem coleção inversa, só 1 pagamento referenciado
              .HasForeignKey(o => o.PaymentId)
              .IsRequired(false)         // pode ser nulo (sem pagamento atual)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);

        entity.HasMany(o => o.Itens)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}