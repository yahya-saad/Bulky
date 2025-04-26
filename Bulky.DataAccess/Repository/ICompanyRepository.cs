using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;
public interface ICompanyRepository : IRepository<Company>
{
    void Update(Company obj);
}

