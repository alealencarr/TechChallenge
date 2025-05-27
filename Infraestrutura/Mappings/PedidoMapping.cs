using Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedido");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .IsRequired();

            builder.Property(x => x.IdCliente)
                   .HasColumnName("IdCliente")
                   .IsRequired(false);

            builder.Property(x => x.StatusPedido)
                   .HasColumnName("StatusPedido")
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(50);

            builder.Property(x => x.PrecoPedido)
                   .HasColumnName("PrecoPedido")
                   .HasColumnType("DECIMAL(18,2)")
                   .IsRequired();

            builder.HasOne(x => x.Cliente)
                   .WithMany() // Sem navegação reversa
                   .HasForeignKey(x => x.IdCliente)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(typeof(ItemPedido), "_itens")
                   .WithOne()
                   .HasForeignKey("PedidoId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(x => x.Itens);
        }
    }

}
