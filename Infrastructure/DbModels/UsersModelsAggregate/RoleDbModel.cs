namespace Infrastructure.DbModels.UsersModelsAggregate
{
    public class RoleDbModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<UserRoleDbModel> UserRoles { get; set; } = new List<UserRoleDbModel>();
        public RoleDbModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        protected RoleDbModel() { }
    }
}
