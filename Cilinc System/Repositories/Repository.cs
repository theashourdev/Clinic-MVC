
using ClinicApp.IRepositories.IRepositories;
using ClinicApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClinicApp.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ClinicContext dbContext;
        public DbSet<T> dbSet { get; }

        public Repository(ClinicContext appDbContext)
        {
            dbContext = appDbContext;
            dbSet = dbContext.Set<T>();
        }

        // CRUD
        public void Create(T entity)
        {
            dbSet.Add(entity);
        }
        public void CreateAll(List<T> entities)
        {
            dbSet.AddRange(entities);
        }
        public void Edit(T entity)
        {
            dbSet.Update(entity);
        }
        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }
        public void DeleteAll(List<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
        public void Commit()
        {
            dbContext.SaveChanges();
        }

        public void CreateAll(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
            dbContext.SaveChanges();
        }
        public void Delete(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
            dbContext.SaveChanges();
        }

        public IEnumerable<T> Get(
                Expression<Func<T, bool>>? filter = null,
                Expression<Func<T, object>>[]? includes = null,
                bool tracked = true, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return query.ToList();
        }

        public T? GetOne(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>[]? includes = null,
            bool tracked = true)
        {
            return Get(filter, includes, tracked).FirstOrDefault();
        }

        public async Task<bool> CommitAsync()
        {
            try
            {
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
