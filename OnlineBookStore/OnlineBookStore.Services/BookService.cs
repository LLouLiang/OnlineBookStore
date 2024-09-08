using AutoMapper;
using OnlineBookStore.Interfaces;
using OnlineBookStore.Interfaces.Repository;
using OnlineBookStore.Models;

namespace OnlineBookStore.Services
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public BookService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IServiceResponse<BookDTO>> AddBookAsync(BookDTO bookDto)
        {
            try
            {
                using (_unitOfWork)
                {
                    var bookEntity = _mapper.Map<Book>(bookDto);

                    await _unitOfWork.Repository<Book>().InsertAsync(bookEntity);

                    await _unitOfWork.CompleteAsync();

                    var bookDtoResponse = _mapper.Map<BookDTO>(bookEntity);

                    return new ServiceResponse<BookDTO>(true, "Book added successfully", "0001", "Book added successfully", bookDtoResponse);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<BookDTO>(false, ex.Message, "0002", ex.Message, null);
            }
        }

        public async Task<IServiceResponse<IEnumerable<BookDTO>>> GetAllBooksAsync()
        {
            try
            {
                using (_unitOfWork)
                {
                    var books = await _unitOfWork.Repository<Book>().GetAllAsync();

                    var bookDtos = _mapper.Map<IEnumerable<BookDTO>>(books);

                    return new ServiceResponse<IEnumerable<BookDTO>>(true, "Books retrieved successfully", "0001", "Books retrieved successfully", bookDtos);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<BookDTO>>(false, ex.Message, "0002", ex.Message, null);
            }
        }

        public async Task<IServiceResponse<BookDTO>> GetBookByIdAsync(int id)
        {
            try
            {
                using (_unitOfWork)
                {
                    var bookDB = await _unitOfWork.Repository<Book>().GetByIdAsync(id);

                    var bookDto = _mapper.Map<BookDTO>(bookDB);

                    return new ServiceResponse<BookDTO>(true, "Fetch book by Id successfully", "0001", "Fetch book by Id successfully", bookDto);
                }
            }
            catch(Exception ex)
            {
                return new ServiceResponse<BookDTO>(false, ex.Message, "0002", ex.Message, null);
            }
        }
    }
}
