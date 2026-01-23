using Domain.Entities;  
using Domain.Entities.Aggregates.AggregateOrder;
using Domain.Entities.Aggregates.AggregateProduct;  
using FluentAssertions;
using Xunit;

namespace Orders.UnitTests.Domain.Entities.Aggregates.AggregateOrder
{
    public class ItemOrderTests
    {
        private Product CreateMockProduct(bool isLanche, decimal price, List<ProductIngredient>? ingredients = null)
        {
            return new Product(
                Guid.NewGuid(),
                "Product Test",
                 "Desc",
                price,
                Guid.NewGuid(),
               DateTime.Now,
                ingredients ?? new List<ProductIngredient>(),
                new List<ProductImage>(),
                isLanche
            );
        }

        private IngredientSnack CreateInputIngredient(Guid id, int qty, decimal price)
        {
            return new IngredientSnack(id, false, qty, price, Guid.Empty);
        }

        [Fact]
        public void Constructor_ProdutoSimples_DeveCalcularPrecoBase_SemIngredientes()
        {
            var productPrice = 10.00m;
            var quantity = 2;
            var product = CreateMockProduct(isLanche: false, price: productPrice);

            var itemOrder = new ItemOrder(product.Id, quantity, product, null);

            itemOrder.IsLanche.Should().BeFalse();
            itemOrder.Quantity.Should().Be(quantity);
            itemOrder.Price.Should().Be(productPrice * quantity); 
            itemOrder.Ingredients.Should().BeNullOrEmpty();

            itemOrder.GetPrice().Should().Be(20.00m);
        }

        [Fact]
        public void Constructor_Lanche_DeveLancarErro_QuandoSemIngredientes()
        {
            var product = CreateMockProduct(isLanche: true, price: 10m);

            Action act = () => new ItemOrder(product.Id, 1, product, null);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Um lanche deve ter ao menos um ingrediente.");
        }

        [Fact]
        public void Constructor_Lanche_DeveSepararAdicionaisDoPadrao()
        {
         
            var ingredienteId = Guid.NewGuid();
            var productIngredients = new List<ProductIngredient>
            {
                new ProductIngredient(ingredienteId, 1) 
            };
            var product = CreateMockProduct(isLanche: true, price: 20.00m, ingredients: productIngredients);

            var inputIngredients = new List<IngredientSnack>
            {
                CreateInputIngredient(ingredienteId, 3, 5.00m)  
            };

            var itemOrder = new ItemOrder(product.Id, 1, product, inputIngredients);

            itemOrder.IsLanche.Should().BeTrue();
            itemOrder.Ingredients.Should().HaveCount(2);

            var padrao = itemOrder.Ingredients!.First(x => x.Additional == false);
            padrao.Quantity.Should().Be(1);
            padrao.IdIngredient.Should().Be(ingredienteId);

            var adicional = itemOrder.Ingredients!.First(x => x.Additional == true);
            adicional.Quantity.Should().Be(2); 
            adicional.Price.Should().Be(5.00m);
        }

        [Fact]
        public void GetPrice_DeveSomarValorDosAdicionaisAoPrecoBase()
        {
           
            var idBacon = Guid.NewGuid();
            var productIngredients = new List<ProductIngredient>
            {
                new ProductIngredient(idBacon, 1) 
            };
            var product = CreateMockProduct(isLanche: true, price: 10.00m, ingredients: productIngredients);

            var inputIngredients = new List<IngredientSnack>
            {
                CreateInputIngredient(idBacon, 3, 2.00m) 
            };

            var itemOrder = new ItemOrder(product.Id, 1, product, inputIngredients);


            var precoInicial = itemOrder.Price;

            var precoFinal = itemOrder.GetPrice();


            precoInicial.Should().Be(10.00m);
            precoFinal.Should().Be(14.00m);  
            itemOrder.Price.Should().Be(14.00m); 
        }

        [Fact]
        public void Constructor_Lanche_DeveIgnorarAdicional_QuandoQuantidadeMenorQuePadrao()
        {
            var idCarne = Guid.NewGuid();
            var productIngredients = new List<ProductIngredient>
            {
                new ProductIngredient(idCarne, 2) 
            };
            var product = CreateMockProduct(isLanche: true, price: 20.00m, ingredients: productIngredients);

            var inputIngredients = new List<IngredientSnack>
            {
                CreateInputIngredient(idCarne, 1, 5.00m) 
            };

            var itemOrder = new ItemOrder(product.Id, 1, product, inputIngredients);
            var finalPrice = itemOrder.GetPrice();

            itemOrder.Ingredients.Should().HaveCount(1);
            itemOrder.Ingredients!.First().Additional.Should().BeFalse();
            itemOrder.Ingredients!.First().Quantity.Should().Be(2); 

            finalPrice.Should().Be(20.00m); 
        }

        [Fact]
        public void ReconstructConstructor_DevePreencherPropriedades()
        {
            var price = 50m;
            var qty = 5;
            var prodId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var id = Guid.NewGuid();
            var listIng = new List<IngredientSnack>();

            var item = new ItemOrder(price, listIng, qty, prodId, orderId, id);

            item.Price.Should().Be(price);
            item.Quantity.Should().Be(qty);
            item.ProductId.Should().Be(prodId);
            item.OrderId.Should().Be(orderId);
            item.Id.Should().Be(id);
            item.Ingredients.Should().BeEquivalentTo(listIng);
        }
    }
}