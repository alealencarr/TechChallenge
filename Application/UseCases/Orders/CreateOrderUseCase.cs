using Application.Gateways;
using Application.UseCases.Orders.Command;
using Domain.Entities;
using Domain.Entities.Aggregates.AggregateOrder;
using Domain.Entities.Aggregates.AggregateProduct;

namespace Application.UseCases.Orders
{
    public class CreateOrderUseCase
    {
        OrderGateway _gateway = null;
        ProductGateway _gatewayProduct = null;
        IngredientGateway _gatewayIngredient = null;
        CustomerGateway _gatewayCustomer = null;
        OrderCommand _command;

        List<Product> _products;
        List<Ingredient> _ingredients;

        public static CreateOrderUseCase Create(OrderGateway gateway, CustomerGateway customerGateway, IngredientGateway ingredientGateway, ProductGateway productGateway)
        {
            return new CreateOrderUseCase(gateway, customerGateway, ingredientGateway, productGateway);
        }

        private CreateOrderUseCase(OrderGateway gateway, CustomerGateway customerGateway, IngredientGateway ingredientGateway, ProductGateway productGateway)
        {
            _gateway = gateway;
            _gatewayCustomer = customerGateway;
            _gatewayIngredient = ingredientGateway;
            _gatewayProduct = productGateway;
        }

        public async Task<Domain.Entities.Aggregates.AggregateOrder.Order> Run(OrderCommand order)
        {
            try
            {
                _command = order;

                await CommandIsValidForEntity();


                var itemsOrder = order.Itens.Select(x => new ItemOrder(x.Id, x.Quantity,
                    _products.Where(p => p.Id == x.Id).FirstOrDefault()!,
                    x.IngredientsSnack.Select(y => new IngredientSnack(y.Id, y.Quantity, _ingredients.Where(i => i.Id == y.Id).FirstOrDefault()!)).ToList()
                    )
                ).ToList();

                var orderEntity = Order.Create(order.CustomerId, itemsOrder);

                await _gateway.CreateOrder(orderEntity);

                return orderEntity;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error:{ex.Message}");
            }
        }

        private async Task CommandIsValidForEntity()
        {
            if (_command.CustomerId.HasValue)
            {
                await ValidationCustomerAndProductsAndIngredients();
            }
            else
            {
                await ValidationAnyProductsAndIngredints();
            }
        }

        private async Task ValidationCustomerAndProductsAndIngredients()
        {
            var validationErrors = new List<string>();

            List<Guid> produtosIds = _command.Itens
                   .Select(i => i.Id)
                   .Distinct()
                   .ToList();

            List<Guid> ingredientesIds = _command.Itens
                      .SelectMany(i => i.IngredientsSnack)
                      .Select(i => i.Id)
                      .Distinct()
                      .ToList();

            _products = await _gatewayProduct.GetByIds(produtosIds);
            _ingredients = await _gatewayIngredient.GetByIds(ingredientesIds);
            var customerDb = await _gatewayCustomer.GetById(_command.CustomerId ?? Guid.NewGuid());

            //await Task.WhenAll(taskProducts, taskIngredients, taskCustomer);

            //var productsDb = await taskProducts;
            //var ingredientesDb = await taskIngredients;
            //var customerDb = await taskCustomer;

            if (customerDb is null)
                validationErrors.Add($"Cliente com ID {_command.CustomerId} não encontrado.");

            if (ingredientesIds.Count != _ingredients.Count)
            {
                var ingredientesEncontradosIds = _ingredients.Select(i => i.Id).ToHashSet();
                var ingredientesNaoEncontrados = ingredientesIds
                    .Where(id => !ingredientesEncontradosIds.Contains(id))
                    .ToList();

                if (ingredientesNaoEncontrados.Any())
                {
                    var idsFaltando = string.Join(", ", ingredientesNaoEncontrados);
                    validationErrors.Add($"Os seguintes ingredientes não foram encontrados: {idsFaltando}");
                }
            }

            if (produtosIds.Count != _products.Count)
            {
                var productsEncontradosIds = _products.Select(i => i.Id).ToHashSet();
                var productsNaoEncontrados = produtosIds
                    .Where(id => !productsEncontradosIds.Contains(id))
                    .ToList();

                if (productsNaoEncontrados.Any())
                {
                    var idsFaltando = string.Join(", ", productsNaoEncontrados);
                    validationErrors.Add($"Os seguintes produtos não foram encontrados: {idsFaltando}");
                }
            }

            if (validationErrors.Any())
            {
                var mensagemDeErroConsolidada = string.Join(" | ", validationErrors);
                throw new Exception(mensagemDeErroConsolidada);
            }
        }

        private async Task ValidationAnyProductsAndIngredints()
        {
            var validationErrors = new List<string>();

            List<Guid> produtosIds = _command.Itens
                   .Select(i => i.Id)
                   .Distinct()
                   .ToList();

            List<Guid> ingredientesIds = _command.Itens
                      .SelectMany(i => i.IngredientsSnack)
                      .Select(i => i.Id)
                      .Distinct()
                      .ToList();

            _products = await _gatewayProduct.GetByIds(produtosIds);
            _ingredients = await _gatewayIngredient.GetByIds(ingredientesIds);

          

            if (ingredientesIds.Count != _ingredients.Count)
            {
                var ingredientesEncontradosIds = _ingredients.Select(i => i.Id).ToHashSet();
                var ingredientesNaoEncontrados = ingredientesIds
                    .Where(id => !ingredientesEncontradosIds.Contains(id))
                    .ToList();

                if (ingredientesNaoEncontrados.Any())
                {
                    var idsFaltando = string.Join(", ", ingredientesNaoEncontrados);
                    validationErrors.Add($"Os seguintes ingredientes não foram encontrados: {idsFaltando}");
                }
            }

            if (produtosIds.Count != _products.Count)
            {
                var productsEncontradosIds = _products.Select(i => i.Id).ToHashSet();
                var productsNaoEncontrados = produtosIds
                    .Where(id => !productsEncontradosIds.Contains(id))
                    .ToList();

                if (productsNaoEncontrados.Any())
                {
                    var idsFaltando = string.Join(", ", productsNaoEncontrados);
                    validationErrors.Add($"Os seguintes produtos não foram encontrados: {idsFaltando}");
                }
            }

            if (validationErrors.Any())
            {
                var mensagemDeErroConsolidada = string.Join(" | ", validationErrors);
                throw new Exception(mensagemDeErroConsolidada);
            }
        }

    }
}