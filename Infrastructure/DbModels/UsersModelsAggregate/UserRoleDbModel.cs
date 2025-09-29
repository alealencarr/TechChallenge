namespace Infrastructure.DbModels.UsersModelsAggregate
{
    public class UserRoleDbModel
    {
        public Guid UserId { get; set; }
        public UserDbModel User { get; set; } = null!;

        public Guid RoleId { get; set; }
        public RoleDbModel Role { get; set; } = null!;

        public UserRoleDbModel(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        protected UserRoleDbModel() { }

    }
}
