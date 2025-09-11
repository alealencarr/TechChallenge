using Infrastructure.DbModels.UsersModelsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelsConfiguration.UsersAggregateConfiguration;

public class UserDbModelConfiguration : IEntityTypeConfiguration<UserDbModel>
{
    public void Configure(EntityTypeBuilder<UserDbModel> entity)
    {
        entity.ToTable("User");
        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .HasDefaultValueSql("NEWID()");

        entity.Property(x => x.Name)
            .HasMaxLength(120)
            .IsRequired(true);

        entity.Property(x => x.Mail)
            .HasMaxLength(150)
            .IsRequired(true);
        entity.HasIndex(u => u.Mail)
              .IsUnique();
        entity.HasIndex(u => u.Mail)
              .IsUnique();


        entity.Property(x => x.RefreshToken)
        .HasMaxLength(300)
        .IsRequired(false);

        entity.Property(x => x.RefreshTokenExpiryTime)
        .IsRequired(false);

        entity.HasMany(u => u.UserRoles)
               .WithOne(ur => ur.User)
               .HasForeignKey(ur => ur.UserId)
               .IsRequired();
    }
}