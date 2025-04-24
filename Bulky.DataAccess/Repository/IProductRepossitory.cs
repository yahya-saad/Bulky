using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;
public interface IProductRepossitory : IRepository<Product>
{
    void Update(Product obj);
}

