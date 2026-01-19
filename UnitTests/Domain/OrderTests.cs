using Domain.Entities;
using Domain.Entities.Aggregates.AggregateOrder;
using Domain.Entities.Aggregates.AggregateProduct;
using Domain.Entities.Enums;
using FluentAssertions;
using Xunit;

namespace Orders.UnitTests.Domain.Entities
{
    public class OrderTests
    {

        [Fact]
        public void Deve_Criar_Pedido_Com_Sucesso()
        {

            var produto = new Product(Guid.NewGuid(), "X-Burger", "Desc", 20m, Guid.NewGuid(), DateTime.Now, new List<ProductIngredient>(), new List<ProductImage>(), false);
            var item = new ItemOrder(produto.Id, 2, produto, null); // 2x 20.00 = 40.00
            var itens = new List<ItemOrder> { item };


            var order = Order.Create(Guid.NewGuid(), itens);


            order.Should().NotBeNull();
            order.StatusOrder.Should().Be(EStatusOrder.EmAberto);
            order.Price.Should().Be(40m);
            order.Itens.Should().HaveCount(1);
        }

        [Fact]
        public void Deve_Lancar_Erro_Se_Criar_Pedido_Sem_Itens()
        {

            Action act = () => Order.Create(Guid.NewGuid(), new List<ItemOrder>());


            act.Should().Throw<ArgumentException>()
               .WithMessage("Para criar um pedido é necessário pelo menos 1 item.");
        }



        [Fact]
        public void Deve_Calcular_Preco_Item_Com_Adicionais()
        {

            var ingId = Guid.NewGuid();
            var produto = new Product(Guid.NewGuid(), "Lanche", "Desc", 20m, Guid.NewGuid(), DateTime.Now,
                new List<ProductIngredient> { new ProductIngredient(ingId, 1) }, 

                new List<ProductImage>(), true); 



            var snackRequest = new List<IngredientSnack>
            {
                new IngredientSnack(ingId, 2, 2.00m)

            };

            var item = new ItemOrder(produto.Id, 1, produto, snackRequest);
            var order = Order.Create(null, new List<ItemOrder> { item });

            order.Price.Should().Be(22m);


            item.Ingredients.Should().HaveCount(2);
            item.Ingredients!.Count(x => x.Additional).Should().Be(1);
        }

        [Fact]
        public void Deve_Lancar_Erro_Se_Lanche_Sem_Ingredientes()
        {

            var produto = new Product(Guid.NewGuid(), "Lanche", "Desc", 10m, Guid.NewGuid(), DateTime.Now,
                new List<ProductIngredient>(), new List<ProductImage>(), true); 


            Action act = () => new ItemOrder(produto.Id, 1, produto, null); 



            act.Should().Throw<ArgumentException>()
               .WithMessage("Um lanche deve ter ao menos um ingrediente.");
        }



        [Theory]
        [InlineData(EStatusOrder.EmAberto, EStatusOrder.Recebido)]
        [InlineData(EStatusOrder.Recebido, EStatusOrder.EmPreparacao)]
        [InlineData(EStatusOrder.EmPreparacao, EStatusOrder.Pronto)]
        [InlineData(EStatusOrder.Pronto, EStatusOrder.Finalizado)]
        public void Deve_Atualizar_Status_Corretamente(EStatusOrder atual, EStatusOrder proximo)
        {
            var order = new Order(Guid.NewGuid(), DateTime.Now, new List<ItemOrder>(), null, atual, 0m, null);

            order.UpdateStatus();

            order.StatusOrder.Should().Be(proximo);
        }

        [Fact]
        public void Deve_Lancar_Erro_Se_Tentar_Atualizar_Finalizado()
        {
            var order = new Order(Guid.NewGuid(), DateTime.Now, new List<ItemOrder>(), null, EStatusOrder.Finalizado, 0m, null);

            Action act = () => order.UpdateStatus();

            act.Should().Throw<Exception>().WithMessage("Este pedido já foi finalizado.");
        }
    }
}