using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static OnlineBookStore.DTOs.Constants.Authorization;

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
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IServiceResponse<BookDTO>> AddBook(BookDTO bookDto)
            => await _bookService.InsertBook(bookDto).ConfigureAwait(false);

        [Authorize]
        [HttpGet]
        public async Task<IServiceResponse<IEnumerable<BookDTO>>> GetAllBooks()
            => await _bookService.GetAllBooks().ConfigureAwait(false);

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IServiceResponse<BookDTO>> GetBookById(long id)
            => await _bookService.GetBookById(id).ConfigureAwait(false);
    }
}
