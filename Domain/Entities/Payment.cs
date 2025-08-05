using Domain.Entities.Aggregates.AggregateOrder;
using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Entities
{
    public class Payment 
    {
        public Guid Id { get; private set; } 

        public Guid OrderId { get; private set; }
        public Order? Order { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime? PaidAt { get; private set; }

        public EPaymentMethod PaymentMethod { get; private set; }

        public EPaymentStatus PaymentStatus { get;  set; }

        public byte[] QrBytes { get; private set; } 

        public string FileName { get; private set; }

        public string PathRoot { get ; private set; }

        protected Payment() { }

        public Payment(Guid orderId, decimal amount, byte[] qrBytes)
        {
            if (amount == 0)
                throw new ArgumentNullException("É necessário que o valor seja maior que zero para criar um pagamento.");
            if(qrBytes.Length == 0)
                throw new ArgumentNullException("É necessário um QR Code para criar um pagamento.");

            Id = Guid.NewGuid();
            OrderId = orderId;
            Amount = amount;
            QrBytes = qrBytes;
            CreatedAt = DateTime.Now;
            PaymentStatus = EPaymentStatus.Pending;
            PaymentMethod = EPaymentMethod.QrCode;

            FileName = $"pedido-{orderId}.png";
            PathRoot = "qrcodes";

        }

        public Payment(Guid id, Guid orderId, decimal amount, byte[] qrBytes, DateTime createdAt, EPaymentStatus paymentStatus, EPaymentMethod paymentMethod, string fileName, string pathRoot, DateTime? paidAt)
        {
 
            Id = id;
            OrderId = orderId;
            Amount = amount;
            QrBytes = qrBytes;
            CreatedAt = createdAt;
            PaymentStatus = paymentStatus;
            PaymentMethod = paymentMethod;
            FileName = fileName;
            PathRoot = pathRoot;
            PaidAt = paidAt;

        }
    }

}
