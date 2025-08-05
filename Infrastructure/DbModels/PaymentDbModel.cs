namespace Infrastructure.DbModels
{
    public class PaymentDbModel
    {
        public Guid Id { get; private set; }

        public Guid OrderId { get; private set; }
        public OrderDbModel? Order { get; set; }

        public decimal Amount { get;  set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime? PaidAt { get; private set; }

        public int PaymentMethod { get; private set; }

        public int PaymentStatus { get;  set; }

        public byte[] QrBytes { get; private set; }

        public string FileName { get; private set; }

        public string PathRoot { get; private set; }

        public bool Processed = false;
        protected PaymentDbModel() { }

        public PaymentDbModel(Guid id, Guid orderId, decimal amount, int paymentMethod, int paymentStatus, byte[] qrBytes, DateTime createdAt, DateTime? paidAt, string fileName, string pathRoot)
        {
            Id = id;
            OrderId = orderId;
            Amount = amount;
            PaymentMethod = paymentMethod;
            QrBytes = qrBytes;
            CreatedAt = createdAt;
            PaymentStatus = paymentStatus;
            PaymentMethod = paymentMethod;
            PaidAt = paidAt;
            FileName = fileName;
            PathRoot = pathRoot;
        }
    }

}



