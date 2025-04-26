using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;
public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
    void Update(ShoppingCart shoppingCart);
}
