namespace OnlineBookStore.Interfaces.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(long id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<long> InsertAsync(T entity);
        void UpdateAsync(T entity);
        Task<int> SaveChangesAsync();
    }
}
