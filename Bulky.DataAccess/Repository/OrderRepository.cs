using BulkyBook.DataAccess.Data;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;
public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly ApplicationDbContext dbContext;

    public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Update(Order order)
    {
        dbContext.Orders.Update(order);
    }

    public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
    {
        var order = dbContext.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null) return;

        order.OrderStatus = orderStatus;

        if (!string.IsNullOrEmpty(paymentStatus))
            order.PaymentStatus = paymentStatus;
    }

    public void UpdateStripePaymentIntentId(int id, string paymentIntentId)
    {
        var order = dbContext.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null) return;

        if (!string.IsNullOrEmpty(paymentIntentId))
        {
            order.PaymentIntentId = paymentIntentId;
            order.PaymentDate = DateTime.Now;
        }
    }

    public void UpdateStripeSessionId(int id, string sessionId)
    {
        var order = dbContext.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null) return;

        if (!string.IsNullOrEmpty(sessionId))
            order.SessionId = sessionId;
    }
}
