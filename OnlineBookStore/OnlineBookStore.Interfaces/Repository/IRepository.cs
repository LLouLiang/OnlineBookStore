namespace OnlineBookStore.Interfaces.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> InsertAsync(T entity);
        void Update(T entity);
        Task<int> SaveChangesAsync();
    }
}
