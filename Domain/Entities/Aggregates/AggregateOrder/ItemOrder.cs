using Domain.Entities.Aggregates.AggregateProduct;

namespace Domain.Entities.Aggregates.AggregateOrder
{
    public class ItemOrder
    {
        protected ItemOrder() { }
        public ItemOrder(Guid productId, int quantity, Product product, List<IngredientSnack>? ingredientSnacks)
        {
            var price = product.Price;

            Id = Guid.NewGuid();
            Price = price * quantity;
            ProductId = productId;
            Quantity = quantity;
            IsLanche = product!.Categorie!.IsLanche();

            if (IsLanche)
            {
                if (ingredientSnacks is null || ingredientSnacks.Count == 0)
                    throw new ArgumentException("Um lanche deve ter ao menos um ingrediente.");

                var ingredientsGrouped = ingredientSnacks
                            .GroupBy(i => i.IdIngredient)
                            .Select(group => new
                            {
                                Id = group.Key,
                                QuantitySolicited = group.Sum(x => x.Quantity),
                            });

                var ingredientsStandard = product.ProductIngredients
                .GroupBy(pi => pi.IngredientId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

                foreach (var group in ingredientsGrouped)
                {
                    var priceIngredient = ingredientSnacks.Where(x => x.IdIngredient == group.Id).FirstOrDefault()!.Price;
                    var quantityStandard = ingredientsStandard.ContainsKey(group.Id) ? ingredientsStandard[group.Id] : 0;
                    int quantitySolicited = group.QuantitySolicited;

                    int quantityStandardUsed = Math.Min(quantityStandard, quantitySolicited);
                    if (quantityStandardUsed > 0)
                    {
                        var ingredientSnackStandard = new IngredientSnack(
                            group.Id,
                            additional: false,
                            quantityStandard,
                            priceIngredient,
                            Id
                        );

                        ToAddIngredient(ingredientSnackStandard);
                    }


                    int quantityAdditional = quantitySolicited - quantityStandard;
                    if (quantityAdditional > 0)
                    {
                        var ingredienteLancheAdicional = new IngredientSnack(
                            group.Id,
                            additional: true,
                            quantityAdditional,
                            priceIngredient,
                            Id
                        );
                        ToAddIngredient(ingredienteLancheAdicional);
                    }
                }
            }
        }


        public ItemOrder(decimal price, List<IngredientSnack>? ingredientes, int quantity, Guid productId, Guid orderId, Guid id)
        {
            Price = price;
            _ingredientes = ingredientes;
            Quantity = quantity;
            ProductId = productId;
            OrderId = orderId;
            Id = id;
        }

        internal decimal GetPrice()
        {
            var adicionais = Ingredients?.Where(x => x.Additional == true).Sum(x => x.Price * x.Quantity) ?? 0;
            Price = Price + adicionais;
            return Price;
        }
        public void ToRemoveIngredient(IngredientSnack? ingredient)
        {
            if (_ingredientes?.Count > 0)
                _ingredientes.Remove(ingredient!);
        }

        public void ToAddIngredient(IngredientSnack? ingredient)
        {
            _ingredientes?.Add(ingredient!);
        }
        public decimal Price { get; private set; } = 0M;

        private readonly List<IngredientSnack>? _ingredientes = new();
        public IReadOnlyCollection<IngredientSnack>? Ingredients => _ingredientes?.AsReadOnly();

        public int Quantity { get; set; }
        public Guid ProductId { get; private set; }
        public Guid OrderId { get; internal set; }
        public Order? Order { get; set; }
        public Guid Id { get; private set; }

        public bool IsLanche { get; set; }
    }

}


