using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelsConfiguration
{
    public class UserDbModelConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("User");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.CreatedAt)
                 .IsRequired();

            entity.Property(p => p.ClientId)
                  .IsRequired();

            entity.Property(p => p.ClientSecret)
                  .IsRequired();
        }
    }
}

