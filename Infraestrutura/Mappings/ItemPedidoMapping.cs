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
    public class ItemPedidoMapping : IEntityTypeConfiguration<ItemPedido>
    {
        public void Configure(EntityTypeBuilder<ItemPedido> builder)
        {
            builder.ToTable("ItemPedido");

            builder.HasKey(x => x.ItemId);

            builder.Property(x => x.ItemId)
                   .HasColumnName("ItemId")
                   .IsRequired();

            builder.Property(x => x.ProdutoId)
                   .HasColumnName("ProdutoId")
                   .IsRequired();

            builder.Property(x => x.PedidoId)
                   .HasColumnName("PedidoId")
                   .IsRequired();

            builder.Property(x => x.Preco)
                   .HasColumnName("Preco")
                   .HasColumnType("DECIMAL(18,2)")
                   .IsRequired();

            builder.HasMany(typeof(IngredienteLanche), "_ingredientes")
                   .WithOne()
                   .HasForeignKey("ItemId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(x => x.Ingredientes);
        }
    }

}
