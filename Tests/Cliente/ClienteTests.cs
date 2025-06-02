using Aplicacao.UseCases.Cliente.Criar;
using Domain.Ports;
using FluentAssertions;
using Moq;

namespace Tests.Cliente
{
    public class CreateClienteTests
    {

        [Fact]
        public async Task CriarHandler_ComDadosValidos_DeveCriarClienteComSucesso()
        {

            var repositoryMock = new Mock<IClienteRepository>();
            repositoryMock.Setup(r => r.Adicionar(It.IsAny<Domain.Entidades.Cliente>()))
                          .Returns(Task.CompletedTask);

            var handler = new CriarHandler(repositoryMock.Object);

            var command = new CriarCommand("75298835007", "João", "joao@email.com");


            var result = await handler.Handler(command);


            result.Should().NotBeNull();
            result.IsSucess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.CPF.Should().Be("75298835007");
            result.Message.Should().Be("Cliente Criado com sucesso.");
        }

        [Fact]
        public async Task CriarCliente_ComCpfInvalido_DeveRetornarBadRequest()
        {
            var repositoryMock = new Mock<IClienteRepository>();
            repositoryMock.Setup(r => r.Adicionar(It.IsAny<Domain.Entidades.Cliente>()))
                          .Returns(Task.CompletedTask);

            var handler = new CriarHandler(repositoryMock.Object);

            var command = new CriarCommand("12345678900", "João", "joao@email.com");

            var result = await handler.Handler(command);


            result.Should().NotBeNull();
            result.IsSucess.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Message.Should().Be("CPF inválido.");

        }

        [Fact]
        public async Task CriarCliente_ComCpfVazio_DeveRetornarBadRequest()
        {
            var repositoryMock = new Mock<IClienteRepository>();
            repositoryMock.Setup(r => r.Adicionar(It.IsAny<Domain.Entidades.Cliente>()))
                          .Returns(Task.CompletedTask);

            var handler = new CriarHandler(repositoryMock.Object);

            var command = new CriarCommand("", "João", "joao@email.com");

            var result = await handler.Handler(command);


            result.Should().NotBeNull();
            result.IsSucess.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Message.Should().Be("CPF precisa ser informado.");

        }




    }


}
