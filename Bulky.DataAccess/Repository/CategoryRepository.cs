using BulkyBook.DataAccess.Data;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;
public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly ApplicationDbContext dbContext;

    public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Update(Category category)
    {
        dbContext.Categories.Update(category);
    }
}
