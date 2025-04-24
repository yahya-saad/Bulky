using BulkyBook.DataAccess.Data;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;
public class ProductRepository : Repository<Product>, IProductRepossitory
{
    private readonly ApplicationDbContext dbContext;
    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }
    public void Update(Product obj)
    {
        dbContext.Products.Update(obj);
    }
}
