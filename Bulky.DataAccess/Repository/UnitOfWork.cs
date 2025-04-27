using BulkyBook.DataAccess.Data;

namespace BulkyBook.DataAccess.Repository;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext dbContext;

    public ICategoryRepository Category { get; private set; }

    public IProductRepossitory Product { get; private set; }

    public ICompanyRepository Company { get; private set; }
    public IShoppingCartRepository ShoppingCart { get; private set; }
    public IUserRepository User { get; private set; }
    public IOrderRepository Order { get; private set; }
    public IOrderItemRepository OrderItem { get; private set; }

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        Category = new CategoryRepository(dbContext);
        Product = new ProductRepository(dbContext);
        Company = new CompanyRepository(dbContext);
        ShoppingCart = new ShoppingCartRepository(dbContext);
        User = new UserRepository(dbContext);
        Order = new OrderRepository(dbContext);
        OrderItem = new OrderItemRepository(dbContext);
        this.dbContext = dbContext;
    }
    public void Save()
    {
        dbContext.SaveChanges();
    }
}
