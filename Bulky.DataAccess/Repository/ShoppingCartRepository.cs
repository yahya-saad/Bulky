using BulkyBook.DataAccess.Data;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;
public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
{
    private readonly ApplicationDbContext dbContext;

    public ShoppingCartRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Update(ShoppingCart shoppingCart)
    {
        dbContext.ShoppingCarts.Update(shoppingCart);
    }
}
