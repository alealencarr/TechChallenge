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
    public class IngredienteLancheMapping : IEntityTypeConfiguration<IngredienteLanche>
    {
        public void Configure(EntityTypeBuilder<IngredienteLanche> builder)
        {
            builder.ToTable("IngredienteLanche");

            builder.HasKey(x => new { x.IdIngrediente, x.ItemId });

            builder.Property(x => x.IdIngrediente)
                   .HasColumnName("IdIngrediente")
                   .IsRequired();

            builder.Property(x => x.ItemId)
                   .HasColumnName("ItemId")
                   .IsRequired();

            builder.Property(x => x.Preco)
                   .HasColumnName("Preco")
                   .HasColumnType("DECIMAL(18,2)")
                   .IsRequired();

            builder.Property(x => x.Adicional)
                   .HasColumnName("Adicional")
                   .IsRequired();
        }
    }

}
