using Bulky.Models;

namespace Bulky.DataAccess.Repository;
internal interface ICategoryRepository : IRepository<Category>
{
    void Update(Category category);
    void Save();
}
