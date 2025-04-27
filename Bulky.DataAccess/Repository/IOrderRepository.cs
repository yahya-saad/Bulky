using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;
public interface IOrderRepository : IRepository<Order>
{
    void Update(Order order);
    void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
    void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId);
}
