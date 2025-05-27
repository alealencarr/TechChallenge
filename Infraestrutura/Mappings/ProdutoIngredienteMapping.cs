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
    public class ProdutoIngredienteMapping : IEntityTypeConfiguration<ProdutoIngrediente>
    {
        public void Configure(EntityTypeBuilder<ProdutoIngrediente> builder)
        {
            builder.ToTable("ProdutoIngrediente");

            builder.Property(pi => pi.IdProduto)
                   .IsRequired();

            builder.Property(pi => pi.IdIngrediente)
                   .IsRequired();

            builder.HasOne(pi => pi.Produto)
                   .WithMany(p => p.ProdutoIngredientes)
                   .HasForeignKey(pi => pi.IdProduto)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pi => pi.Ingrediente)
                   .WithMany()
                   .HasForeignKey(pi => pi.IdIngrediente)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
