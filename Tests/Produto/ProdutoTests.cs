using Aplicacao.Services;
using Aplicacao.UseCases.Produto.Criar;
using Domain.Ports;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Tests.Produto
{
    public class CreateProdutoTests
    {
        //Teste depende do Banco de Dados por conta das categorias..

        //[Fact]
        //public async Task CriarProduto_TipoLancheSemIngredientes_DeveRetornarBadRequest()
        //{
        //    var repositoryMock = new Mock<IProdutoRepository>();
        //    var repositoryMockCategory = new Mock<ICategoriaRepository>();
        //    var repositoryMockIngrediente = new Mock<IIngredienteRepository>();
        //    var repositoryMockFileSaver = new Mock<IFileSaver>();
        //    var repositoryMockHttpContext = new Mock<IHttpContextAccessor>();

        //    repositoryMock.Setup(r => r.Adicionar(It.IsAny<Domain.Entidades.Agregados.AgregadoProduto.Produto>()))
        //                  .Returns(Task.CompletedTask);

        //    var handler = new CriarHandler(repositoryMock.Object, repositoryMockCategory.Object, repositoryMockIngrediente.Object, repositoryMockFileSaver.Object, repositoryMockHttpContext.Object);

        //    var command = new CriarCommand("Lanche", 1M, Guid.Parse("00000000-0000-0000-0000-000000000001"), [], "Lanche Médio", null);

        //    var result = await handler.Handle(command);

        //    result.Should().NotBeNull();
        //    result.IsSucess.Should().BeFalse();
        //    result.Data.Should().BeNull();
        //    result.Message.Should().Be("É necessário informar pelo menos um ingrediente para criar um produto do tipo Lanche.");

        //}

    }
}

