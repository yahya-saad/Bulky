using BulkyBook.DataAccess.Data;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;
public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    private readonly ApplicationDbContext dbContext;

    public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Update(Company obj)
    {
        dbContext.Companies.Update(obj);
    }
}
