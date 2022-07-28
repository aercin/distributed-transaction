using System.Linq.Expressions;

namespace core_domain.Interfaces
{
    public interface IGenericRepository<T> where T : class, IAggregateRoot
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void Remove(T entity);
    }
}
