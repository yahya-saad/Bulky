using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;
public interface IOrderRepository : IRepository<Order>
{
    void Update(Order order);
    void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
    void UpdateStripePaymentIntentId(int id, string paymentIntentId);
    void UpdateStripeSessionId(int id, string sessionId);
}
