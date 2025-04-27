namespace BulkyBook.DataAccess.Repository;
public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    IProductRepossitory Product { get; }
    ICompanyRepository Company { get; }
    IShoppingCartRepository ShoppingCart { get; }
    IUserRepository User { get; }
    IOrderRepository Order { get; }
    IOrderItemRepository OrderItem { get; }

    void Save();
}
