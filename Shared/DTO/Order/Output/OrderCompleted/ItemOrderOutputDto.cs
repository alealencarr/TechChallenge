namespace Shared.DTO.Order.Output.OrderCompleted
{
    public record ItemOrderOutputDto
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public ICollection<IngredientItemOrderOutputDto> Ingredients { get; set; } = [];
    }
}
