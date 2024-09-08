using Microsoft.AspNetCore.Mvc;

namespace OnlineBookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        public async Task<IServiceResponse<BookDTO>> AddBook(BookDTO bookDto)
            => await _bookService.AddBookAsync(bookDto).ConfigureAwait(false);

        [HttpGet]
        public async Task<IServiceResponse<IEnumerable<BookDTO>>> GetAllBooks()
            => await _bookService.GetAllBooksAsync().ConfigureAwait(false);

        [HttpGet("{id}")]
        public async Task<IServiceResponse<BookDTO>> GetBookById(int id)
            => await _bookService.GetBookByIdAsync(id).ConfigureAwait(false);
    }
}
