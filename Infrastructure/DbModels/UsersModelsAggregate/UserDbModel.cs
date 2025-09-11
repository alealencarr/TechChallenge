namespace Infrastructure.DbModels.UsersModelsAggregate
{
    public class UserDbModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string PasswordHash { get; set; }

        public string? RefreshToken { get;  set; }
        public DateTime? RefreshTokenExpiryTime { get;  set; }


        public List<UserRoleDbModel> UserRoles { get; set; } = new List<UserRoleDbModel>();

        public UserDbModel(Guid id, string name, string mail, string passwordHash, List<UserRoleDbModel> userRoles, string? refreshToken, DateTime? refreshTokenExpiryTime)
        {
            Id = id;
            Name = name;
            Mail = mail;
            PasswordHash = passwordHash;
            UserRoles = userRoles;
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }

        protected UserDbModel() { }

    }
}
