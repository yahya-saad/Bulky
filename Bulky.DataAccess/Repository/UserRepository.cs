using BulkyBook.DataAccess.Data;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;
public class UserRepository : Repository<ApplicationUser>, IUserRepository
{
    private readonly ApplicationDbContext dbContext;

    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }
}
