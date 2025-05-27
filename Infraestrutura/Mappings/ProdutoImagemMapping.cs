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
    public class ProdutoImagemMapping : IEntityTypeConfiguration<ProdutoImagem>
    {
        public void Configure(EntityTypeBuilder<ProdutoImagem> builder)
        {
            builder.ToTable("ProdutoImagem");

            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.Id)
                   .IsRequired();

            builder.Property(pi => pi.Blob)
                   .IsRequired();

            builder.Property(pi => pi.IdProduto)
                   .IsRequired();

            builder.HasOne(pi => pi.Produto)
                   .WithMany(p => p.ProdutoImagens)
                   .HasForeignKey(pi => pi.IdProduto)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}