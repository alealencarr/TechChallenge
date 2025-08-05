namespace Shared.DTO.Order.Output.OrderCompleted
{
    public record IngredientItemOrderOutputDto
    {
        public Guid IngredientId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public bool Additional { get; set; }
    }
}

 