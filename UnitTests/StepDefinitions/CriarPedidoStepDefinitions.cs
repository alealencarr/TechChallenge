using Application.Gateways;
using Application.Interfaces.DataSources; // Assumindo que suas interfaces estão aqui igual no Customer
using Application.UseCases.Orders;
using Application.UseCases.Orders.Command;
using Domain.Entities.Aggregates.AggregateOrder;
using Domain.Entities.Aggregates.AggregateProduct;
using FluentAssertions;

using Moq;
using Reqnroll;
using Shared.DTO.Categorie.Input;
using Shared.DTO.Ingredient;
using Shared.DTO.Order.Input;
using Shared.DTO.Order.Request;
using Shared.DTO.Product.Input;

namespace Orders.UnitTests.StepDefinitions
{
    [Binding]
    public class CreateOrderSteps
    {

        private readonly Mock<IOrderDataSource> _orderDataSourceMock;
        private readonly Mock<ICustomerDataSource> _customerDataSourceMock;
        private readonly Mock<IProductDataSource> _productDataSourceMock;
        private readonly Mock<IIngredientDataSource> _ingredientDataSourceMock;


        private readonly OrderGateway _orderGateway;
        private readonly CustomerGateway _customerGateway;
        private readonly ProductGateway _productGateway;
        private readonly IngredientGateway _ingredientGateway;

        private readonly CreateOrderUseCase _useCase;

        private OrderCommand _commandInput;
        private  Order _orderResult;
        private Exception _exceptionResult;


        private List<Product> _fakeProductsDb = new();
        private List<IngredientDto> _fakeIngredientsDb = new();
        private Dictionary<string, Guid> _idMap = new();

        public CreateOrderSteps()
        {

            _orderDataSourceMock = new Mock<IOrderDataSource>();
            _customerDataSourceMock = new Mock<ICustomerDataSource>();
            _productDataSourceMock = new Mock<IProductDataSource>();
            _ingredientDataSourceMock = new Mock<IIngredientDataSource>();


            _productDataSourceMock.Setup(x => x.GetByIds(It.IsAny<List<Guid>>()))
              .ReturnsAsync((List<Guid> ids) =>
                  _fakeProductsDb
                      .Where(product => ids.Contains(product.Id))
                      .Select(product => new ProductInputDto(product.Id, product.CreatedAt, product.Name, product.Description, product.Price, product.CategorieId,
                        product.ProductImages.Select(x => new ProductImageInputDto(x.Id, x.FileName, x.MimeType, x.ImagePath, x.Name, x.Blob, x.ProductId)).ToList(),
                        product.ProductIngredients.Select(x => new ProductIngredientInputDto(x.IngredientId, x.Quantity, x.ProductId)).ToList(), product.IsLanche))
                      .ToList());

            _ingredientDataSourceMock.Setup(x => x.GetByIds(It.IsAny<List<Guid>>()))
                .ReturnsAsync((List<Guid> ids) =>
                    _fakeIngredientsDb.Where(dto => ids.Contains(dto.Id)).ToList());

            _orderDataSourceMock.Setup(x => x.Create(It.IsAny<OrderInputDto>()))
                .Returns(Task.CompletedTask);


            _orderGateway = OrderGateway.Create(_orderDataSourceMock.Object);
            _customerGateway = CustomerGateway.Create(_customerDataSourceMock.Object);
            _productGateway = ProductGateway.Create(_productDataSourceMock.Object);
            _ingredientGateway = IngredientGateway.Create(_ingredientDataSourceMock.Object);


            _useCase = CreateOrderUseCase.Create(
                _orderGateway,
                _customerGateway,
                _ingredientGateway,
                _productGateway
            );
        }

        private Guid GetGuid(string alias)
        {
            if (!_idMap.ContainsKey(alias))
                _idMap[alias] = Guid.NewGuid();
            return _idMap[alias];
        }


        [Given("que existe um cliente na base com ID {string}")]
        public void GivenClienteExiste(string idAlias)
        {
            var guid = GetGuid(idAlias);
            _customerDataSourceMock.Setup(x => x.GetById(guid))
               .ReturnsAsync(new CustomerDto(guid, DateTime.Now, "11111111111", "Teste", "mail@mail.com", true));
        }

        [Given("que o cliente com ID {string} nao existe")]
        public void GivenClienteNaoExiste(string idAlias)
        {
            var guid = GetGuid(idAlias);
            _customerDataSourceMock.Setup(x => x.GetById(guid))
                .ReturnsAsync((CustomerDto?)null);
        }

        [Given("que existe um produto com ID {string} Nome {string} e Preco {decimal}")]
        public void GivenProdutoExiste(string idAlias, string nome, decimal preco)
        {
            var guid = GetGuid(idAlias);

            var product = new Product(
                guid,
                nome,
                "Descricao",
                preco,
                Guid.NewGuid(),
                DateTime.Now,
                new List<ProductIngredient>(),
                new List<ProductImage>(),
                false
            );
            _fakeProductsDb.Add(product);
        }

        [Given("o produto com ID {string} nao existe na base")]
        public void GivenProdutoNaoExiste(string idAlias)
        {
            GetGuid(idAlias);
        }

        [Given("que existe um ingrediente com ID {string} Nome {string} e Preco {decimal}")]
        public void GivenIngredienteExiste(string idAlias, string nome, decimal preco)
        {
            var guid = GetGuid(idAlias);
            var ingredient = new IngredientDto(guid, DateTime.Now, nome, preco);
            _fakeIngredientsDb.Add(ingredient);
        }

        [Given("o ingrediente com ID {string} nao existe na base")]
        public void GivenIngredienteNaoExiste(string idAlias)
        {
            GetGuid(idAlias);
        }


        [When("eu monto um pedido para o cliente {string}")]
        public void WhenMontoPedidoComCliente(string idAlias)
        {
            _commandInput = new OrderCommand(
                GetGuid(idAlias),
                new List<ItemOrderRequestDto>()
            );
        }

        [When("eu monto um pedido sem cliente identificado")]
        public void WhenMontoPedidoAnonimo()
        {
            _commandInput = new OrderCommand(
                null,
                new List<ItemOrderRequestDto>()
            );
        }

        [When("adiciono {int} unidade do produto {string}")]
        [When("adiciono {int} unidades do produto {string}")]
        public void WhenAdicionoProduto(int qtd, string idAlias)
        {
            var item = new ItemOrderRequestDto
            {
                Id = GetGuid(idAlias),
                Quantity = qtd,
                IngredientsSnack = new List<IngredientSnackRequestDto>()
            };
            _commandInput.Itens.Add(item);
        }

        [When("adiciono o ingrediente {string} no produto {string}")]
        public void WhenAdicionoIngrediente(string idIngrediente, string idProduto)
        {
            var prodGuid = GetGuid(idProduto);
            var ingGuid = GetGuid(idIngrediente);

            var item = _commandInput.Itens.FirstOrDefault(x => x.Id == prodGuid);
            if (item != null)
            {
                item.IngredientsSnack.Add(new IngredientSnackRequestDto { Id = ingGuid, Quantity = 1 });
            }
        }


        private async Task EnsureUseCaseExecuted()
        {
            if (_orderResult == null && _exceptionResult == null)
            {
                try
                {
                    _orderResult = await _useCase.Run(_commandInput);
                }
                catch (Exception ex)
                {
                    _exceptionResult = ex;
                }
            }
        }

        [Then("o pedido deve ser salvo com sucesso")]
        public async Task ThenPedidoSalvo()
        {
            await EnsureUseCaseExecuted();

            if (_exceptionResult != null)
                throw new Exception($"Deveria ter sucesso, mas falhou com: {_exceptionResult.Message}");

            _orderResult.Should().NotBeNull();
            _orderResult.Id.Should().NotBeEmpty();
        }

        [Then("o gateway de pedidos deve ter sido chamado")]
        public async Task ThenGatewayChamado()
        {
            await EnsureUseCaseExecuted();

            _orderDataSourceMock.Verify(x => x.Create(It.IsAny<OrderInputDto>()), Times.Once);
        }

        [Then("o sistema deve retornar um erro contendo {string} e {string}")]
        public async Task ThenErroContendoDuasStrings(string parte1, string parte2)
        {
            await EnsureUseCaseExecuted();

            _exceptionResult.Should().NotBeNull("Deveria ter retornado erro, mas foi sucesso.");
            _exceptionResult.Message.Should().Contain(parte1);
            _exceptionResult.Message.Should().Contain(parte2);
        }

        [Then("o sistema deve retornar um erro contendo {string}")]
        public async Task ThenErroContendo(string mensagem)
        {
            await EnsureUseCaseExecuted();

            _exceptionResult.Should().NotBeNull("Deveria ter retornado erro, mas foi sucesso.");
            _exceptionResult.Message.Should().Contain(mensagem);
        }
    }
}