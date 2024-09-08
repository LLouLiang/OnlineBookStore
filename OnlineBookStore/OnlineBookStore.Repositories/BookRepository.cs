using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Interfaces.Repository;
using OnlineBookStore.Models;
using OnlineBookStore.Repositories.Data;
using OnlineBookStore.Services.DB;

namespace OnlineBookStore.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(OnlineBookStoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(string category)
        {
            return await _dbSet.Where(book => book.Category == category).ToListAsync();
        }
    }
}
