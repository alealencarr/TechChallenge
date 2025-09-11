using Infrastructure.DbModels.UsersModelsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelsConfiguration.UsersAggregateConfiguration
{

    public class UserRoleDbModelConfiguration : IEntityTypeConfiguration<UserRoleDbModel>
    {
        public void Configure(EntityTypeBuilder<UserRoleDbModel> builder)
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });
        }
    }
}
