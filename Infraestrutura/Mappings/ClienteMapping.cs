﻿using Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Mapping
{
    public class ClienteMapping : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Cliente");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .IsRequired(false)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(255);

            builder.OwnsOne(x => x.CPF, cpf => //Mapeando Value O.
            {
                cpf.Property(x => x.Valor).HasColumnName("Cpf")
                .IsRequired(true)
                .HasColumnType("VARCHAR")
                .HasMaxLength(11);
            });


            builder.Property(x => x.Nome)
                .IsRequired(false)
                .HasColumnType("VARCHAR")
                .HasMaxLength(100);
        }
    }
   
}
