using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Aggregates.AggregateUser
{
    public class User
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }  
        public string Mail { get; set; } 
        public string PasswordHash { get; set; }

        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiryTime { get; private set; }

        // Propriedade de navegação para a relação muitos-para-muitos com Role
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public User(Guid id, string name, string mail, string passwordHash, ICollection<UserRole> userRoles, string? refreshToken, DateTime? refreshTokenExpiryTime)  
        {
            Id = id;
            Name = name;
            Mail = mail;
            PasswordHash = passwordHash;
            UserRoles = userRoles;
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }

        public User(string name, string mail, string passwordHash)
        {
            Id = Guid.NewGuid();
            Name = name;
            Mail = mail;
            PasswordHash = passwordHash;
            UserRoles = new List<UserRole>
            {
                new UserRole(Id,Guid.Parse("00000000-0000-0000-0000-000000000002"), new Role(Guid.Parse("00000000-0000-0000-0000-000000000002"), "Admin") )
            };

        }

        public void SetRefreshToken(string refresh)
        {
            RefreshToken = refresh;
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
        }
    }
}
