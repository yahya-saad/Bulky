using BulkyBook.DataAccess.Data;

namespace BulkyBook.DataAccess.Repository;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext dbContext;

    public ICategoryRepository Category { get; private set; }

    public IProductRepossitory Product { get; private set; }

    public ICompanyRepository Company { get; private set; }

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        this.Category = new CategoryRepository(dbContext);
        this.Product = new ProductRepository(dbContext);
        this.Company = new CompanyRepository(dbContext);
        this.dbContext = dbContext;
    }
    public void Save()
    {
        dbContext.SaveChanges();
    }
}
