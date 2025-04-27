using BulkyBook.DataAccess.Data;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;
public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
{
    private readonly ApplicationDbContext dbContext;

    public OrderItemRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Update(OrderItem orderItem)
    {
        dbContext.OrderItems.Update(orderItem);
    }
}
