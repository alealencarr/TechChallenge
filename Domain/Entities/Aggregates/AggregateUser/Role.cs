namespace Domain.Entities.Aggregates.AggregateUser
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Propriedade de navegação
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public Role(Guid id, string name)
        {
            Id = id;
            Name = name;            
        }
    }
}
