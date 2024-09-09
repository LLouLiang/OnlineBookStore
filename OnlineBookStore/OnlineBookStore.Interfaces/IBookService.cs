using OnlineBookStore.Models;

namespace OnlineBookStore.Interfaces
{
    public interface IBookService
    {
        Task<IServiceResponse<BookDTO>> InsertBook(BookDTO bookDto);
        Task<IServiceResponse<IEnumerable<BookDTO>>> GetAllBooks();
        Task<IServiceResponse<BookDTO>> GetBookById(long id);
        Task<IServiceResponse<IEnumerable<BookDTO>>> GetBooksByIds(List<long> ids);
    }
}
