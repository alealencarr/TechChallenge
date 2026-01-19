using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Orders.UnitTests.Domain.Entities
{
    public class UserTests
    {
        [Fact]
        public void Deve_Criar_Usuario_Com_Sucesso()
        {
            var clientId = "123";
            var clientSecret = "secret";
            var name = "Totem";

            var user = new User(clientId, clientSecret, name);

            user.Should().NotBeNull();
            user.Id.Should().NotBeEmpty();
            user.ClientId.Should().Be(clientId);
            user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }
    }
}