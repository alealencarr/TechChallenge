using Domain.Entities.Aggregates.AggregateUser;
using IdentityModel;
using Infrastructure.DbModels.UsersModelsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelsConfiguration.RolesAggregateConfiguration;


public class RoleDbModelConfiguration : IEntityTypeConfiguration<RoleDbModel>
{
    public void Configure(EntityTypeBuilder<RoleDbModel> builder)
    {
        builder.ToTable("Role");
 
        builder.HasKey(r => r.Id);
        builder.Property(x => x.Id)
            .HasDefaultValueSql("NEWID()");
        // Garante que o Nome do papel é obrigatório e único
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(r => r.Name)
            .IsUnique();

        // Configura a relação com UserRole
        builder.HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();
 
    }
}