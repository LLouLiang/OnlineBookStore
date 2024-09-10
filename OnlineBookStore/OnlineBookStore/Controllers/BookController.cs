using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Post: api/addbook
        /// </summary>
        /// <param name="bookDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IServiceResponse<BookDTO>> AddBook([FromBody] BookDTO bookDto)
            => await _bookService.InsertBook(bookDto).ConfigureAwait(false);

        /// <summary>
        /// Get: api/getallbooks
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IServiceResponse<IEnumerable<BookDTO>>> GetAllBooks()
            => await _bookService.GetAllBooks().ConfigureAwait(false);

        /// <summary>
        /// Get: api/getbookbyid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IServiceResponse<BookDTO>> GetBookById([FromRoute] long id)
            => await _bookService.GetBookById(id).ConfigureAwait(false);
    }
}
