using Application.Interfaces.DataSources;
using Infrastructure.DbContexts;
using Infrastructure.DbModels;
using Infrastructure.DbModels.OrdersModelsAggregate;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Order.Input;
using Shared.DTO.Payment;

namespace Infrastructure.DataSources
{
    public class OrderDataSource : IOrderDataSource
    {
        private readonly AppDbContext _appDbContext;

        public OrderDataSource(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
   
        public async Task Create(OrderInputDto order)
        {
            var orderDbModel = new OrderDbModel(order.CustomerId, order.OrderStatus, order.Id, order.CreatedAt, order.Itens.Select(x => new ItemOrderDbModel(x.OrderId, x.ProductId, x.Price, x.Quantity,
                (x.IngredientsSnack is null ? new List<IngredientSnackDbModel>() :
                x.IngredientsSnack.Select(y => new IngredientSnackDbModel(y.Id, y.IngredientId, y.Additional, y.Price, y.ItemId, y.Quantity)).ToList()))).ToList(), order.Price, null);

            await _appDbContext.AddAsync(orderDbModel);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<OrderInputDto>> GetListOrders()
        {
            var orders = await _appDbContext.Order
             .AsNoTracking()
             .Include(p => p.Payment)
             .Where(x => x.StatusPedido != 1 && x.StatusPedido != 5 && x.StatusPedido != 6)
             .OrderBy(x =>
                 x.StatusPedido == 4 ? 0 :    // Pronto
                 x.StatusPedido == 3 ? 1 :    // Em Preparação
                 x.StatusPedido == 2 ? 2 :    // Recebido
                 3)                           
             .ThenBy(x => x.CreatedAt)        // Pedidos mais antigos primeiro
             .ToListAsync()
             ?? throw new Exception("Order not found by Id.");

            return orders.Select( order =>  new OrderInputDto(order.Id, order.CreatedAt, order.StatusPedido, order.Price, order.CustomerId,
                order.Itens.Select(x => new ItemOrderInputDto(x.Id, x.OrderId, x.ProductId, x.Quantity, x.Price, x.IngredientsSnack.Select(y => new IngredientSnackInputDto(y.Id, y.IdIngredient, y.ItemId, y.Additional, y.Quantity, y.Price)).ToList())).ToList(),
                (order.Payment is null ? null :
                new PaymentInputDto(order.Payment.Id, order.Payment.OrderId, order.Payment.Amount, order.Payment.CreatedAt, order.Payment.PaidAt, order.Payment.PaymentMethod, order.Payment.PaymentStatus, order.Payment.QrBytes,
                order.Payment.FileName, order.Payment.PathRoot)))).ToList();
        }        

        public async Task<OrderInputDto?> GetById(Guid id)
        {
            var order = await _appDbContext.Order.AsNoTracking().Include(i => i.Itens)
                                                                    .ThenInclude(l => l.IngredientsSnack)
                                                                    .Include(p => p.Payment).AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync() ?? throw new Exception("Order not find by Id.");

            return order is not null ? new OrderInputDto(order.Id, order.CreatedAt, order.StatusPedido, order.Price, order.CustomerId,
                order.Itens.Select(x => new ItemOrderInputDto(x.Id, x.OrderId, x.ProductId, x.Quantity, x.Price, x.IngredientsSnack.Select(y => new IngredientSnackInputDto(y.Id, y.IdIngredient, y.ItemId, y.Additional, y.Quantity, y.Price)).ToList())).ToList(),
                ( order.Payment is null ? null :
                new PaymentInputDto(order.Payment.Id, order.Payment.OrderId, order.Payment.Amount, order.Payment.CreatedAt, order.Payment.PaidAt, order.Payment.PaymentMethod, order.Payment.PaymentStatus, order.Payment.QrBytes, 
                order.Payment.FileName, order.Payment.PathRoot)) ) : null;
        }

        public async Task UpdatePayment(PaymentInputDto payment)
        {           
            var order = await _appDbContext.Order
            .Include(o => o.Payments)
            .FirstOrDefaultAsync(o => o.Id == payment.OrderId) ?? throw new Exception("Order not found.");

            var newPayment = new PaymentDbModel(
                payment.Id, payment.OrderId, payment.Amount, payment.PaymentMethod,
                payment.PaymentStatus, payment.QrBytes, payment.CreatedAt, payment.PaidAt,
                payment.FileName, payment.PathRoot
            );

            await _appDbContext.Payment.AddAsync(newPayment);

            order.PaymentId = newPayment.Id;

            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdatePaymentAndStatusOrder(PaymentInputDto payment, int statusOrder)
        {
            var order = await _appDbContext.Order
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(x => x.Id == payment.OrderId)
                ?? throw new Exception("Order not found by Id.");

            if (order.Payment is null)
                throw new Exception("É necessário primeiro gerar o pagamento no checkout do pedido.");
            
            order.Payment.Amount = payment.Amount;
            order.Payment.PaymentStatus = payment.PaymentStatus;
            // Atualiza o status do pedido
            order.StatusPedido = statusOrder;

            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateStatusOrder(Guid id, int statusOrder)
        {
            var order = await _appDbContext.Order                
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new Exception("Order not found by Id.");

            order.StatusPedido = statusOrder;

            await _appDbContext.SaveChangesAsync();
        }

        

    }
}

