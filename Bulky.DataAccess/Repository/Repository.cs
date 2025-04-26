using BulkyBook.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BulkyBook.DataAccess.Repository;
public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext dbContext;
    internal DbSet<T> dbSet;

    public Repository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
        this.dbSet = dbContext.Set<T>();
    }

    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public T? Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
    {
        IQueryable<T> query = dbSet;

        if (!tracked)
        {
            query = query.AsNoTracking();
        }

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties
                                            .Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        query = query.Where(filter);

        return query.FirstOrDefault();
    }


    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet.AsNoTracking();

        if (filter != null)
            query = query.Where(filter);

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties
                                            .Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        return query.ToList();
    }


    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
    }
}