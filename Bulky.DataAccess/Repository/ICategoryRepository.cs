using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;
public interface ICategoryRepository : IRepository<Category>
{
    void Update(Category category);
}
