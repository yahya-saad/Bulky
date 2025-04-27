using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;
public interface IOrderItemRepository : IRepository<OrderItem>
{
    void Update(OrderItem orderItem);
}
