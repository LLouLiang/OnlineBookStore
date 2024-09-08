using OnlineBookStore.Models;

namespace OnlineBookStore.Interfaces.Repository
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetBooksByCategoryAsync(string category);
    }
}
