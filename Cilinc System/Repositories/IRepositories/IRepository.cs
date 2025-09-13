using System.Linq.Expressions;

namespace ClinicApp.IRepositories.IRepositories
{
    public interface IRepository<T> where T : class
    {

        void Create(T entity);

        void CreateAll(List<T> entities);

        public void Edit(T entity);
        void Commit();

        public void Delete(T entity);

        public void DeleteAll(List<T> entities);

        public IEnumerable<T> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true, string includeProperties = null);

        public T? GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);
    }
}
