using Bulky.DataAccess.Data;
using Bulky.Models;

namespace Bulky.DataAccess.Repository;
internal class CategoryRepository : Repository<Category>, ICategoryRepository
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

    public void Save()
    {
        dbContext.SaveChanges();
    }
}
