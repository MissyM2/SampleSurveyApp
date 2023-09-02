using System;
using SQLite;
using SampleSurveyApp.Core.Extensions;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Linq.Expressions;
using SampleSurveyApp.Core.Services;

namespace SampleSurveyApp.Core.Database
{
    
    public class AsyncRepository<T> : IAsyncRepository<T> where T : IDatabaseItem, new()
    {
        readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            Console.WriteLine("SampleSurveyApp db: " + DatabaseConstants.DatabasePath);
            return new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
        });

        public SQLiteAsyncConnection Database => lazyInitializer.Value;

        public AsyncRepository()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(T).Name))
            {
                await Database.CreateTableAsync(typeof(T)).ConfigureAwait(false);
            }
        }

        public Task<T> GetByIdAsync(int id)
        {
            return Database.Table<T>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
           => Database.Table<T>().FirstOrDefaultAsync(predicate);

        public async Task<int> InsertAsync(T entity)
        {
            return await Database.InsertAsync(entity);
            //await Database.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(T entity)
        {
            return Database.UpdateAsync(entity);
            //return Database.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(T entity)
        {
            return Database.DeleteAsync(entity);
            //return Database.SaveChangesAsync();
        }

        public Task DeleteAllAsync()
        {
            return Database.DeleteAllAsync<T>();
        }

        public Task<List<T>> GetAllAsync()
        {
            return Database.Table<T>().ToListAsync();
        }

        

        public async Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await Database.Table<T>().Where(predicate).ToListAsync();
        }

        public Task<int> CountAllAsync() => Database.Table<T>().CountAsync();

        public Task<int> CountWhereAsync(Expression<Func<T, bool>> predicate)
            => Database.Table<T>().CountAsync(predicate);

        public AsyncTableQuery<T> AsQueryable() => Database.Table<T>();

    }
}

