namespace Shared.DTO.Order.Output.OrderSummary
{
    public record OrderSummaryOutputDto
    {
        public Guid Id { get; set; }

        public decimal Price { get; set; }

        public OrderStatusOutputDto OrderStatus { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
