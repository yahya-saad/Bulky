namespace BulkyBook.DataAccess.Repository;
public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    IProductRepossitory Product { get; }
    void Save();
}
