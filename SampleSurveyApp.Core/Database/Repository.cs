using System;
using SQLite;
using SampleSurveyApp.Core.Extensions;

namespace SampleSurveyApp.Core.Database
{
    public interface IDatabaseItem
    {
        int Id { get; set; }
    }

    public abstract class BaseDatabaseItem : IDatabaseItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }

    public interface IRepository<T> where T : IDatabaseItem, new()
    {
        Task<T> GetById(int id);
        Task<int> DeleteAsync(T item);
        Task DeleteAllAsync();
        Task<List<T>> GetAllAsync();
        Task<int> SaveAsync(T item);
        SQLiteAsyncConnection Database { get; }
    }

    public class Repository<T> : IRepository<T> where T : IDatabaseItem, new()
    {
        readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            Console.WriteLine("Missy db: " + DatabaseConstants.DatabasePath);
            return new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
        });

        public SQLiteAsyncConnection Database => lazyInitializer.Value;

        public Repository()
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

        public Task<T> GetById(int id)
        {
            return Database.Table<T>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> DeleteAsync(T item)
        {
            return Database.DeleteAsync(item);
        }

        public Task DeleteAllAsync()
        {
            return Database.DeleteAllAsync<T>();
        }

        public Task<List<T>> GetAllAsync()
        {
            return Database.Table<T>().ToListAsync();
        }

        public Task<int> SaveAsync(T item)
        {
            if (item.Id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }
    }
}

