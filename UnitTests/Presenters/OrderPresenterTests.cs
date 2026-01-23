using Application.Presenter.Orders;
using Domain.Entities;
using Domain.Entities.Aggregates.AggregateOrder;
using Domain.Entities.Enums;  
using FluentAssertions;
using Xunit;

namespace Orders.UnitTests.Application.Presenter
{
    public class OrderPresenterTests
    {
        private readonly OrderPresenter _presenter;
        private const string DefaultMessage = "Mensagem de Teste";

        public OrderPresenterTests()
        {
            _presenter = new OrderPresenter(DefaultMessage);
        }

        #region Basic CommandResult Tests

        [Fact]
        public void RetornoSucess_DeveRetornarSucesso_ComMensagemConfigurada()
        {
            var result = _presenter.RetornoSucess();

            result.Succeeded.Should().BeTrue();
            result.Messages[0].Should().Be(DefaultMessage);
        }

        [Fact]
        public void Error_Generico_DeveRetornarFalha_ComMensagemInformada()
        {
            var msgErro = "Erro de validação";
            var result = _presenter.Error<object>(msgErro);

            result.Succeeded.Should().BeFalse();
            result.Messages[0].Should().Be(msgErro);
            result.Data.Should().BeNull();
        }

        [Fact]
        public void Error_Simples_DeveRetornarFalha_ComMensagemInformada()
        {
            var msgErro = "Erro simples";
            var result = _presenter.Error(msgErro);

            result.Succeeded.Should().BeFalse();
            result.Messages[0].Should().Be(msgErro);
        }

        [Fact]
        public void Success_Simples_DeveRetornarSucesso_SemMensagemEspecifica()
        {
            var result = _presenter.Success();

            result.Succeeded.Should().BeTrue();
        }

        #endregion

        #region Summary Tests

        [Fact]
        public void TransformObjectSummary_DeveMapearPropriedadesCorretamente()
        {

            var orderId = Guid.NewGuid();

            var order = new Order(orderId, DateTime.Now, new List<ItemOrder>(), null, EStatusOrder.EmPreparacao, 20M, null);


            var result = _presenter.TransformObjectSummary(order);


            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(order.Id);
            result.Data.Price.Should().Be(order.Price);
            result.Data.CreatedAt.Should().Be(order.CreatedAt);


            result.Data.OrderStatus.Id.Should().Be((int)order.StatusOrder);
            result.Data.OrderStatus.Description.Should().Be(order.StatusOrder.ToString());

            result.Messages[0].Should().Be(DefaultMessage);
        }

        [Fact]
        public void TransformListSummary_DeveMapearListaDePedidos()
        {

            var order1 = new Order(Guid.NewGuid(), DateTime.Now, new List<ItemOrder>(), null, EStatusOrder.Finalizado, 50M, null);
            var order2 = new Order(Guid.NewGuid(), DateTime.Now, new List<ItemOrder>(), null, EStatusOrder.Pronto, 30M, null);
            var list = new List<Order> { order1, order2 };


            var result = _presenter.TransformListSummary(list);


            result.Succeeded.Should().BeTrue();
            result.Data.Should().HaveCount(2);
            result.Data[0].Id.Should().Be(order1.Id);
            result.Data[1].Id.Should().Be(order2.Id);
        }

        #endregion

        #region Payment Tests

        [Fact]
        public void TransformObjectPaymentStatus_DeveMapearPagamentoEEnums()
        {

            var paymentId = Guid.NewGuid();
            var orderId = Guid.NewGuid();



            var payment = new Payment(orderId, 150.50m, new byte[] {1,2});

             var result = _presenter.TransformObjectPaymentStatus(payment);

            result.Succeeded.Should().BeTrue();
            result.Data.Id.Should().Be(payment.Id);
            result.Data.Amount.Should().Be(payment.Amount);

            result.Data.PaymentMethod.Id.Should().Be((int)payment.PaymentMethod);
            result.Data.PaymentMethod.Description.Should().Be(payment.PaymentMethod.ToString());

            result.Data.PaymentStatus.Id.Should().Be((int)payment.PaymentStatus);
            result.Data.PaymentStatus.Description.Should().Be(payment.PaymentStatus.ToString());
        }

        #endregion

 
        #region OrderCompleted (Detailed) Tests

        [Fact]
        public void TransformObject_DeveMapearOrdemCompletaComItensEIngredientes()
        {
             var ingredientId = Guid.NewGuid();
            var itemIngredients = new List<IngredientSnack>
            {
                 new IngredientSnack(ingredientId, true, 2, 1.50m, Guid.NewGuid())
            };

            var productId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
 
            var itemOrder = new ItemOrder(20.00m, itemIngredients, 2, productId, orderId, Guid.NewGuid());

            var orderItems = new List<ItemOrder> { itemOrder };


            var customerId = Guid.NewGuid();
            var order = new Order(orderId, DateTime.Now, orderItems, customerId, EStatusOrder.EmPreparacao, 40.00m, null);


            var result = _presenter.TransformObject(order);


            result.Succeeded.Should().BeTrue();
            var dto = result.Data;


            dto.Id.Should().Be(order.Id);
            dto.CustomerId.Should().Be(order.CustomerId);
            dto.Price.Should().Be(order.Price);
            dto.OrderStatus.Description.Should().Be("EmPreparacao");


            dto.Itens.Should().HaveCount(1);
            var itemDto = dto.Itens.First();
            itemDto.ProductId.Should().Be(productId);
            itemDto.Quantity.Should().Be(2);
            itemDto.Price.Should().Be(20.00m);


            itemDto.Ingredients.Should().HaveCount(1);
            var ingDto = itemDto.Ingredients.First();
            ingDto.IngredientId.Should().Be(ingredientId);
            ingDto.Quantity.Should().Be(2);
            ingDto.Price.Should().Be(1.50m);
            ingDto.Additional.Should().Be(true); 

        }

        [Fact]
        public void TransformObject_DeveMapearItensSemIngredientes()
        {

            var orderItems = new List<ItemOrder>
            {

                new ItemOrder(10.00m, null, 1, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
            };

            var order = new Order(Guid.NewGuid(), DateTime.Now, orderItems, null, EStatusOrder.Recebido, 10.00m, null);


            var result = _presenter.TransformObject(order);


            result.Succeeded.Should().BeTrue();
            var itemDto = result.Data.Itens.First();
            itemDto.Ingredients.Should().NotBeNull();
            itemDto.Ingredients.Should().BeEmpty();
        }

        #endregion
    }
}