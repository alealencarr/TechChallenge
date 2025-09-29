using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Authentication.Input
{
    public record UserRolesInputDto
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public RoleInputDto Role { get; set; } = null!;

    }
}
