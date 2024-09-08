using OnlineBookStore.Models;

namespace OnlineBookStore.Interfaces
{
    public interface IBookService
    {
        Task<IServiceResponse<BookDTO>> AddBookAsync(BookDTO bookDto);
        Task<IServiceResponse<IEnumerable<BookDTO>>> GetAllBooksAsync();
        Task<IServiceResponse<BookDTO>> GetBookByIdAsync(int id);
    }
}
