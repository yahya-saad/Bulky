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
        var existingProduct = dbContext.Products.FirstOrDefault(p => p.Id == obj.Id);
        if (existingProduct != null)
        {
            existingProduct.Title = obj.Title;
            existingProduct.ISBN = obj.ISBN;
            existingProduct.Price = obj.Price;
            existingProduct.Author = obj.Author;
            existingProduct.CategoryId = obj.CategoryId;
            existingProduct.Description = obj.Description;

            if (!string.IsNullOrEmpty(obj.ImageUrl))
                existingProduct.ImageUrl = obj.ImageUrl;
        }
    }
}
