using System;
using SQLite;
using System.Linq.Expressions;

namespace SampleSurveyApp.Core.Services
{
    public interface IAsyncRepository<T> where T : IDatabaseItem, new()
    {
        SQLiteAsyncConnection Database { get; }

        Task<T> GetByIdAsync(int id);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task<int> InsertAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T entity);

        Task DeleteAllAsync();

        Task<List<T>> GetAllAsync();
        Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);

        Task<int> CountAllAsync();
        Task<int> CountWhereAsync(Expression<Func<T, bool>> predicate);

        AsyncTableQuery<T> AsQueryable();

    }
}

