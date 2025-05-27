using Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Infraestrutura.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produto");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nome)
                .IsRequired(true)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(255);

            builder.Property(x => x.Preco)
                .IsRequired(true)
                .HasColumnType("DECIMAL(12,4)");

            builder.Property(x => x.Descricao)
            .IsRequired(true)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(255);

            builder.Property(p => p.CategoriaId)
                .IsRequired(true);

            builder.HasOne(p => p.Categoria)
                .WithMany()
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.ProdutoImagens)
                    .WithOne(pi => pi.Produto)
                    .HasForeignKey(pi => pi.IdProduto)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.ProdutoIngredientes)
                .WithOne(pi => pi.Produto)
                .HasForeignKey(pi => pi.IdProduto);
        }
 
    }
}
