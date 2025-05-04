using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;
public interface IUserRepository : IRepository<ApplicationUser>
{
    void Update(ApplicationUser user);
}
