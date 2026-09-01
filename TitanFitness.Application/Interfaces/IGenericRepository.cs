using System.Linq.Expressions;

namespace TitanFitness.Application.Interfaces
{
    // basic database operations used by the application layer
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);

    }


}